using DongUtility;
using System;
using System.Drawing;
using static DongUtility.UtilityFunctions;

namespace PhysicsUtility
{
    /// <summary>
    /// A triangle that can be used to make larger objects that things bounce off
    /// </summary>
    public class Triangle
    {
        /// <summary>
        /// The three points in the triangle, wound counterclockwise
        /// </summary>
        public Vector[] Points { get; } = new Vector[3];

        public Color Color { get; }

        public bool IsTransparent { get; }

        /// <summary>
        /// The three points in the rotated reference frame
        /// </summary>
        private readonly Vector[] rotatedPoints = new Vector[3];

        /// <summary>
        /// The normal vector to the plane of the triangle, with right-handed winding
        /// </summary>
        public Vector Normal { get; }
        /// <summary>
        /// A rotation to bring points from the original frame to the rotated reference frame in which the triangle lies in the xy plane
        /// </summary>
        private readonly Rotation rotationToZ;
        /// <summary>
        /// A rotation to bring points from the rotated reference frame back to the lab reference frame
        /// </summary>
        private readonly Rotation rotationFromZ;
        /// <summary>
        /// A translation to bring point 0 to the origin
        /// </summary>
        private Vector translateToZ;

        /// <summary>
        /// Constructor. Points should be wound counterclockwise
        /// </summary>
        public Triangle(Vector point1, Vector point2, Vector point3, Color color, bool isTransparent = false)
        {
            if (point1 == point2 || point2 == point3 || point1 == point3)
                throw new ArgumentException("All three points of a triangle must be distinct!");

            Points[0] = point1;
            Points[1] = point2;
            Points[2] = point3;
            Color = color;
            IsTransparent = isTransparent;

            // Find normal
            Vector dir1 = Points[1] - Points[0];
            Vector dir2 = Points[2] - Points[0];
            Normal = Vector.Cross(dir1, dir2).UnitVector();

            // Create translations
            translateToZ = -Points[0];

            // Create rotations
            rotationFromZ = MakeRotationFromNormal(Normal);
            rotationToZ = rotationFromZ.Inverse();

            // Find rotated points
            for (int i = 0; i < 3; ++i)
            {
                rotatedPoints[i] = TransformToZ(Points[i]);
            }
        }

        static public Rotation MakeRotationFromNormal(Vector normal)
        {
            var rotation = new Rotation();
            rotation.RotateYAxis(normal.Polar);
            rotation.RotateZAxis(normal.Azimuthal);
            return rotation;
        }

        private Vector2D[] TransformRotatedPoints()
        {
            return new Vector2D[] { ConvertTo2D(rotatedPoints[0]),
                ConvertTo2D(rotatedPoints[1]),
                ConvertTo2D(rotatedPoints[2]) };
        }

        /// <summary>
        /// Returns the intersection of a given line and the triangle.
        /// Returns the null vector if they do not reach
        /// </summary>
        /// <param name="point">The starting point of the line</param>
        /// <param name="direction">The vector of the line's direction.  Magnitude dows not matter.</param>
        public Vector Intersection(Vector initialPoint, Vector finalPoint)
        {
            // Rotate so that plane of the Triangle is the xy plane
            Vector rotatedInitial = TransformToZ(initialPoint);
            Vector rotatedFinal = TransformToZ(finalPoint);

            Vector2D rotatedIntersection = GetRotatedIntersection(rotatedInitial, rotatedFinal);
            return TransformFromZ(ConvertTo3D(rotatedIntersection));
        }

        /// <summary>
        /// Returns the closest point on the triangle to the point in question
        /// </summary>
        public Vector ClosestPoint(Vector point)
        {
            Vector rotatedPoint = TransformToZ(point);
            Vector2D projection = ConvertTo2D(rotatedPoint);

            // If the projection is in the triangle, this is it
            if (InTriangle(projection))
                return TransformFromZ(ConvertTo3D(projection));

            // If not, try the lines
            var rotatedPoints2D = TransformRotatedPoints();

            double d1 = DistanceSquaredToLineSegment(rotatedPoints2D[0], rotatedPoints2D[1], projection);
            double d2 = DistanceSquaredToLineSegment(rotatedPoints2D[1], rotatedPoints2D[2], projection);
            double d3 = DistanceSquaredToLineSegment(rotatedPoints2D[0], rotatedPoints2D[2], projection);

            Vector2D closestPoint;
            if (d1 < d2 && d1 < d3)
                closestPoint = ClosestPointInPlane(rotatedPoints2D[0], rotatedPoints2D[1], projection);
            else if (d2 < d3)
                closestPoint = ClosestPointInPlane(rotatedPoints2D[1], rotatedPoints2D[2], projection);
            else
                closestPoint = ClosestPointInPlane(rotatedPoints2D[0], rotatedPoints2D[2], projection);

            return TransformFromZ(ConvertTo3D(closestPoint));
        }

