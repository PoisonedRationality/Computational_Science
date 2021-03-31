using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry2D
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
        public Vector2D Direction { get; }

        /// <param name="direction">A non-normalized vector pointing in the direction of the ray</param>
        public Ray(Point endpoint, Vector2D direction)
        {
            if (direction == Vector2D.NullVector())
            {
                throw new GeometryException("No direction passed to Ray");
            }
            EndPoint = endpoint;
            Direction = direction;
        }

        /// <summary>
        /// The line that contains the ray
        /// </summary>
        public Line UnderlyingLine
        {
            get
            {
                var otherPoint = GetSecondPoint();
                return new Line(EndPoint, otherPoint);
            }
        }

        /// <summary>
        /// Returns a second point, used to easily construct a line
        /// </summary>
        private Point GetSecondPoint()
        {
            var otherPosition = EndPoint.PositionVector() + Direction;
            return otherPosition.ToPoint();
        }

        public double Slope => UnderlyingLine.Slope;
        public bool IsVertical => Direction.X == 0;
        public bool IsHorizontal => Direction.Y == 0;
        
        static public bool AreParallel(Ray ray, Line line)
        {
            return Line.AreParallel(ray.UnderlyingLine, line);
        }

        static public bool AreParallel(Ray ray1, Ray ray2)
        {
            return Line.AreParallel(ray1.UnderlyingLine, ray2.UnderlyingLine);
        }

        static public Point Intersection(Ray ray, Line line)
        {
            if (!Intersect(ray, line))
                throw new GeometryException("Ray and line do not intersect!");
            return PrivateIntersection(line, ray.UnderlyingLine, out _);
        }

        static public Point Intersection(Ray ray1, Ray ray2)
        {
            if (!Intersect(ray1, ray2))
                throw new GeometryException("Ray and line do not intersect!");
            return PrivateIntersection(ray1.UnderlyingLine, ray2.UnderlyingLine, out _);
        }

        static public bool Intersect(Ray ray, Line line)
        {
            Point intersection = PrivateIntersection(line, ray.UnderlyingLine, out bool possible);
            if (!possible)
                return false;
            return ray.OnRay(intersection);
        }

        /// <summary>
        /// A private method to find the intersection of two lines, if possible
        /// </summary>
        /// <param name="possible">This is false if the lines are parallel</param>
        static private Point PrivateIntersection(Line line1, Line line2, out bool possible)
        {
            possible = !Line.AreParallel(line1, line2);

            return Line.Intersection(line1, line2);
        }

        static public bool Intersect(Ray ray1, Ray ray2)
        {
            Point intersection = PrivateIntersection(ray1.UnderlyingLine, ray2.UnderlyingLine,
                out bool possible);
            if (!possible)
                return false;

            return ray1.OnRay(intersection) && ray2.OnRay(intersection);
        }

        /// <summary>
        /// Determines if a point lies on the ray.  
        /// This assumes that the point is on the line including the ray!  
        /// Roundoff error makes it impossible to check if a point is actually on a line, so that check is not done!
        /// </summary>
        public bool OnRay(Point point)
        {
            Point point2 = GetSecondPoint();
            bool xIsGreater = point2.X > EndPoint.X;
            bool yIsGreater = point2.Y > EndPoint.Y;

            bool newXIsGreater = point.X > EndPoint.X;
            bool newYIsGreater = point.Y > EndPoint.Y;

            return xIsGreater == newXIsGreater && yIsGreater == newYIsGreater;
        }

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
            Vector2D difference = NearestPoint(point) - point;
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
