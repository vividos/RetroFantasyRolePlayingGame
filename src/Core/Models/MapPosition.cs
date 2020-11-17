using System.Diagnostics;

namespace Game.Core.Models
{
    /// <summary>
    /// Position on a map
    /// </summary>
    [DebuggerDisplay("X = {X}, Y = {Y}")]
    public class MapPosition
    {
        /// <summary>
        /// X coordinate of position
        /// </summary>
        public int X { get; set; } = 0;

        /// <summary>
        /// Y coordinate of position
        /// </summary>
        public int Y { get; set; } = 0;

        /// <summary>
        /// Creates a new position object without defined coordinates
        /// </summary>
        public MapPosition()
        {
            this.X = this.Y = 0;
        }

        /// <summary>
        /// Creates a new position object with given X and Y coordinates
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        public MapPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Checks if an object is equal to this position
        /// </summary>
        /// <param name="obj">object to check</param>
        /// <returns>true when they are equal, false when not</returns>
        public override bool Equals(object obj)
        {
            var other = (MapPosition)obj;
            if (obj == null)
            {
                return false;
            }

            return this.X == other.X && this.Y == other.Y;
        }

        /// <summary>
        /// Returns hash code for this position object
        /// </summary>
        /// <returns>calculated hash code</returns>
        public override int GetHashCode()
        {
            return (this.X, this.Y).GetHashCode();
        }

        /// <summary>
        /// Returns displayable text for this map position
        /// </summary>
        /// <returns>displayable text</returns>
        public override string ToString()
        {
            return $"X={this.X}/Y={this.Y}";
        }
    }
}
