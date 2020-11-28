using Game.Core.Models;
using System;
using System.Diagnostics;

namespace Game.Core.Logic
{
    /// <summary>
    /// Calculates and stores map visibility, depending on where on the map the player is and the
    /// surrounding objects that block the visibility. See also:
    /// https://www.fadden.com/tech/fast-los.html
    /// </summary>
    public class MapVisibility
    {
        /// <summary>
        /// Function to get tile info for a given map position
        /// </summary>
        private readonly Func<MapPosition, TileInfo> getTileInfoFunc;

        /// <summary>
        /// View radius in tiles
        /// </summary>
        private readonly int viewRadius;

        /// <summary>
        /// Size of map view, in tiles; must be an uneven number, since player is positioned in
        /// the center.
        /// </summary>
        private readonly int viewSize;

        /// <summary>
        /// Calculated visibility map. Each tile is visible when it doesn't have All flags set.
        /// </summary>
        private readonly RasterShadowFlags[] visibilityMap;

        /// <summary>
        /// Raster shadown flags, indicating if a tile is visible or not
        /// </summary>
        [Flags]
#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
        private enum RasterShadowFlags
#pragma warning restore S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
        {
            /// <summary>
            /// Tile is fully visible; no shadow
            /// </summary>
            None = 0,

            /// <summary>
            /// Indicates that the left edge of a tile is obscured
            /// </summary>
            LeftEdge = 1 << 1,

            /// <summary>
            /// Indicates that the bottom edge of a tile is obscured
            /// </summary>
            BottomEdge = 1 << 2,

            /// <summary>
            /// Indicates that the middle point of a tile is obscured
            /// </summary>
            MiddlePoint = 1 << 3,

            /// <summary>
            /// Indicates that tile tile is invisible (since left and bottom edges and middle
            /// point is obscured)
            /// </summary>
            All = LeftEdge | BottomEdge | MiddlePoint,

            /// <summary>
            /// Special flag to indicate that a new "line" starts
            /// </summary>
            MoveUp = 1 << 7,
        }

        /// <summary>
        /// Current player's position in the map
        /// </summary>
        private MapPosition centerPosition;

        /// <summary>
        /// Creates a new map visibility object
        /// </summary>
        /// <param name="map">map to use for tile info checks</param>
        /// <param name="getTileInfoFunc">function to get tile info</param>
        /// <param name="viewRadius">map view radius</param>
        public MapVisibility(Func<MapPosition, TileInfo> getTileInfoFunc, int viewRadius = 5)
        {
            this.getTileInfoFunc = getTileInfoFunc;
            this.viewRadius = viewRadius;
            this.viewSize = (2 * viewRadius) + 1;
            this.visibilityMap = new RasterShadowFlags[this.viewSize * this.viewSize];
        }

        /// <summary>
        /// Returns if a given position is visible, based on the currently calculated visibility
        /// map.
        /// </summary>
        /// <param name="position">position to query</param>
        /// <returns>true when visible from player point, false when not</returns>
        public bool IsVisible(MapPosition position)
        {
            int offsetX = position.X - this.centerPosition.X;
            int offsetY = position.Y - this.centerPosition.Y;

            if (offsetX < -this.viewRadius || offsetX > this.viewRadius ||
                offsetY < -this.viewRadius || offsetY > this.viewRadius)
            {
                return false;
            }

            offsetX += this.viewRadius;
            offsetY += this.viewRadius;

            // only when all flags are set, the tile is fully hidden
            return !this.visibilityMap[(offsetY * this.viewSize) + offsetX]
                .HasFlag(RasterShadowFlags.All);
        }