        /// <summary>
        /// Determines the minimum distance from a point to any point on the triangle
        /// </summary>
        public double Distance(Vector point)
        {
            Vector rotatedPoint = TransformToZ(point);
            Vector2D projection = ConvertTo2D(rotatedPoint);

            // If the projection is in the triangle, this is just the z distance
            if (InTriangle(projection))
                return Math.Abs(point.Z);

            // If not, try the lines
            double minPlaneDistanceSquared = MinimumPlaneDistanceSquared(projection);

            // Reconstruct the rest by Pythagorean theorem
            return Math.Sqrt(minPlaneDistanceSquared + Square(point.Z));
        }

        private double MinimumPlaneDistanceSquared(Vector2D projection)
        {
            var rotatedPoints2D = TransformRotatedPoints();

            double d1 = DistanceSquaredToLineSegment(rotatedPoints2D[0], rotatedPoints2D[1], projection);
            double d2 = DistanceSquaredToLineSegment(rotatedPoints2D[1], rotatedPoints2D[2], projection);
            double d3 = DistanceSquaredToLineSegment(rotatedPoints2D[0], rotatedPoints2D[2], projection);

            return Math.Min(Math.Min(d1, d2), d3);
        }

        /// <summary>
        /// Finds the closest point in a plane from a point to a line segment (degined by points p1 and p2).
        /// </summary>
        static private Vector2D ClosestPointInPlane(Vector2D p1, Vector2D p2, Vector2D point)
        {
            // shift so that p1 is the origin
            p2 -= p1;
            point -= p1;

            // Project point onto p2
            Vector2D intersection = point.ProjectOnto(p2);

            // Check if it lies on the line, which means its distance from the origin (p1) is less than the length of p2
            if (Vector2D.SameDirection(intersection, p2) && intersection.MagnitudeSquared <= p2.MagnitudeSquared)
            {
                // Then we're done
                return intersection;
            }
            // otherwise, use the endpoint distances
            else
            {
                if (Vector2D.Distance2(point, p1) < Vector2D.Distance2(point, p2))
                {
                    return p1;
                }
                else
                {
                    return p2;
                }
            }
        }

        /// <summary>
        /// Finds the minimum distance in a plane from a point to a line segment (degined by points p1 and p2).
        /// </summary>
        static private double DistanceSquaredToLineSegment(Vector2D p1, Vector2D p2, Vector2D point)
        {
            // shift so that p1 is the origin
            p2 -= p1;
            point -= p1;

            // Project point onto p2
            Vector2D intersection = point.ProjectOnto(p2);

            // Check if it lies on the line, which means its distance from the origin (p1) is less than the length of p2
            if (Vector2D.SameDirection(intersection, p2) && intersection.MagnitudeSquared <= p2.MagnitudeSquared)
            {
                // That means just return the distance to the intersection
                return Vector2D.Distance2(point, intersection);
            }
            // otherwise, use the endpoint distances
            else
            {
                return Math.Min(Vector2D.Distance2(point, p1), Vector2D.Distance2(point, p2));
            }
        }

        /// <summary>
        /// Determines the minimum distance from a line segment to any point on the triangle. 
        /// Returns zero if the segment intersects the triangle
        /// </summary>
        public double Distance(Vector start, Vector end)
        {
            Vector rotatedStart = TransformToZ(start);
            Vector rotatedEnd = TransformToZ(end);

            // Check if points are on opposite sides
            if (Math.Sign(rotatedStart.Z) == Math.Sign(rotatedEnd.Z))
            {
                // If on the same side, it's just the minimum distance
                Vector minimum = Math.Abs(rotatedStart.Z) > Math.Abs(rotatedEnd.Z) ? rotatedEnd : rotatedStart;
                return Distance(minimum);
            }

            Vector2D rotatedIntersection = GetRotatedIntersection(rotatedStart, rotatedEnd);
            // Check to see if it is inside the triangle
            if (InTriangle(rotatedPoints[0], rotatedPoints[1], rotatedPoints[2], rotatedIntersection))
                return 0;

            // If not, check for the distance between lines
            double d1 = DistanceSquaredBetweenLines(rotatedPoints[0], rotatedPoints[1], rotatedStart, rotatedEnd);
            double d2 = DistanceSquaredBetweenLines(rotatedPoints[0], rotatedPoints[2], rotatedStart, rotatedEnd);
            double d3 = DistanceSquaredBetweenLines(rotatedPoints[1], rotatedPoints[2], rotatedStart, rotatedEnd);

            return Math.Sqrt(Math.Min(d1, Math.Min(d2, d3)));
        }

