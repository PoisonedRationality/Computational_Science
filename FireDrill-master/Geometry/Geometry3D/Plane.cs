using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry3D
{
    public class Plane
    {
        // Implemented internally as coeffs.X * x + coeffs.Y * y + coeffs.Z * z + d = 0
        private readonly Vector coeffs;
        private readonly double constant;

        public Plane(Point point, Vector normal)
        {
            coeffs = normal;
            constant = -Vector.Dot(normal, point.PositionVector());
        }

        public Plane(Point p1, Point p2, Point p3) :
            this(p1, Vector.Cross(p2 - p1, p3 - p1))
        { }

        public Vector Normal => coeffs;

        private Rotation RotationTo2D
        {
            get
            {
                if (rotationTo2D == null)
                {
                    rotationTo2D = new Rotation();
                    rotationTo2D.RotateYAxis(Normal.Polar);
                    rotationTo2D.RotateZAxis(Normal.Azimuthal);
                }
                return rotationTo2D;
            }
        }

        private Rotation rotationTo2D = null;
        private Rotation rotationFrom2D = null;

        private Rotation RotationFrom2D
        {
            get
            {
                if (rotationFrom2D == null)
                {
                    rotationFrom2D = RotationTo2D.Inverse();
                }
                return rotationFrom2D;
            }
        }

        public Geometry2D.Point TransformTo2D(Point point)
        {
            var rotatedPoint = RotationTo2D.ApplyRotation(point.PositionVector());
            return new Geometry2D.Point(rotatedPoint.X, rotatedPoint.Y);
        }

        public Geometry2D.Line TransformTo2D(Line line)
        {
            var ray = TransformTo2D(line.UnderlyingRay);
            return ray.UnderlyingLine;
        }

        public Geometry2D.Ray TransformTo2D(Ray ray)
        {
            var point1 = TransformTo2D(ray.EndPoint);
            var point2inSpace = ray.EndPoint.PositionVector()
                + ray.Direction;
            var point2 = TransformTo2D(point2inSpace.ToPoint());

            return new Geometry2D.Ray(point1, point2 - point1);
        }

        public Geometry2D.LineSegment TransformTo2D(LineSegment segment)
        {
            var point1 = TransformTo2D(segment.Point1);
            var point2 = TransformTo2D(segment.Point2);

            return new Geometry2D.LineSegment(point1, point2);
        }

        public bool IsInPlane(Point point)
        {
            return Vector.Dot(coeffs, point.PositionVector()) + constant == 0;
        }

        /// <summary>
        /// Returns the point in the plane closest to the given point
        /// </summary>
        public Point NearestPoint(Point point)
        {
            if (IsInPlane(point))
                return point;

            var connectingLine = new Line(point, Normal);
            return Intersection(connectingLine);
        }

        /// <summary>
        /// Returns the point in the plane closest to the given line segment
        /// </summary>
        public Point NearestPoint(LineSegment segment)
        {
            if (Intersects(segment))
            {
                return Intersection(segment.UnderlyingLine);
            }

            double d1 = Distance2(segment.Point1);
            double d2 = Distance2(segment.Point2);

            if (d1 < d2)
                return NearestPoint(segment.Point1);
            else
                return NearestPoint(segment.Point2);
        }

        public Point Intersection(Line line)
        {
            // This needs to be redone with the new equation
            throw new NotImplementedException();

            //var dir = line.UnderlyingRay.Direction;
            //var point = line.UnderlyingRay.EndPoint;
            //// First the special case of ux = 0
            //if (dir.X == 0)
            //{
            //    if (dir.Y == 0)
            //    {
            //        // -a * x0 – b * y0 – 1
            //        double numeratorx0y0 = -coeffs.X * point.X
            //            - coeffs.Y * point.Y - 1;
            //        // c
            //        double denominatorx0y0 = coeffs.Z;
            //        TestIntersection(numeratorx0y0, denominatorx0y0);
            //        double zx0y0 = numeratorx0y0 / denominatorx0y0;
            //        return new Point(point.X, point.Y, zx0y0);
            //    }
            //    else
            //    {
            //        // c * (uz * y0 – uy * z0) – uy * (a * x0 – 1)
            //        double numeratorx0 = coeffs.Z * (dir.Z * point.Y - dir.Y * point.Z)
            //            - dir.Y * (coeffs.X * point.X - 1);
            //        //  b * uy + c * uz
            //        double denominatorx0 = coeffs.Y * dir.Y + coeffs.Z * dir.Z;
            //        TestIntersection(numeratorx0, denominatorx0);
            //        double yx0 = numeratorx0 / denominatorx0;
            //        // z = uz / uy * (y – y0) + z0
            //        double zx0 = dir.Z / dir.Y * (yx0 - point.Y) + point.Z;
            //        return new Point(point.X, yx0, zx0);
            //    }
            //}

            ////b * (uy * x0 – ux * y0) + c * (uz * x0 – ux * z0) – ux
            //double numerator =
            //    coeffs.Y * (dir.Y * point.X - dir.X * point.Y)
            //    + coeffs.Z * (dir.Z * point.X - dir.X * point.Z)
            //    - dir.X;

            //// a * ux + b * uy + c * uz
            //double denominator = coeffs.X * dir.X
            //    + coeffs.Y * dir.Y
            //    + coeffs.Z * dir.Z;

            //TestIntersection(numerator, denominator);

            //double x = numerator / denominator;
            //// y = uy / ux * (x – x0) + y0
            //double y = dir.Y / dir.X * (x - point.X) + point.Y;
            //// z = uz / ux * (x – x0) + z0
            //double z = dir.Z / dir.X * (x - point.X) + point.Z;

            //return new Point(x, y, z);            
        }

        public double Distance2 (Point point)
        {
            return (point - NearestPoint(point)).MagnitudeSquared;
        }

        public double Distance2 (Line line)
        {
            if (Intersects(line))
            {
                return 0;
            }
            else
            {
                return Distance2(line.UnderlyingRay.EndPoint);
            }
        }

        public double Distance2 (Ray ray)
        {
            if (Intersects(ray))
            {
                return 0;
            }
            else
            {
                return Distance2(ray.EndPoint);
            }
        }

        public double Distance2(LineSegment segment)
        {
            if (Intersects(segment))
            {
                return 0;
            }
            else
            {
                double d1Squared = Distance2(segment.Point1);
                double d2Squared = Distance2(segment.Point2);
                return d1Squared < d2Squared ? d1Squared : d2Squared;
            }
        }

        public double Distance(Point point)
        {
            return Math.Sqrt(Distance2(point));
        }

        public double Distance(Line line)
        {
            return Math.Sqrt(Distance2(line));
        }

        public double Distance(Ray ray)
        {
            return Math.Sqrt(Distance2(ray));
        }

        public double Distance(LineSegment segment)
        {
            return Math.Sqrt(Distance2(segment));
        }


        public bool Intersects(Line line)
        {
            return Vector.Dot(line.UnderlyingRay.Direction, coeffs) != 0;
        }

        public bool Intersects(Ray ray)
        {
            if (Intersects(ray.UnderlyingLine))
            {
                // If it points toward the plane, then a point on the ray should be closer than
                // the endpoint
                double endpointDistance2 = Distance2(ray.EndPoint);
                Point newPoint = ray.EndPoint + ray.Direction;
                double otherPointDistance2 = Distance2(newPoint);
                return otherPointDistance2 < endpointDistance2;
            }
            else
            {
                return false;
            }
        }

        public bool Intersects(LineSegment segment)
        {
            return !SameHalfSpace(segment.Point1, segment.Point2);
        }

        public bool SameHalfSpace(Point p1, Point p2)
        {
            double score1 = Vector.Dot(coeffs, p1.PositionVector()) + constant;
            double score2 = Vector.Dot(coeffs, p2.PositionVector()) + constant;

            if (score1 == 0 || score2 == 0)
                return false;

            bool space1 = score1 > 0;
            bool space2 = score2 > 0;

            return space1 == space2;
        }

        private void TestIntersection(double numerator, double denominator)
        {
            if (denominator == 0)
            {
                if (numerator == 0)
                    throw new GeometryException("Attempted to call Plane.Intersect() when line lies in the plane.");
                else
                    throw new GeometryException("Attempted to call Plane.Intersect() when lies does not intersect the plane.");
            }
        }
    }
}
