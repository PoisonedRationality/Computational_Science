using System;

namespace DongUtility
{
    /// <summary>
    /// A simple two-dimensional integer coordinate
    /// </summary>
    public struct Coordinate2D : IEquatable<Coordinate2D>
    {
        public Coordinate2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Coordinate2D && Equals((Coordinate2D)obj);
        }

        public bool Equals(Coordinate2D other)
        {
            return X == other.X &&
                   Y == other.Y;
        }

        public override int GetHashCode()
        {
            // Visual Studio did this for me.  Thanks, Microsoft!
            var hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        static public bool operator ==(Coordinate2D lhs, Coordinate2D rhs)
        {
            return lhs.Equals(rhs);
        }

        static public bool operator !=(Coordinate2D lhs, Coordinate2D rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override string ToString()
        {
            return "[ " + X + ", " + Y + " ]";
        }
    }
}

