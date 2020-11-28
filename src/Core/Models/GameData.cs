using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Game.Core.Models
{
    /// <summary>
    /// The game's data
    /// </summary>
    public class GameData
    {
        /// <summary>
        /// Game name
        /// </summary>
        public static string GameName { get; set; } = "Retro Fantasy RPG";

        /// <summary>
        /// Subtitle of the game name
        /// </summary>
        public static string GameSubtitle { get; set; } = "The Test Of Data";

        /// <summary>
        /// The filename of the save game that stores the game data
        /// </summary>
        public static string SavegameFilename { get; } = "retro-fantasy-test-of-data.json";

        #region Static game data

        /// <summary>
        /// Dictionary of map IDs and all maps in the game
        /// </summary>
        public static Dictionary<string, Map> AllMaps { get; } = new Dictionary<string, Map>()
        {
            {
                "wilderness",
                new Map
                {
                    Id = "wilderness",
                    Name = "The Wilderness",
                    TiledMapFilename = "tilemaps/wilderness.tmx",
                    EdgeType = Map.MapEdgeType.WrapAround,
                    UseVisibilityCheck = true,
                }
            },
            {
                "bobs-hut",
                new Map
                {
                    Id = "bobs-hut",
                    Name = "Bob's Hut",
                    TiledMapFilename = "tilemaps/bobs-hut.tmx",
                    EdgeType = Map.MapEdgeType.ExitMap,
                    UseVisibilityCheck = true,
                }
            },
        };

        #endregion

        #region Dynamic game data

        /// <summary>
        /// Map ID of currently loaded map
        /// </summary>
        public string CurrentMapId { get; set; }

        /// <summary>
        /// The player's (or party's) position on the current map
        /// </summary>
        public MapPosition PlayerPosition { get; set; }

        #endregion

        /// <summary>
        /// Creates a new game data instance
        /// </summary>
        /// <returns>game data instance</returns>
        public static GameData Create()
        {
            return new GameData
            {
                CurrentMapId = "wilderness",
                PlayerPosition = new MapPosition(35, 33),
            };
        }

        /// <summary>
        /// Saves game data to the stream
        /// </summary>
        /// <param name="stream">stream to save game to</param>
        public void Save(Stream stream)
        {
            string json = JsonConvert.SerializeObject(this);

            using (var writer = new StreamWriter(stream))
            {
                writer.Write(json);
            }
        }

        /// <summary>
        /// Loads game data from given stream
        /// </summary>
        /// <param name="stream">stream to load game data from</param>
        /// <returns>loaded game data</returns>
        public static GameData Load(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<GameData>(json);
            }
        }
    }
}
