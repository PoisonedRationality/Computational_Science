using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry3D
{
    /// <summary>
    /// A triangle in three dimensions
    /// </summary>
    public class Triangle
    {
        /// <summary>
        /// The three points that make up a triangle
        /// </summary>
        public Point[] Points { get; }

        public Triangle(Point p1, Point p2, Point p3)
        {
            Points = new Point[3] { p1, p2, p3 };
        }

        /// <summary>
        /// Whether a given line segment intersects the triangle
        /// </summary>
        public bool Intersects(LineSegment segment)
        {
            if (!ContainingPlane.Intersects(segment))
            {
                return false;
            }

            var point = ContainingPlane.Intersection(segment.UnderlyingLine);
            return IsInside(point);
        }

        /// <summary>
        /// Returns whether a point known to be in the containing plane lies inside the triangle
        /// </summary>
        private bool IsInside(Point point)
        {
            var rotatedPoint = ContainingPlane.TransformTo2D(point);
            return RotatedTriangle.Inside(rotatedPoint);
        }

        /// <summary>
        /// Finds the distance from the triangle to a line segment
        /// </summary>
        public double DistanceFrom(LineSegment segment)
        {
            if (Intersects(segment))
                return 0;

            var nearestPointInPlane = ContainingPlane.NearestPoint(segment);
            if (IsInside(nearestPointInPlane))
            {
                return ContainingPlane.Distance(segment);
            }

            var line1 = new LineSegment(Points[0], Points[1]);
            var line2 = new LineSegment(Points[1], Points[2]);
            var line3 = new LineSegment(Points[2], Points[0]);

            double p1 = segment.DistanceSquared(line1);
            double p2 = segment.DistanceSquared(line2);
            double p3 = segment.DistanceSquared(line3);

            double minDistance2 = UtilityFunctions.Min(p1, p2, p3);

            return Math.Sqrt(minDistance2);
        }

        /// <summary>
        /// Returns both points (if there are two) on the line segment that lie a given distance from 
        /// </summary>
        public Tuple<Point?, Point?> PointAtDistance(LineSegment segment, double distance)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The plane which contains this triangle
        /// </summary>
        public Plane ContainingPlane
        {
            get
            {
                if (plane == null)
                    plane = new Plane(Points[0], Points[1], Points[2]);
                return plane;
            }
        }
        private Plane plane;


        /// <summary>
        /// The triangle as it exists in a 2-D plane
        /// </summary>
        private Geometry2D.Triangle RotatedTriangle
        {
            get
            {
                if (rotatedTriangle == null)
                {
                    rotatedTriangle = new Geometry2D.Triangle(
                        ContainingPlane.TransformTo2D(Points[0]),
                        ContainingPlane.TransformTo2D(Points[1]),
                        ContainingPlane.TransformTo2D(Points[2]));
                }
                return rotatedTriangle;
            }
        }
        private Geometry2D.Triangle rotatedTriangle;
    }
}
