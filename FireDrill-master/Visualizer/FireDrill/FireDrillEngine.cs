using DongUtility;
using Geometry.Geometry2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visualizer.FireDrill
{
    public class FireDrillEngine
    {
        public double TimeStep { get; set; } = .01;

        public List<Person> People { get; } = new List<Person>();

        public List<Wall> Walls { get; } = new List<Wall>();

        public AlignedRectangle Floor { get; set; } = new AlignedRectangle(new Point(0, 0), new Point(1, 1));


        public void CreateRandomStudents<T>(int nStudents, AlignedRectangle region) where T : IndividualIntelligence, new()
        {
            for (int i = 0; i < nStudents; ++i)
            {
                var newPerson = new Person(this, new Point(0, 0));
                newPerson.Intelligence = new T
                {
                    Owner = newPerson
                };

                while (true)
                {
                    double x = RandomGenerator.NextDouble(region.UpperLeft.X, region.LowerRight.X);
                    double y = RandomGenerator.NextDouble(region.UpperLeft.Y, region.LowerRight.Y);
                    Point proposedPosition = new Point(x, y);
                    if (!HitsWall(proposedPosition, newPerson.PhysicalRadius) 
                        && !BreachesPersonalSpace(newPerson, proposedPosition))
                    {
                        newPerson.Position = proposedPosition;
                        People.Add(newPerson);
                        break;
                    }
                }
            }
        }

        public double Time { get; private set; }

        static private Random RandomGenerator = new Random();

        public bool Tick(double timeStep)
        {
            People.Shuffle(RandomGenerator);
            foreach (var person in People)
            {
                var proposedMove = person.GetMove(timeStep);
                // Make sure the move is legal
                if (HitsWall(proposedMove, person.PhysicalRadius) || IntersectsWall(person.Position, proposedMove) || BreachesPersonalSpace(person, proposedMove))
                {
                    proposedMove = person.Position;
                }

                person.Position = proposedMove;
            }

            Time += timeStep;

            return !RoomIsClear();
        }

        private bool HitsWall(Point proposedPosition, double radius)
        {
            return Walls.Any((x) => x.HitsWall(proposedPosition, radius));
        }

        private bool IntersectsWall(Point oldPosition, Point newPosition)
        {
            return Walls.Any((x) => x.IntersectsWall(oldPosition, newPosition));
        }

        private bool BreachesPersonalSpace(Person person, Point proposedPosition)
        {
            foreach (var otherPerson in People)
            {
                if (person == otherPerson)
                {
                    continue;
                }

                double distanceSquared = (otherPerson.Position - proposedPosition).MagnitudeSquared;

                if (distanceSquared < UtilityFunctions.Square(person.ExclusionRadius))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// CHecks if the room is entirely empty.  Easily done by requiring the door to be on the x axis, so anyone with a positive y coordinate has reached freedom
        /// </summary>
        private bool RoomIsClear()
        {
            return !(People.Any((x) => x.Position.Y <= 0));
        }
    }
}