        /// <summary>
        /// Updates the visibility map, based on the player's position. Algorithm based on this
        /// article: https://www.fadden.com/tech/fast-los.html
        /// </summary>
        /// <param name="playerPosition">new player position</param>
        public void Update(MapPosition playerPosition)
        {
            this.centerPosition = playerPosition;

            for (int mapIndex = 0; mapIndex < this.visibilityMap.Length; mapIndex++)
            {
                this.visibilityMap[mapIndex] = RasterShadowFlags.None;
            }

            // loop over all octants
            for (int octant = 0; octant < 8; octant++)
            {
                // check all octant tiles
                for (int x = 1; x < this.viewRadius; x++)
                {
                    for (int y = 0; y <= x; y++)
                    {
                        MapPosition pos = this.PositionFromOctantXY(octant, x, y);

                        if (!this.IsBlockingVisibility(pos))
                        {
                            continue;
                        }

                        ////Debug.WriteLine($"checking octant {octant}, x={x}, y={y}, pos={pos.X}/{pos.Y}, blocking visibility");

                        // calculate offset into raster table
                        int rasterShadowTableOffset = ((x - 1) * (x + 2) / 2) + y;

                        Debug.Assert(
                            rasterShadowTableOffset < RasterShadowTableList.Length,
                            "offset calculation must not yield an invalid offset");

                        // look up raster shadow flags for this offset
                        int[] singleTable = RasterShadowTableList[rasterShadowTableOffset];

                        this.ApplyRasterShadowTable(octant, x, y, singleTable);
                    }
                }
            }
        }

        /// <summary>
        /// Applies a single raster shadow table to the visibility map
        /// </summary>
        /// <param name="octant">number of octant</param>
        /// <param name="x">octant X coordinate</param>
        /// <param name="y">octant Y coordinate</param>
        /// <param name="rasterShadowTable">raster shadow table to apply</param>
        private void ApplyRasterShadowTable(int octant, int x, int y, int[] rasterShadowTable)
        {
            // start at right of the found blocking tile
            int checkX = x + 1;
            int checkY = y;

            for (int index = 0; index < rasterShadowTable.Length; index += 2)
            {
                var flag = (RasterShadowFlags)rasterShadowTable[index];
                int length = rasterShadowTable[index + 1];

                // check if we should move up a line
                if (flag.HasFlag(RasterShadowFlags.MoveUp))
                {
                    flag &= ~RasterShadowFlags.MoveUp;

                    checkX = x + 1;
                    checkY++;

                    if (checkY > checkX)
                    {
                        checkX = checkY;
                    }
                }

                ////Debug.WriteLine($"at x={checkX}, y={checkY}, setting flag {flag}, length {length}");

                // mark tiles
                for (int tile = 0; tile < length; tile++)
                {
                    this.MergeVisibilityMapFlags(octant, checkX + tile, checkY, flag);
                }

                checkX += length;
            }
        }

        /// <summary>
        /// Sets visibility map flags for given octant, x and y coordinates.
        /// </summary>
        /// <param name="octant">number of octant</param>
        /// <param name="x">octant X coordinate</param>
        /// <param name="y">octant Y coordinate</param>
        /// <param name="flag">flag to merge</param>
        private void MergeVisibilityMapFlags(int octant, int x, int y, RasterShadowFlags flag)
        {
            MapOctantXYToRelative(octant, ref x, ref y);

            x += this.viewRadius;
            y += this.viewRadius;

            Debug.Assert(x >= 0, "mapped X coordinate must be positive");
            Debug.Assert(y >= 0, "mapped Y coordinate must be positive");

            this.visibilityMap[(y * this.viewSize) + x] |= flag;
        }

        /// <summary>
        /// Calculates position from octant, x and y coordinates.
        /// </summary>
        /// <param name="octant">number of octant</param>
        /// <param name="x">octant X coordinate</param>
        /// <param name="y">octant Y coordinate</param>
        /// <returns>calculated absolute map position</returns>
        private MapPosition PositionFromOctantXY(int octant, int x, int y)
        {
            MapOctantXYToRelative(octant, ref x, ref y);

            return new MapPosition(this.centerPosition.X + x, this.centerPosition.Y + y);
        }

        /// <summary>
        /// Returns if the tile at given position is blocking the visibility of tiles beyond it.
        /// </summary>
        /// <param name="position">map position</param>
        /// <returns>true when tile blocks visibility, false when not</returns>
        private bool IsBlockingVisibility(MapPosition position)
        {
            TileInfo tileInfo = this.getTileInfoFunc(position);
            if (tileInfo == null)
            {
                // return false here so no visibility checks are made for non-existing tiles
                return false;
            }

            return tileInfo.IsBlockingVisibility;
        }

