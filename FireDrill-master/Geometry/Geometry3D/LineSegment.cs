using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry3D
{
    /// <summary>
    /// A finite line segment
    /// </summary>
    public class LineSegment
    {
        public Point Point1 { get; }
        public Point Point2 { get; }

        public LineSegment(Point p1, Point p2)
        {
            Point1 = p1;
            Point2 = p2;
        }

        /// <summary>
        /// The line that contains the line segment
        /// </summary>
        public Line UnderlyingLine => new Line(Point1, Point2);

        static public bool AreParallel(LineSegment seg1, LineSegment seg2)
        {
            return Line.AreParallel(seg1.UnderlyingLine, seg2.UnderlyingLine);
        }

        static public bool AreParallel(LineSegment seg, Line line)
        {
            return Line.AreParallel(seg.UnderlyingLine, line);
        }

        static public bool AreParallel(LineSegment seg, Ray ray)
        {
            return Line.AreParallel(seg.UnderlyingLine, ray);
        }

        /// <summary>
        /// Returns whether a point is within the bounds of the segment.  Notice that this assumes that the 
        /// point lies on the line containing the segment.  Roundoff error makes it impossible to check 
        /// whether this point lies on the line.
        /// </summary>
        public bool OnSegment(Point point)
        {
            return UtilityFunctions.UnsortedBetween(point.X, Point1.X, Point2.X)
                && UtilityFunctions.UnsortedBetween(point.Y, Point1.Y, Point2.Y)
                && UtilityFunctions.UnsortedBetween(point.Z, Point1.Z, Point2.Z);
        }

        /// <summary>
        /// Finds the nearest point to the segment. Note that this may be one of the endpoints
        /// </summary>
        public Point NearestPoint(Point point)
        {
            Point closestOnLine = UnderlyingLine.NearestPoint(point);

            if (OnSegment(closestOnLine))
            {
                return point;
            }
            else
            {
                double distanceSquared1 = Point.DistanceSquared(point, Point1);
                double distanceSquared2 = Point.DistanceSquared(point, Point2);

                return distanceSquared1 > distanceSquared2 ? Point2 : Point1;
            }
        }

        public Point NearestPoint(Line line)
        {
            return line.NearestPoint(this);
        }

        public Point NearestPoint(Ray ray)
        {
            throw new NotImplementedException();
            //return ray.NearestPoint(this);
        }

        public Point NearestPoint(LineSegment segment)
        {
            Point point = UnderlyingLine.NearestPoint(segment);
            if (OnSegment(point))
                return point;
            else
            {
                double d1 = Point.DistanceSquared(point, Point1);
                double d2 = Point.DistanceSquared(point, Point2);

                return d1 > d2 ? Point2 : Point1;
            }
        }

        /// <summary>
        /// Finds the distance squared (for efficiency) from a point to a segment
        /// </summary>
        public double DistanceSquared(Point point)
        {
            Vector difference = NearestPoint(point) - point;
            return difference.MagnitudeSquared;
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

        /// <summary>
        /// Finds the distance from a point to a segment
        /// </summary>
        public double Distance(Point point)
        {
            return Math.Sqrt(DistanceSquared(point));
        }

        public double Distance(Line line)
        {
            return Math.Sqrt(DistanceSquared(line));
        }

        public double Distance(Ray ray)
        {
            return Math.Sqrt(DistanceSquared(ray));
        }

        public double Distance(LineSegment segment)
        {
            return Math.Sqrt(DistanceSquared(segment));
        }

        /// <summary>
        /// Returns both points (if there are two) that lie a given distance from
        /// a given point not on the line.
        /// </summary>
        public Tuple<Point?, Point?> PointAtDistance(Point point, double distance)
        {
            var points = UnderlyingLine.PointAtDistance(point, distance);
            return new Tuple<Point?, Point?>(
                points.Item1 != null && OnSegment((Point)points.Item1) ? points.Item1 : null,
                points.Item2 != null && OnSegment((Point)points.Item2) ? points.Item2 : null
                );
        }

        public Vector Vector => Point2 - Point1;

        public double Length => Point.Distance(Point1, Point2);

        public Point Midpoint => new Point((Point1.X + Point2.X) / 2, (Point1.Y + Point2.Y) / 2,
            (Point1.Z + Point2.Z) / 2);
    }
}
