using DongUtility;
using Geometry.Geometry2D;
using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.FireDrill
{
    public class Person
    {   
        public Person(FireDrillEngine engine, Point position)
        {
            this.Engine = engine;
            Position = position;
        }

        public FireDrillEngine Engine { get; }
        public IndividualIntelligence Intelligence { get; set; }

        public double PhysicalRadius { get; } = .5;
        public double ExclusionRadius { get; } = 1.8288;

        public Point GetMove(double timeStep)
        {
            return Intelligence.GetMove(timeStep);
        }

        public Point Position { get; set; }
    }
}
