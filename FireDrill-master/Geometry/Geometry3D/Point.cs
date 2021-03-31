using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry3D
{
    /// <summary>
    /// Simple three-dimensional point.
    /// Represents a coordinate, not a direction.
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// Implemented as a vector, since mathematically there is no difference
        /// </summary>
        private Vector underlyingData;

        public Point(double x, double y, double z)
        {
            underlyingData = new Vector(x, y, z);
        }

        static public Point Origin() => new Point(0, 0, 0);

        public double X => underlyingData.X;
        public double Y => underlyingData.Y;
        public double Z => underlyingData.Z;

        /// <summary>
        /// The distance between two points
        /// </summary>
        static public double Distance(Point p1, Point p2)
        {
            return Vector.Distance(p1.underlyingData, p2.underlyingData);
        }

        /// <summary>
        /// The distance squared (for faster computation) between two points
        /// </summary>
        static public double DistanceSquared(Point p1, Point p2)
        {
            return Vector.Distance2(p1.underlyingData, p2.underlyingData);
        }

        static public Vector operator-(Point p1, Point p2)
        {
            return p1.underlyingData - p2.underlyingData;
        }

        static public Point operator+(Point p1, Vector vec)
        {
            return (p1.underlyingData + vec).ToPoint();
        }

        /// <summary>
        /// Returns a vector pointing from the origin to the point
        /// </summary>
        public Vector PositionVector()
        {
            return underlyingData;
        }
    }

    static public class VectorExtensions
    {
        static public Point ToPoint(this Vector vec)
        {
            return new Point(vec.X, vec.Y, vec.Z);
        }
    }
}
