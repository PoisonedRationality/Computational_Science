using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry2D
{
    /// <summary>
    /// Simple two-dimensional point.
    /// Represents a coordinate, not a direction.
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// Implemented as a vector, since mathematically there is no difference
        /// </summary>
        private Vector2D underlyingData;

        public Point(double x, double y)
        {
            underlyingData = new Vector2D(x, y);
        }

        static public Point Origin() => new Point(0, 0);

        public double X => underlyingData.X;
        public double Y => underlyingData.Y;

        /// <summary>
        /// The distance between two points
        /// </summary>
        static public double Distance(Point p1, Point p2)
        {
            return Vector2D.Distance(p1.underlyingData, p2.underlyingData);
        }

        /// <summary>
        /// The distance squared (for faster computation) between two points
        /// </summary>
        static public double DistanceSquared(Point p1, Point p2)
        {
            return Vector2D.Distance2(p1.underlyingData, p2.underlyingData);
        }

        static public Vector2D operator-(Point p1, Point p2)
        {
            return p1.underlyingData - p2.underlyingData;
        }

        static public Point operator+(Point p1, Vector2D vec)
        {
            return (p1.underlyingData + vec).ToPoint();
        }

        static public bool operator==(Point p1, Point p2)
        {
            return p1.underlyingData == p2.underlyingData;
        }

        static public bool operator!=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        /// <summary>
        /// Returns a vector pointing from the origin to the point
        /// </summary>
        public Vector2D PositionVector()
        {
            return underlyingData;
        }
    }

    static public class Vector2DExtensions
    {
        static public Point ToPoint(this Vector2D vec)
        {
            return new Point(vec.X, vec.Y);
        }
    }
}
