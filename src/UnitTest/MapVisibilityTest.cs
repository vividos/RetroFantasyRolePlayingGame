using Game.Core.Logic;
using Game.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Game.UnitTest
{
    /// <summary>
    /// Tests for MapVisibility class
    /// </summary>
    [TestClass]
    public class MapVisibilityTest
    {
        /// <summary>
        /// Tests an empty map
        /// </summary>
        [TestMethod]
        public void TestEmptyMap()
        {
            // set up
            var mapVisibility = new MapVisibility(GetTileInfoEmptyMap, viewRadius: 5);

            // run
            var pos = new MapPosition(5, 5);
            mapVisibility.Update(pos);

            // check
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    Assert.IsTrue(mapVisibility.IsVisible(new MapPosition(x, y)));
                }
            }

            static TileInfo GetTileInfoEmptyMap(MapPosition position)
            {
                return new TileInfo
                {
                    IsBlockingVisibility = false
                };
            }
        }

        /// <summary>
        /// Tests a special case where two blocking tiles create a medium big shadow
        /// </summary>
        [TestMethod]
        public void TestTwoBlockingTiles()
        {
            // set up
            var mapVisibility = new MapVisibility(GetTileInfoBlockingMap, viewRadius: 5);

            // run
            var pos = new MapPosition(5, 5);
            mapVisibility.Update(pos);

            // check

            // some tiles must be invisible
            Assert.IsTrue(mapVisibility.IsVisible(new MapPosition(8, 8)));
            Assert.IsTrue(mapVisibility.IsVisible(new MapPosition(9, 8)));
            Assert.IsTrue(mapVisibility.IsVisible(new MapPosition(10, 8)));
            Assert.IsTrue(mapVisibility.IsVisible(new MapPosition(9, 9)));
            Assert.IsTrue(mapVisibility.IsVisible(new MapPosition(10, 9)));
            Assert.IsTrue(mapVisibility.IsVisible(new MapPosition(10, 10)));

            static TileInfo GetTileInfoBlockingMap(MapPosition arg)
            {
                return new TileInfo
                {
                    IsBlockingVisibility =
                        arg == new MapPosition(7, 7) ||
                        arg == new MapPosition(7, 8)
                };
            }
        }
    }
}
