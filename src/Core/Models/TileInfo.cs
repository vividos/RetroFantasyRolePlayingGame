namespace Game.Core.Models
{
    /// <summary>
    /// Information about a single tile on the map
    /// </summary>
    public class TileInfo
    {
        /// <summary>
        /// Tile index in tileset
        /// </summary>
        public int TileIndex { get; set; } = 0;

        /// <summary>
        /// Indicates if the tile is solid or can be walked through
        /// </summary>
        public bool IsSolid { get; set; } = false;

        /// <summary>
        /// Indicates if the tile blocks visibility
        /// </summary>
        public bool IsBlockingVisibility { get; set; } = false;
    }
}
