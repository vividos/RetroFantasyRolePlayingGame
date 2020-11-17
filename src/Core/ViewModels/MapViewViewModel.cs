using Game.Core.Models;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled;
using System.Linq;

namespace Game.Core.ViewModels
{
    /// <summary>
    /// View model for the map view. The view model provides the map with the TiledMap object to
    /// draw the tilemap, visibility infos about tiles, the player's map position and a list of
    /// objects to draw. When the user is in a selection mode, the selectable objects are mkared
    /// accordingly.
    /// </summary>
    internal class MapViewViewModel
    {
        /// <summary>
        /// Current game data instance
        /// </summary>
        private readonly GameData currentGameData;

        /// <summary>
        /// Content manager; used to load new tile maps.
        /// </summary>
        private readonly ContentManager contentManager;

        /// <summary>
        /// Currently loaded map
        /// </summary>
        private Map currentMap;

        /// <summary>
        /// Tiled tilemap to use for drawing
        /// </summary>
        public TiledMap TileMap { get; internal set; }

        /// <summary>
        /// Returns if tile with given position is currently visible
        /// </summary>
        /// <param name="position">map position</param>
        /// <returns>true when visible, false when not</returns>
        public bool IsTileVisible(MapPosition position)
        {
            // TODO implement
            return true;
        }

        /// <summary>
        /// Returns the map's center position. Depending on if the player is on a combat map or
        /// not, the map center may be equal to the player's position.
        /// </summary>
        public MapPosition MapCenterPosition =>
            this.currentGameData.PlayerPosition;

        /// <summary>
        /// Returns the player's position on the map.
        /// </summary>
        public MapPosition PlayerPosition =>
            this.currentGameData.PlayerPosition;

        /// <summary>
        /// Creates a new view model for the map view
        /// </summary>
        /// <param name="game">game instance</param>
        public MapViewViewModel(TheGame game)
        {
            this.currentGameData = game.CurrentGameData;
            this.contentManager = game.Content;

            this.UpdateTilemap();
        }

        /// <summary>
        /// Updates tilemap, e.g. when the current map has changed.
        /// </summary>
        public void UpdateTilemap()
        {
            string currentMapId = this.currentGameData.CurrentMapId;

            this.currentMap = GameData.AllMaps[currentMapId];

            string tiledMapFilename = this.currentMap.TiledMapFilename.Replace(".tmx", string.Empty);
            this.TileMap = this.contentManager.Load<TiledMap>(tiledMapFilename);
        }

        /// <summary>
        /// Gets tile info for a specific tile on the map
        /// </summary>
        /// <param name="position">map position of the tile</param>
        /// <returns>tile info, or null when no tile info is available</returns>
        public TileInfo GetTileInfo(MapPosition position)
        {
            if (this.TileMap == null)
            {
                return null;
            }

            int mapX = position.X;
            int mapY = position.Y;

            if (this.currentMap.EdgeType == Map.MapEdgeType.WrapAround)
            {
                if (mapX > this.TileMap.Width)
                {
                    mapX %= this.TileMap.Width;
                }

                if (mapY > this.TileMap.Height)
                {
                    mapY %= this.TileMap.Height;
                }
            }

            if (mapX < 0 ||
                mapY < 0 ||
                mapX >= this.TileMap.Width ||
                mapY >= this.TileMap.Height)
            {
                return null;
            }

            TiledMapTileLayer layer = this.TileMap.TileLayers.FirstOrDefault();
            if (layer == null)
            {
                return null;
            }

            if (!layer.TryGetTile((ushort)mapX, (ushort)mapY, out TiledMapTile? tile) ||
                !tile.HasValue)
            {
                return null;
            }

            return new TileInfo
            {
                TileIndex = tile.Value.GlobalIdentifier - 1,
                // TODO get visibility property from somewhere else
                IsBlockingVisibility = tile.Value.GlobalIdentifier == 58,
            };
        }
    }
}
