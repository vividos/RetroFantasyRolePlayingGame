namespace Game.Core.Models
{
    /// <summary>
    /// Map information for one single map.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// The map's internal ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The map's display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Relative filename of the Tiled map file for the map
        /// </summary>
        public string TiledMapFilename { get; set; }

        /// <summary>
        /// Map edge type; determines what happens when player crosses the edge of the map.
        /// </summary>
        public enum MapEdgeType
        {
            /// <summary>
            /// Wrap around to the other side
            /// </summary>
            WrapAround = 0,

            /// <summary>
            /// Exits map on crossing the edge
            /// </summary>
            ExitMap = 1,

            /// <summary>
            /// Blocks when trying to cross the map edge
            /// </summary>
            Block = 2,
        }

        /// <summary>
        /// Map edge type for this map
        /// </summary>
        public MapEdgeType EdgeType { get; set; } = MapEdgeType.Block;
    }
}
