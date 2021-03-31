using Geometry.Geometry2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Visualizer.FireDrill
{
    public class IndividualIntelligenceADVAIT : IndividualIntelligence
    {
        public override Color Color => Colors.Red;

        protected override Point ChooseNewPosition(double timeStep)
        {
            // Access your current position like this
            bool escaped = Position.Y > 0;
            var locationList = LocationsOfOtherPeople.ToList();

            var leaderTarget = new Point(0, -.2);

            bool isLeader = true;
            var leaderPosition = Position;

            if (escaped)
            {
                return MoveToward(FindOuterLoc(new Point(20, 20), locationList), timeStep);
            }
            else
            {
                foreach (var location in locationList)
                {
                    if (location.Y <= 0)
                    {
                        double distance1 = Point.Distance(location, leaderTarget);
                        double distance2 = Point.Distance(Position, leaderTarget);
                        if (distance1 <= distance2)
                        {
                            isLeader = false;
                            leaderPosition = location;
                        }
                    }
                }

                if (isLeader)
                {
                    if (Point.Distance(Position, leaderTarget) <= 0.2)
                    {
                        return MoveToward(new Point(0, 5), timeStep);
                    }
                    else
                    {
                        return MoveToward(leaderTarget, timeStep);

                        /*if (leaderTarget.X == Position.X)
                        {
                            return MoveToward(leaderTarget, timeStep);
                        }
                        else
                        {
                            return MoveToward(new Point(leaderTarget.X, Position.Y), timeStep);

                        }*/
                    }
                }
                else
                {
                    Point distance = new Point(2.5, 2.5);

                    return MoveToward(FindInnerLoc(new Point(leaderPosition.X - distance.X, leaderPosition.Y-distance.Y), locationList), timeStep);
                   // return MoveToward(new Point(leaderPosition.X, leaderPosition.Y-4), timeStep);
                }

            }



            // Access walls like this
            /*foreach (var wall in Walls)
            {
                var slope = wall.Position.Slope;
                var endpoints = wall.Endpoints;
            }
            
            // Access other people like this
            foreach (var location in LocationsOfOtherPeople)
            {
                var xCoord = location.X;
            }
            // Or get them as a list like this
            var locationList = LocationsOfOtherPeople.ToList();


            if (escaped)
            {
                return MoveToward(new Point(20, 20), timeStep);
            }

            else
            {
                // Move toward a point like this
                return MoveToward(new Point(0, 10), timeStep);
            }*/

        }

        private Point FindOuterLoc(Point targetLoc, List<Point> points)
        {
            foreach (var location in points)
            {
                if (location.Y >= 0)
                {
                    if (location == targetLoc)
                    {
                        if (targetLoc.X < -12)
                        {
                            return FindOuterLoc(new Point(18, targetLoc.Y - 2), points);
                        }
                        else
                        {
                            return FindOuterLoc(new Point(targetLoc.X - 5, targetLoc.Y), points);
                        }
                    }
                }
            }
            return targetLoc;
        }

        private Point FindInnerLoc(Point targetLoc, List<Point> points)
        {
            foreach (var location in points)
            {
                if (location.Y < 0)
                {
                    if (location == targetLoc)
                    {
                        if (targetLoc.X < -20)
                        {
                            return FindInnerLoc(new Point(18, targetLoc.Y - 2), points);
                        }
                        else
                        {
                            return FindInnerLoc(new Point(targetLoc.X - 3, targetLoc.Y), points);
                        }
                    }
                }
            }
            return targetLoc;
        }
    }
}
