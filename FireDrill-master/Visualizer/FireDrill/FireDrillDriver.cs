using Geometry.Geometry2D;
using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer.FireDrill
{
    static internal class FireDrillDriver
    {
        static public void Run()
        {
            const double roomRadius = 20;
            const double doorSize = 4;
            const int nStudents = 20;

            var engine = new FireDrillEngine
            {
                Floor = new AlignedRectangle(new Point(-roomRadius, roomRadius), new Point(roomRadius, -roomRadius))
            };

            // Left wall
            engine.Walls.Add(new Wall(new Point(-roomRadius, -roomRadius), new Point(-roomRadius, 0)));
            // Right wall
            engine.Walls.Add(new Wall(new Point(roomRadius, -roomRadius), new Point(roomRadius, 0)));
            // Back wall
            engine.Walls.Add(new Wall(new Point(-roomRadius, -roomRadius), new Point(roomRadius, -roomRadius)));
            // Front walls
            engine.Walls.Add(new Wall(new Point(-roomRadius, 0), new Point(-doorSize / 2, 0)));
            engine.Walls.Add(new Wall(new Point(roomRadius, 0), new Point(doorSize / 2, 0)));

            var usableRegion = new AlignedRectangle(new Point(-roomRadius, 0), new Point(roomRadius, -roomRadius));
            // Make students
            engine.CreateRandomStudents<IndividualIntelligenceADVAIT>(nStudents, usableRegion);

            var visualization = new FireDrillVisualization(engine);
            var viz = new MotionVisualizer3D.MotionVisualizer3DControl(visualization);

            viz.Show();
        }
    }
}