        /// <summary>
        /// Maps an octant and the x and y coordinates to relative positions. Octants start in +x,
        /// +y and go counterclockwise around the center point.
        /// </summary>
        /// <param name="octant">octant value</param>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        private static void MapOctantXYToRelative(int octant, ref int x, ref int y)
        {
            if (octant == 1 || octant == 2 ||
                octant == 5 || octant == 6)
            {
                int temp = x;
                x = y;
                y = temp;
            }

            int quadrant = octant >> 1;
            if (quadrant == 1 || quadrant == 2)
            {
                x = -x;
            }

            if (quadrant == 2 || quadrant == 3)
            {
                y = -y;
            }
        }

        /// <summary>
        /// Raster shadow table list, which contains entries for each tile of an octant. The list
        /// starts with distance 1 from the center point, where there are two tiles, and continues
        /// with the next tiles; each next tile column has one more tile. The entries in the
        /// jagged arrays are as follows: First comes a bit flag, then a length.
        /// </summary>
        private static readonly int[][] RasterShadowTableList = new int[14][]
        {
            // at x=1, y=0,1
            new int[]
            {
                (int)RasterShadowFlags.All, 4,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.MiddlePoint | RasterShadowFlags.BottomEdge), 1,
                (int)RasterShadowFlags.All, 3,
                (int)RasterShadowFlags.MoveUp, 1,
                (int)(RasterShadowFlags.MiddlePoint | RasterShadowFlags.BottomEdge), 1,
                (int)RasterShadowFlags.All, 2,
            },
            new int[]
            {
                (int)(RasterShadowFlags.MiddlePoint | RasterShadowFlags.LeftEdge), 1,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.All), 2,
                (int)(RasterShadowFlags.MiddlePoint | RasterShadowFlags.LeftEdge), 1,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.All), 3,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.All), 2,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.All), 1,
            },
            //// at x=2, y=0,1,2
            new int[]
            {
                (int)RasterShadowFlags.All, 3,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.BottomEdge), 1,
                (int)(RasterShadowFlags.MiddlePoint | RasterShadowFlags.BottomEdge), 1,
                (int)RasterShadowFlags.All, 1,
            },
            new int[]
            {
                (int)(RasterShadowFlags.MiddlePoint | RasterShadowFlags.LeftEdge), 2,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.MiddlePoint | RasterShadowFlags.BottomEdge), 1,
                (int)RasterShadowFlags.All, 2,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.MiddlePoint | RasterShadowFlags.BottomEdge), 1,
                (int)RasterShadowFlags.All, 2,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.BottomEdge), 2,
            },
            new int[]
            {
                (int)RasterShadowFlags.LeftEdge, 1,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.All), 1,
                (int)(RasterShadowFlags.MiddlePoint | RasterShadowFlags.LeftEdge), 1,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.All), 1,
                (int)(RasterShadowFlags.MiddlePoint | RasterShadowFlags.LeftEdge), 1,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.All), 1,
            },
            //// at x=3, y=0..3
            new int[]
            {
                (int)RasterShadowFlags.All, 2,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.BottomEdge), 2,
            },
            new int[]
            {
                (int)(RasterShadowFlags.MiddlePoint | RasterShadowFlags.LeftEdge), 2,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.MiddlePoint | RasterShadowFlags.BottomEdge), 1,
                (int)RasterShadowFlags.All, 1,
            },
            new int[]
            {
                (int)(RasterShadowFlags.MiddlePoint | RasterShadowFlags.LeftEdge), 1,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.All), 2,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.MiddlePoint | RasterShadowFlags.BottomEdge), 2,
            },
            new int[]
            {
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.All), 1,
                (int)RasterShadowFlags.LeftEdge, 1,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.All), 1,
            },
            //// at x=4, y=0..4
            new int[]
            {
                (int)RasterShadowFlags.All, 1,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.BottomEdge), 1,
            },
            new int[]
            {
                (int)(RasterShadowFlags.MiddlePoint | RasterShadowFlags.LeftEdge), 1,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.BottomEdge), 1,
            },
            new int[]
            {
                (int)(RasterShadowFlags.MiddlePoint | RasterShadowFlags.LeftEdge), 1,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.MiddlePoint | RasterShadowFlags.BottomEdge), 1,
            },
            new int[]
            {
                (int)RasterShadowFlags.LeftEdge, 1,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.All), 1,
            },
            new int[]
            {
                (int)RasterShadowFlags.LeftEdge, 1,
                (int)(RasterShadowFlags.MoveUp | RasterShadowFlags.All), 1,
            },
        };
    }
}
