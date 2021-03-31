using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry3D
{
    /// <summary>
    /// A geometrical ray, infinite in one direction
    /// </summary>
    public class Ray
    {
        public Point EndPoint { get; }
        /// <summary>
        /// A non-normalized vector pointing in the direction of the ray
        /// </summary>
        public Vector Direction { get; }

        /// <param name="direction">A non-normalized vector pointing in the direction of the ray</param>
        public Ray(Point endpoint, Vector direction)
        {
            if (direction.IsNull)
            {
                throw new GeometryException("No direction passed to Ray");
            }
            EndPoint = endpoint;
            Direction = direction;
        }

        static public bool AreParallel(Ray ray, Line line)
        {
            return AreParallel(ray, line.UnderlyingRay);
        }

        static public bool AreParallel(Ray ray1, Ray ray2)
        {
            double dotProduct = Vector.Dot(ray1.Direction.UnitVector(), ray2.Direction.UnitVector());
            return Math.Abs(dotProduct) == 1;
        }

        
        /// <summary>
        /// Determines if a point lies on the ray.  
        /// This assumes that the point is on the line including the ray!  
        /// Roundoff error makes it impossible to check if a point is actually on a line, 
        /// so that check is not done!
        /// </summary>
        public bool OnRay(Point point)
        {
            Point point2 = (EndPoint.PositionVector() + Direction).ToPoint();
            bool xIsGreater = point2.X > EndPoint.X;
            bool yIsGreater = point2.Y > EndPoint.Y;
            bool zIsGreater = point2.Z > EndPoint.Z;

            bool newXIsGreater = point.X > EndPoint.X;
            bool newYIsGreater = point.Y > EndPoint.Y;
            bool newZIsGreater = point.Z > EndPoint.Z;

            return xIsGreater == newXIsGreater && yIsGreater == newYIsGreater
                && zIsGreater == newZIsGreater;
        }

        public Line UnderlyingLine => new Line(EndPoint, Direction);

        /// <summary>
        /// Finds the nearest point to the ray. Note that this will often be the endpoint if the given point 
        /// is on the far side of the ray.
        /// </summary>
        public Point NearestPoint(Point point)
        {
            Point closestOnLine = UnderlyingLine.NearestPoint(point);

            if (OnRay(closestOnLine))
                return point;
            else
                return EndPoint;
        }

        /// <summary>
        /// Finds the distance squared (for efficiency) from a point to a ray
        /// </summary>
        public double DistanceSquared(Point point)
        {
            Vector difference = NearestPoint(point) - point;
            return difference.MagnitudeSquared;
        }

        /// <summary>
        /// Finds the distance from a point to a ray
        /// </summary>
        public double Distance(Point point)
        {
            return Math.Sqrt(DistanceSquared(point));
        }
    }
}
