using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using VisualizerBaseClasses;
using VisualizerControl;
using VisualizerControl.Commands;
using VisualizerControl.Shapes;

namespace Visualizer.FireDrill
{
    class FireDrillVisualization : IVisualization
    {
        private FireDrillEngine engine;
        private Dictionary<Person, int> personMap = new Dictionary<Person, int>();

        public FireDrillVisualization(FireDrillEngine engine)
        {
            this.engine = engine;
        }

        public bool Continue { get; private set; }

        public double Time => engine.Time;

        public Color FloorColor { get; set; } = Colors.SandyBrown;
        public Color WallColor { get; set; } = Colors.Chartreuse;

        public double WallHeight { get; set; } = 3;
        public double PersonHeight { get; set; } = 2;

        public CommandSet<VisualizerControl.Visualizer> Initialization()
        {
            var set = new CommandSet<VisualizerControl.Visualizer>();
            int counter = 0;

            // Add floor
            var floor = engine.Floor;
            var floorObj = new ObjectPrototype(new Quadrilateral3D(
                new Vector3D(floor.UpperLeft.X, floor.UpperLeft.Y, 0),
                new Vector3D(floor.LowerLeft.X, floor.LowerLeft.Y, 0),
                new Vector3D(floor.UpperRight.X, floor.UpperRight.Y, 0),
                new Vector3D(floor.LowerRight.X, floor.LowerRight.Y, 0)
                ), new BasicMaterial(FloorColor, .03, .5));
            set.AddCommand(new AddObject(floorObj, counter++));

            // Add walls
            foreach (var wall in engine.Walls)
            {
                var wallObj = new ObjectPrototype(new Quadrilateral3D(
                    new Vector3D(wall.Endpoints.Item1.X, wall.Endpoints.Item1.Y, 0),
                    new Vector3D(wall.Endpoints.Item2.X, wall.Endpoints.Item2.Y, 0),
                    new Vector3D(wall.Endpoints.Item1.X, wall.Endpoints.Item1.Y, WallHeight),
                    new Vector3D(wall.Endpoints.Item2.X, wall.Endpoints.Item2.Y, WallHeight)
                    ), new BasicMaterial(WallColor, .03, .5));
                set.AddCommand(new AddObject(wallObj, counter++));
            }

            // Add people
            foreach (var person in engine.People)
            {
                var personObj = new ObjectPrototype(new Cylinder3D(), new BasicMaterial(person.Intelligence.Color, .03, .5));
                set.AddCommand(new AddObject(personObj, counter));
                set.AddCommand(new TransformObject(counter, new Vector(person.Position.X, person.Position.Y, PersonHeight / 2),
                    new Vector(person.PhysicalRadius, person.PhysicalRadius, PersonHeight / 2)));
                personMap.Add(person, counter);
                ++counter;
            }

            return set;
        }

        public CommandSet<VisualizerControl.Visualizer> Tick(double newTime)
        {
            var set = new CommandSet<VisualizerControl.Visualizer>();
            
            double timeStep = newTime - engine.Time;
            Continue = engine.Tick(timeStep);

            foreach (var person in engine.People)
            {
                set.AddCommand(new MoveObject(personMap[person], person.Position.X, person.Position.Y, PersonHeight / 2));
            }

            return set;
        }
    }
}
