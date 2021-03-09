using DongUtility;
using static DongUtility.UtilityFunctions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry2D
{
    /// <summary>
    /// An infinite geometrical line
    /// </summary>
    public class Line
    {
        // Stored internally as xCoeff * x + yCoeff * y + 1 = 0
        private readonly double xCoeff;
        private readonly double yCoeff;

        /// <summary>
        /// The slope of the line. Beware of vertical lines, which return NaN.
        /// </summary>
        public double Slope => -xCoeff / yCoeff;
        public Point YIntercept
        {
            get
            {
                if (yCoeff == 0)
                    throw new GeometryException("No y-intercept exists!");
                else
                    return new Point(0, -1 / yCoeff);
            }
        }
        public Point XIntercept
        {
            get
            {
                if (xCoeff == 0)
                    throw new GeometryException("No x-intercept exists!");
                else
                    return new Point(-1 / xCoeff, 0);
            }
        }

        public bool IsHorizontal => xCoeff == 0;
        public bool IsVertical => yCoeff == 0;

        /// <summary>
        /// Slope-intercept form constructor
        /// </summary>
        public Line(double slope, double yintercept)
        {
            xCoeff = slope / yintercept;
            yCoeff = -1 / yintercept;
        }

        /// <summary>
        /// Point-slope form constructor
        /// </summary>
        public Line(Point point, double slope) :
            this(slope, point.Y - slope * point.X)
        { }

        /// <summary>
        /// Constructs a line from two points
        /// </summary>
        public Line(Point point1, Point point2)
        {
            xCoeff = (point1.Y - point2.Y) / (point1.X * point2.Y - point1.Y * point2.X);
            yCoeff = -(1 + xCoeff * point1.X) / point1.Y;
        }

        /// <summary>
        /// Creates a horizontal line
        /// </summary>
        static public Line HorizontalLine(double yCoord)
        {
            return new Line(0, yCoord);
        }

        /// <summary>
        /// Creates a vertical line
        /// </summary>
        static public Line VerticalLine(double xCoord)
        {
            // A vertical line doesn't care which y-coordinates are used
            return new Line(new Point(xCoord, 1), new Point(xCoord, 0));
        }

        /// <summary>
        /// Finds the distance squared (for efficiency) from a point to a line
        /// </summary>
        public double DistanceSquared(Point point)
        {
            Vector2D difference = NearestPoint(point) - point;
            return difference.MagnitudeSquared;
        }

        /// <summary>
        /// Finds the distance from a point to a line
        /// </summary>
        public double Distance(Point point)
        {
            return Math.Sqrt(DistanceSquared(point));
        }

        public bool IsParallel(Line other)
        {
            if (IsVertical)
            {
                return other.IsVertical;
            }
            else
            {
                return Slope == other.Slope;
            }
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
        /// Finds the point of intersection of two lines
        /// </summary>
        static public Point Intersection(Line line1, Line line2)
        {
            if (line1.IsParallel(line2))
                throw new GeometryException("Parallel lines have no intersection");

            double x = (line1.yCoeff - line2.yCoeff) / (line1.xCoeff * line2.yCoeff - line2.xCoeff * line1.yCoeff);
            double y;
            if (line2.yCoeff == 0)
                y = -(line1.xCoeff * x + 1) / line1.yCoeff;
            else
                y = -(line2.xCoeff * x + 1) / line2.yCoeff;
            return new Point(x, y);
        }

        /// <summary>
        /// Creates a line perpendicular to the current one through the given point
        /// </summary>
        public Line PerpendicularLine(Point point)
        {
            if (IsVertical)
            {
                return HorizontalLine(point.Y);
            }
            else if (IsHorizontal)
            {
                return VerticalLine(point.X);
            }
            else
            {
                double newSlope = -1 / Slope;
                return new Line(point, newSlope);
            }
        }

        /// <summary>
        /// Finds the nearest point on the line to a given point
        /// </summary>
        public Point NearestPoint(Point point)
        {
            Line perpendicular = PerpendicularLine(point);
            return Intersection(this, perpendicular);
        }

        /// <summary>
        /// Determines whether two points are in the same half-plane from the line
        /// </summary>
        public bool SameSide(Point point1, Point point2)
        {
            bool halfPlane1 = xCoeff * point1.X + yCoeff * point1.Y + 1 > 0;
            bool halfPlane2 = xCoeff * point2.X + yCoeff * point2.Y + 1 > 0;

            return halfPlane1 == halfPlane2;
        }
    }
}
