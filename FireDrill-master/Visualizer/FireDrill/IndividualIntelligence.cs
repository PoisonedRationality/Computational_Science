using DongUtility;
using Geometry.Geometry2D;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Visualizer.FireDrill
{
    public abstract class IndividualIntelligence
    {
        private Person owner;
        public Person Owner
        {
            set
            {
                owner = value;
            }
        }
        protected double MaxSpeed { get; } = 1.8;

        protected Point Position => owner.Position;
        protected IEnumerable<Wall> Walls
        {
            get
            {
                foreach (var wall in owner.Engine.Walls)
                {
                    yield return wall;
                }
            }
        }
        protected IEnumerable<Point> LocationsOfOtherPeople
        {
            get
            {
                foreach (var person in owner.Engine.People)
                {
                    if (person != owner)
                    {
                        yield return person.Position;
                    }
                }
            }
        }

        abstract public Color Color { get; }

        protected double RadiusOfPerson => owner.PhysicalRadius;
        protected double RadiusOfPersonalZone => owner.ExclusionRadius;

        public Point GetMove(double timeStep)
        {
            var proposedPosition = ChooseNewPosition(timeStep);
            return IsLegalDistance(proposedPosition, timeStep) ? proposedPosition : owner.Position;
        }

        private bool IsLegalDistance(Point proposedPosition, double timeStep)
        {
            var displacement = proposedPosition - owner.Position;
            double distanceSquared = displacement.MagnitudeSquared;
            double maxDistance = MaxSpeed * timeStep;
            const double tolerance = 1e5;
            double difference = UtilityFunctions.Square(maxDistance) - distanceSquared ;
            return difference > -tolerance;
        }

        protected Point MoveToward(Point location, double timeStep)
        {
            var difference = location - owner.Position;
            double currentDistanceSquared = difference.MagnitudeSquared;
            double maxDistance = MaxSpeed * timeStep;
            return currentDistanceSquared < UtilityFunctions.Square(maxDistance) ? location : owner.Position + maxDistance * difference.UnitVector();
        }

        abstract protected Point ChooseNewPosition(double timeStep);
    }
}
