using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Geometry2D
{
    /// <summary>
    /// A rectangle whose sides are parallel to the x and y axes
    /// </summary>
    public class AlignedRectangle : Shape2D
    {
        public Point UpperLeft { get; }
        public Point LowerRight { get; }
        public Point UpperRight => new Point(LowerRight.X, UpperLeft.Y);
        public Point LowerLeft => new Point(UpperLeft.X, LowerRight.Y);

        public AlignedRectangle(Point upperLeft, Point lowerRight)
        {
            UpperLeft = upperLeft;
            LowerRight = lowerRight;
        }

        public double Width => LowerRight.X - UpperLeft.X;
        public double Height => UpperLeft.Y - LowerRight.Y;

        public override double Area => Width * Height;

        public override double Perimeter => 2 * (Width + Height);

        public override Point ClosestPoint(Point point)
        {
            throw new NotImplementedException();
        }

        public override bool Inside(Point point)
        {
            return UtilityFunctions.Between(point.X, UpperLeft.X, LowerRight.X)
                && UtilityFunctions.Between(point.Y, LowerRight.Y, UpperLeft.Y);
        }
    }
}
