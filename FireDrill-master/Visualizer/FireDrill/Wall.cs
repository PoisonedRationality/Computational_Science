using DongUtility;
using Geometry.Geometry2D;
using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.FireDrill
{
    public class Wall
    {
        public Wall(Point p1, Point p2)
        {
            Position = new LineSegment(p1, p2);
        }

        public Wall(LineSegment position)
        {
            Position = position;
        }
        public LineSegment Position { get; }

        public bool HitsWall(Point newPosition, double radius)
        {
            // First check if it overlaps the wall
            double distanceFromWallSquared = Position.DistanceSquared(newPosition);
            return distanceFromWallSquared < UtilityFunctions.Square(radius);
        }

        public bool IntersectsWall(Point oldPosition, Point newPosition)
        {
            if (oldPosition == newPosition)
                return false;

            var displacement = new LineSegment(newPosition, oldPosition);
            return LineSegment.Intersect(displacement, Position);
        }

        public Tuple<Point, Point> Endpoints => Position.Endpoints;
    }
}
