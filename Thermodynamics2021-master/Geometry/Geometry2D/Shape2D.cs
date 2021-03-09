using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry2D
{
    /// <summary>
    /// A closed two-dimensional shape
    /// </summary>
    public abstract class Shape2D
    {
        abstract public double Area { get; }

        abstract public double Perimeter { get; }

        /// <summary>
        /// Returns whether a given point is inside the shape
        /// </summary>
        abstract public bool Inside(Point point);

        /// <summary>
        /// Returns the point on the shape boundary that is closes to the given point
        /// </summary>
        abstract public Point ClosestPoint(Point point);

        /// <summary>
        /// Returns the distance squared (for performance) from a given point to the nearest point on the shape boundary.
        /// </summary>
        virtual public double DistanceSquared(Point point)
        {
            Vector2D difference = ClosestPoint(point) - point;
            return difference.MagnitudeSquared;
        }

        /// <summary>
        /// Returns the distance from a given point to the nearest point on the shape boundary.
        /// </summary>
        virtual public double Distance(Point point)
        {
            return Math.Sqrt(DistanceSquared(point));
        }
    }
}
