using Game.Core.Models;
using Game.Core.ViewModels;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Gui;
using MonoGame.Extended.Gui.Controls;
using MonoGame.Extended.Tiled;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Views
{
    /// <summary>
    /// Map view, displaying a tile map in a square.
    /// </summary>
    internal class MapView : Control
    {
        /// <summary>
        /// View model for map view
        /// </summary>
        private readonly MapViewViewModel viewModel;

        /// <summary>
        /// Size of map view, in tiles
        /// </summary>
        private readonly Point sizeInTiles;

        /// <summary>
        /// Returns the control's children; always empty
        /// </summary>
        public override IEnumerable<Control> Children => Enumerable.Empty<Control>();

        /// <summary>
        /// Creates a new map view object
        /// </summary>
        /// <param name="viewModel">view model to use</param>
        public MapView(MapViewViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.sizeInTiles = new Point(11, 11);
        }

        /// <summary>
        /// Returns content size of the view
        /// </summary>
        /// <param name="context">gui context</param>
        /// <returns>content size</returns>
        public override Size GetContentSize(IGuiContext context)
        {
            return new Size(this.Width, this.Height);
        }

        /// <summary>
        /// Draws the tile map
        /// </summary>
        /// <param name="context">gui context</param>
        /// <param name="renderer">gui renderer</param>
        /// <param name="deltaSeconds">delta of seconds since last draw</param>
        public override void Draw(IGuiContext context, IGuiRenderer renderer, float deltaSeconds)
        {
            base.Draw(context, renderer, deltaSeconds);

            renderer.FillRectangle(this.ContentRectangle, this.BackgroundColor);

            this.DrawMapTiles(renderer);
        }

        /// <summary>
        /// Draws the map tiles
        /// </summary>
        /// <param name="renderer">gui renderer</param>
        private void DrawMapTiles(IGuiRenderer renderer)
        {
            MapPosition centerPosition = this.viewModel.MapCenterPosition;

            for (int screenX = 0; screenX < this.sizeInTiles.X; screenX++)
            {
                for (int screenY = 0; screenY < this.sizeInTiles.Y; screenY++)
                {
                    int mapX = screenX + centerPosition.X - (this.sizeInTiles.X / 2);
                    int mapY = screenY + centerPosition.Y - (this.sizeInTiles.Y / 2);
                    var position = new MapPosition(mapX, mapY);

                    if (!this.viewModel.IsTileVisible(position))
                    {
                        continue;
                    }

                    TileInfo tileInfo = this.viewModel.GetTileInfo(position);
                    if (tileInfo != null)
                    {
                        this.DrawTile(renderer, tileInfo.TileIndex, screenX, screenY);
                    }
                }
            }
        }

        /// <summary>
        /// Draws single tile
        /// </summary>
        /// <param name="tileIndex">tile with given index into tilemap tileset</param>
        /// <param name="screenX">screen X coordinates of upper left corner of tile</param>
        /// <param name="screenY">screen Y coordinates of upper left corner of tile</param>
        private void DrawTile(IGuiRenderer renderer, int tileIndex, int screenX, int screenY)
        {
            TiledMapTileset tileset = this.viewModel.TileMap.Tilesets.First();

            this.DrawTilesetTile(renderer, tileset, tileIndex, screenX, screenY);
        }

        /// <summary>
        /// Draws a single tile of given tileset and tileset index
        /// </summary>
        /// <param name="renderer">gui renderer</param>
        /// <param name="tileset">tileset to use</param>
        /// <param name="tileIndex">tile with given index into tileset</param>
        /// <param name="screenX">screen X coordinates of upper left corner of tile</param>
        /// <param name="screenY">screen Y coordinates of upper left corner of tile</param>
        private void DrawTilesetTile(IGuiRenderer renderer, TiledMapTileset tileset, int tileIndex, int screenX, int screenY)
        {
            var tileSize = new Vector2(
                (float)this.Width / this.sizeInTiles.X,
                (float)this.Height / this.sizeInTiles.Y);

            var destinationRectangle = new Rectangle(
                this.Position.X + (int)(screenX * tileSize.X),
                this.Position.Y + (int)(screenY * tileSize.Y),
                (int)(tileSize.X + 0.5),
                (int)(tileSize.Y + 0.5));

            renderer.DrawRegion(
                tileset.GetRegion(tileIndex % tileset.Columns, tileIndex / tileset.Columns),
                destinationRectangle,
                Color.White);
        }
    }
}
