using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry2D
{
    /// <summary>
    /// A finite line segment
    /// </summary>
    public class LineSegment
    {
        private Point point1;
        private Point point2;

        public LineSegment(Point p1, Point p2)
        {
            point1 = p1;
            point2 = p2;
        }

        public Tuple<Point, Point> Endpoints => new Tuple<Point, Point>(point1, point2);

        /// <summary>
        /// The line that contains the line segment
        /// </summary>
        public Line UnderlyingLine => new Line(point1, point2);

        /// <summary>
        /// The slope of the line - will be NaN if the segment is vertical
        /// </summary>
        public double Slope => (point2.Y - point1.Y) / (point2.X - point1.X);

        public bool IsVertical => point1.X == point2.X;
        public bool IsHorizontal => point1.Y == point2.Y;


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
            return Line.AreParallel(seg.UnderlyingLine, ray.UnderlyingLine);
        }

        /// <summary>
        /// Returns the intersection of a line segment and a line.  Will throw a GeometryException if they do not meet.
        /// </summary>
        static public Point Intersection(LineSegment seg, Line line)
        {
            if (!Intersect(seg, line))
                throw new GeometryException("Segment and line do not intersect!");
            return PrivateIntersection(line, seg.UnderlyingLine, out _);
        }

        static public Point Intersection(LineSegment seg, Ray ray)
        {
            if (!Intersect(seg, ray))
                throw new GeometryException("Segment and line do not intersect!");
            return PrivateIntersection(ray.UnderlyingLine, seg.UnderlyingLine, out _);
        }

        static public Point Intersection(LineSegment seg1, LineSegment seg2)
        {
            if (!Intersect(seg1, seg2))
                throw new GeometryException("Segments do not intersect!");
            return PrivateIntersection(seg1.UnderlyingLine, seg2.UnderlyingLine, out _);
        }

        /// <summary>
        /// Determines whether a segment and a line intersect
        /// </summary>
        static public bool Intersect(LineSegment seg, Line line)
        {
            Point intersection = PrivateIntersection(line, seg.UnderlyingLine, out bool possible);
            if (!possible)
                return false;
            return seg.OnSegment(intersection);
        }

        static public bool Intersect(LineSegment seg, Ray ray)
        {
            Point intersection = PrivateIntersection(ray.UnderlyingLine, seg.UnderlyingLine, out bool possible);
            if (!possible)
                return false;
            return seg.OnSegment(intersection) && ray.OnRay(intersection);
        }

        /// <summary>
        /// A private method to find the intersection of two lines, if possible
        /// </summary>
        /// <param name="possible">This is false if the lines are parallel</param>
        static private Point PrivateIntersection(Line line1, Line line2, out bool possible)
        {
            possible = !Line.AreParallel(line1, line2);

            if (!possible)
            {
                // TODO Bad behavior here - should fix this design
                return new Point();
            }

            return Line.Intersection(line1, line2);
        }

        static public bool Intersect(LineSegment seg1, LineSegment seg2)
        {
            Point intersection = PrivateIntersection(seg1.UnderlyingLine, seg2.UnderlyingLine,
                out bool possible);
            if (!possible)
                return false;

            return seg1.OnSegment(intersection) && seg2.OnSegment(intersection);
        }

        /// <summary>
        /// Returns whether a point is within the bounds of the segment.  Notice that this assumes that the 
        /// point lies on the line containing the segment.  Roundoff error makes it impossible to check 
        /// whether this point lies on the line.
        /// </summary>
        public bool OnSegment(Point point)
        {
            if (IsVertical)
                return UtilityFunctions.UnsortedBetween(point.Y, point1.Y, point2.Y);
            else if (IsHorizontal)
                return UtilityFunctions.UnsortedBetween(point.X, point1.X, point2.X);
            else
                return UtilityFunctions.UnsortedBetween(point.X, point1.X, point2.X)
                    && UtilityFunctions.UnsortedBetween(point.Y, point1.Y, point2.Y);
        }

        /// <summary>
        /// Finds the nearest point to the segment. Note that this may be one of the endpoints
        /// </summary>
        public Point NearestPoint(Point point)
        {
            Point closestOnLine = UnderlyingLine.NearestPoint(point);

            if (OnSegment(closestOnLine))
            {
                return closestOnLine;
            }
            else
            {
                double distanceSquared1 = Point.DistanceSquared(point, point1);
                double distanceSquared2 = Point.DistanceSquared(point, point2);

                return distanceSquared1 > distanceSquared2 ? point2 : point1;
            }
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

        public double Length => Point.Distance(point1, point2);

        public Point Midpoint => new Point((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
    }
}
