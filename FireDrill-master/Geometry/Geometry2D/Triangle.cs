using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry2D
{
    public class Triangle : Shape2D
    {
        public Point[] Points { get; }

        public Triangle(Point point1, Point point2, Point point3)
        {
            Points = new Point[] { point1, point2, point3 };
        }

        public Triangle(Point[] points)
        {
            if (points.Length != 3)
            {
                throw new ArgumentException("Three points were not passed to Triangle constructor");
            }
            for (int i = 0; i < 3; ++i)
            {
                Points[i] = points[i];
            }
        }

        public override double Area
        {
            get
            {
                var triangleBase = Segment(Points[0], Points[1]);
                // Distance to the underlying line (to keep the altitude right)
                double height = triangleBase.UnderlyingLine.Distance(Points[2]);
                double baseLength = triangleBase.Length;
                return .5 * baseLength * height;
            }
        }

        public override double Perimeter
        {
            get
            {
                var segments = Segments;

                return segments[0].Length + segments[1].Length + segments[2].Length;
            }
        }

        public override Point ClosestPoint(Point point)
        {
            var segments = Segments;

            double d0 = segments[0].DistanceSquared(point);
            double d1 = segments[1].DistanceSquared(point);
            double d2 = segments[2].DistanceSquared(point);

            if (d0 < d1 && d0 < d2)
            {
                return segments[0].NearestPoint(point);
            }
            else if (d1 < d2)
            {
                return segments[1].NearestPoint(point);
            }
            else
            {
                return segments[2].NearestPoint(point);
            }
        }

        public override bool Inside(Point point)
        {
            // Algorithm courtesy of https://math.stackexchange.com/questions/51326/determining-if-an-arbitrary-point-lies-inside-a-triangle-defined-by-three-points
            bool cross1 = Cross2DZComponentPositive(Points[0], Points[1], Points[0], point);
            bool cross2 = Cross2DZComponentPositive(Points[1], Points[2], Points[1], point);
            bool cross3 = Cross2DZComponentPositive(Points[2], Points[0], Points[2], point);

            return cross1 == cross2 && cross1 == cross3;
        }

        /// <summary>
        /// Calculates whether the z-component (x1y2 - x2y1) of a two-dimensional "cross-product"
        /// of the vectors p1p2 and p3p4 is positive.  Used by Inside() routine.
        /// </summary>
        private bool Cross2DZComponentPositive(Point p1, Point p2, Point p3, Point p4)
        {
            Vector2D v1 = p2 - p1;
            Vector2D v2 = p4 - p3;
            double cross = v1.X * v2.Y - v2.X - v1.Y;
            return cross > 0;
        }

        private LineSegment Segment(Point p1, Point p2)
        {
            return new LineSegment(p1, p2);
        }

        private LineSegment[] Segments
        {
            get
            {
                return new LineSegment[]
                {
                    Segment(Points[0], Points[1]),
                    Segment(Points[1], Points[2]),
                    Segment(Points[2], Points[0])
                };
            }
        }
    }
}