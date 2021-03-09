using DongUtility;
using static DongUtility.UtilityFunctions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry3D
{
    /// <summary>
    /// An infinite geometrical line
    /// </summary>
    public class Line
    {
        // Stored internally as a ray
        private readonly Ray ray;

        public Line(Point point, Vector direction)
        {
            ray = new Ray(point, direction);
        }

        /// <summary>
        /// Constructs a line from two points
        /// </summary>
        public Line(Point point1, Point point2)
        {
            Vector direction = point2 - point1;
            ray = new Ray(point1, direction);
        }

        public Ray UnderlyingRay => ray;

        /// <summary>
        /// Finds the distance squared (for efficiency) from a point to a line
        /// </summary>
        public double DistanceSquared(Point point)
        {
            Vector difference = NearestPoint(point) - point;
            return difference.MagnitudeSquared;
        }

        /// <summary>
        /// Finds the distance from a point to a line
        /// </summary>
        public double Distance(Point point)
        {
            return Math.Sqrt(DistanceSquared(point));
        }

        public double Distance(Line line)
        {
            return Math.Sqrt(DistanceSquared(line));
        }

        public double DistanceSquared(Line line)
        {
            return DistanceSquared(NearestPoint(line));
        }

        public double DistanceSquared(Ray ray)
        {
            return DistanceSquared(NearestPoint(ray));
        }

        public double DistanceSquared(LineSegment segment)
        {
            return DistanceSquared(NearestPoint(segment));
        }

        public bool IsParallel(Line other)
        {
            return Ray.AreParallel(ray, other.ray);
        }

        static public bool AreParallel(Line line1, Line line2)
        {
            return line1.IsParallel(line2);
        }   

        static public bool AreParallel(Line line, Ray ray)
        {
            return Ray.AreParallel(ray, line);
        }

        /// <summary>
        /// Returns both points (if there are two) that lie a given distance from
        /// a given point not on the line.
        /// Generally you should check to see if it's even close enough first, to
        /// save a little time
        /// </summary>
        public Tuple<Point?, Point?> PointAtDistance(Point point, double distance)
        {
            Vector centerVec = point - UnderlyingRay.EndPoint;
            double dotProduct = Vector.Dot(UnderlyingRay.Direction, centerVec);
            double umag2 = UnderlyingRay.Direction.MagnitudeSquared;
            double cmag2 = centerVec.MagnitudeSquared;

            double discriminant = dotProduct - umag2 * (cmag2 - distance * distance);
            if (discriminant < 0)
            {
                return new Tuple<Point?, Point?>(null, null);
            }
            else if (discriminant == 0)
            {
                double coefficient = dotProduct / umag2;
                Vector answer = coefficient * UnderlyingRay.Direction + UnderlyingRay.EndPoint.PositionVector();
                return new Tuple<Point?, Point?>(answer.ToPoint(), null);
            }
            else
            {
                double rootDiscriminant = Math.Sqrt(discriminant);
                double root1 = (dotProduct + rootDiscriminant) / umag2;
                double root2 = (dotProduct - rootDiscriminant) / umag2;
                Vector answer1 = root1 * UnderlyingRay.Direction + UnderlyingRay.EndPoint.PositionVector();
                Vector answer2 = root2 * UnderlyingRay.Direction + UnderlyingRay.EndPoint.PositionVector();
                return new Tuple<Point?, Point?>(answer1.ToPoint(), answer2.ToPoint());
            }
        }

        /// <summary>
        /// Finds the nearest point on the line to a given point
        /// </summary>
        public Point NearestPoint(Point point)
        {
            var nonOptimalIntersectingLine = new LineSegment(point, ray.EndPoint);
            var vectorOnLine = nonOptimalIntersectingLine.Vector.ProjectOnto(ray.Direction);
            return (ray.EndPoint.PositionVector() + vectorOnLine).ToPoint();
        }

        public Point NearestPoint(Line line)
        {
            throw new NotImplementedException();
        }

        public Point NearestPoint(Ray ray)
        {
            Point point = NearestPoint(ray.UnderlyingLine);
            if (ray.OnRay(point))
                return point;
            else
                return ray.EndPoint;
        }

        public Point NearestPoint(LineSegment segment)
        {
            Point point = NearestPoint(segment.UnderlyingLine);
            if (segment.OnSegment(point))
            {
                return point;
            }
            else
            {
                double d1 = DistanceSquared(segment.Point1);
                double d2 = DistanceSquared(segment.Point2);
                return d1 > d2 ? segment.Point2 : segment.Point1;
            }
        }
    }
}