        /// <summary>
        /// Finds the difference squared between the line from p1 to p2 and the line from p3 to p4.
        /// p1 and p2 lie in the xy plane for this one
        /// </summary>
        private double DistanceSquaredBetweenLines(Vector p1, Vector p2, Vector p3, Vector p4)
        {
            // Transform origin to p1
            p2 -= p1;
            p3 -= p1;
            p4 -= p1;

            // The closest point must be perpendicular to both lines, so cross product
            Vector secondLine = p4 - p3;
            Vector crossProduct = Vector.Cross(p2, secondLine);

            // Rotate to make the crossProduct the z axis
            var rotation = MakeRotationFromNormal(crossProduct).Inverse();
            Vector rp2 = rotation.ApplyRotation(p2);
            Vector rp3 = rotation.ApplyRotation(p3);
            Vector rp4 = rotation.ApplyRotation(p4);

            Vector2D crp2 = ConvertTo2D(rp2);
            Vector2D crp3 = ConvertTo2D(rp3);
            Vector2D crp4 = ConvertTo2D(rp4);

            // Find the intersection
            Vector2D intersection;
            try
            {
                intersection = Vector2D.Intersection(Vector2D.NullVector(), ConvertTo2D(rp2),
                    ConvertTo2D(rp3), ConvertTo2D(rp4));
            }
            // For the case of parallel lines
            catch (ArgumentException)
            {
                // Since p1 is the origin, if there is no overlap p3 and p4 must be farther from the origin than p2
                // or point the opposite way


                bool overlap;

                bool sd23 = Vector2D.SameDirection(crp2, crp3);
                bool sd24 = Vector2D.SameDirection(crp2, crp4);
                if (sd23 != sd24)
                {
                    overlap = true;
                }
                else if (sd23 && sd24)
                {
                    double mag2 = crp2.MagnitudeSquared;
                    double mag3 = crp3.MagnitudeSquared;
                    double mag4 = crp4.MagnitudeSquared;
                    overlap = mag2 > mag3 || mag2 > mag4;
                }
                else
                {
                    overlap = false;
                }

                if (overlap)
                {
                    // If they overlap, it's just the z distance between them
                    return Square(rp3.Z);
                }
                else
                {
                    // If not, it's the minimum straight-line distance between endpoints
                    return MinimumDistanceSquared(Vector.NullVector(), rp2, rp3, rp4);
                }
            }

            bool between12 = BetweenInclusive(intersection, Vector2D.NullVector(), crp2);
            bool between34 = BetweenInclusive(intersection, crp3, crp4);

            // If both are between, it's a normal easy intersection
            if (between12 && between34)
            {
                return Square(rp3.Z);
            }
            // Only between one of them means a straight-line distance to the intersection
            else if (between12)
            {
                Vector intersection3D = new Vector(intersection.X, intersection.Y, 0);
                return Math.Min(Vector.Distance2(intersection3D, rp3), Vector.Distance2(intersection3D, rp4));
            }
            else if (between34)
            {
                Vector intersection3D = new Vector(intersection.X, intersection.Y, rp3.Z);
                return Math.Min(intersection3D.MagnitudeSquared, Vector.Distance2(intersection3D, rp2));
            }
            else
            {
                return MinimumDistanceSquared(Vector.NullVector(), rp2, rp3, rp4);
            }
        }

        /// <summary> Returns whether a point is within the (x,y) rectangle formed by the two boundaries.
        /// If the point lies on the line formed by the boundaries, this determined whether it lies on the
        /// segment created by the boundaries.
        /// </summary>
        static private bool BetweenInclusive(Vector2D point, Vector2D boundary1, Vector2D boundary2)
        {
            return UtilityFunctions.BetweenInclusive(point.X, Math.Min(boundary1.X, boundary2.X),
                Math.Max(boundary1.X, boundary2.X))
                && UtilityFunctions.BetweenInclusive(point.Y, Math.Min(boundary1.Y, boundary2.Y),
                Math.Max(boundary1.Y, boundary2.Y));
        }


        /// <summary>
        /// Returns the minimum distance between one of the first two points and one of the second two points, 
        /// NOT all possible pairs of points!
        /// </summary>
        static private double MinimumDistanceSquared(Vector p1, Vector p2, Vector p3, Vector p4)
        {
            double d13 = Vector.Distance2(p1, p3);
            double d14 = Vector.Distance2(p1, p4); ;
            double d23 = Vector.Distance2(p2, p3);
            double d24 = Vector.Distance2(p2, p4);

            return Math.Min(Math.Min(d13, d14), Math.Min(d23, d24));
        }

        /// <summary>
        /// Returns if the point is inside a triangle.
        /// </summary>
        public bool InTriangle(Vector point)
        {
            Vector rotatedPoint = TransformToZ(point);
            if (rotatedPoint.Z != 0)
                return false;

            return InTriangle(rotatedPoints[0], rotatedPoints[1], rotatedPoints[2], ConvertTo2D(rotatedPoint));
        }

        /// <summary>
        /// Transforms a point to the rotated reference frame, in which the triangle lies in the xy plane
        /// </summary>
        private Vector TransformToZ(Vector input)
        {
            input += translateToZ;
            input = rotationToZ.ApplyRotation(input);
            return input;
        }

        /// <summary>
        /// Transforms a point from the rotated reference frame back to the lab frame
        /// </summary>
        private Vector TransformFromZ(Vector input)
        {
            input = rotationFromZ.ApplyRotation(input);
            input -= translateToZ;
            return input;
        }

        /// <summary>
        /// Determines if the particle passed through the triangle between the initial and final points given
        /// </summary>
        public bool PassedThrough(Vector initial, Vector final)
        {
            // Rotate so that plane of the Triangle is the xy plane
            Vector rotatedInitial = TransformToZ(initial);
            Vector rotatedFinal = TransformToZ(final);

            // Check if points are on opposite sides
            if (Math.Sign(rotatedInitial.Z) == Math.Sign(rotatedFinal.Z))
                return false;

            Vector2D rotatedIntersection = GetRotatedIntersection(rotatedInitial, rotatedFinal);
            // Check to see if it is inside the triangle
            return InTriangle(rotatedPoints[0], rotatedPoints[1], rotatedPoints[2], rotatedIntersection);
        }

        static private Vector2D ConvertTo2D(Vector vec)
        {
            return new Vector2D(vec.X, vec.Y);
        }

        static private Vector ConvertTo3D(Vector2D vec)
        {
            return new Vector(vec.X, vec.Y, 0);
        }

        /// <summary>
        /// Finds the intersection of the segment connecting two points and the triangle's plane,
        /// all in the z-rotated plane.  
        /// This assumes there actually is an intersection.
        /// </summary>
        private Vector2D GetRotatedIntersection(Vector rotatedInitial, Vector rotatedFinal)
        {
            // Find where the line intersects the xy plane
            double xslope = (rotatedFinal.X - rotatedInitial.X) / (rotatedFinal.Z - rotatedInitial.Z);
            double yslope = (rotatedFinal.Y - rotatedInitial.Y) / (rotatedFinal.Z - rotatedInitial.Z);
            double zDiff = -rotatedInitial.Z;
            return new Vector2D(xslope * zDiff + rotatedInitial.X, yslope * zDiff + rotatedInitial.Y);
        }

        /// <summary>
        /// Reflects the given vector about the triangle
        /// </summary>
        public Vector Reflect(Vector input)
        {
            Vector rotatedInput = TransformToZ(input);
            rotatedInput.Z = -rotatedInput.Z;
            return TransformFromZ(rotatedInput);
        }

        /// <summary>
        /// Determines if a rotated-space 2D point lies within a triangle
        /// </summary>
        private bool InTriangle(Vector2D rotatedPoint)
        {
            return InTriangle(rotatedPoints[0], rotatedPoints[1], rotatedPoints[2], rotatedPoint);
        }

        static private bool InTriangle(Vector p1, Vector p2, Vector p3, Vector2D point)
        {
            return InTriangle(ConvertTo2D(p1), ConvertTo2D(p2), ConvertTo2D(p3), point);
        }

        /// <summary>
        /// Calculates if a given point lies within the triangle formed by the three points given
        /// All points are assumed to lie in the xy plane
        /// </summary>
        static private bool InTriangle(Vector2D p1, Vector2D p2, Vector2D p3, Vector2D point)
        {
            return CheckSameSide(p1, p2, p3, point) && CheckSameSide(p2, p3, p1, point) && CheckSameSide(p3, p1, p2, point);
        }

        /// <summary>
        /// Returns whether a point is on the same side of a line as a reference point
        /// This assumes that all points lie in the xy plane - z components are ignored
        /// </summary>
        /// <param name="p1">One of the two points that define a line</param>
        /// <param name="p2">One of the two points that define a line</param>
        /// <param name="reference">The reference point</param>
        /// <param name="point">The point we are interested in</param>
        /// <returns>True if point and reference lie on the same side of the line defined by p1 and p2</returns>
        static private bool CheckSameSide(Vector2D p1, Vector2D p2, Vector2D reference, Vector2D point)
        {
            if (p1.X == p2.X)
            {
                double splitPoint = p1.X;
                double checkRef = reference.X - splitPoint;
                double checkPoint = point.X - splitPoint;

                return Math.Sign(checkRef) == Math.Sign(checkPoint);
            }
            else
            {
                double slope = (p2.Y - p1.Y) / (p2.X - p1.X);
                double intercept = p1.Y - slope * p1.X;
                double checkRef = slope * reference.X + intercept - reference.Y;
                double checkPoint = slope * point.X + intercept - point.Y;

                return Math.Sign(checkRef) == Math.Sign(checkPoint);
            }
        }
    }
}
