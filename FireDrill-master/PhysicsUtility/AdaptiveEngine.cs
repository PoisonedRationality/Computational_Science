using DongUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsUtility
{
    public class AdaptiveEngine : KinematicsEngine
    {
        public override bool Increment(double timeIncrement)
        {
            SubdivideTrajectory(timeIncrement);

            return CheckStopConditions();
        }

        public double Tolerance { get; set; } = .001;

        private bool SubdivideTrajectory(double timeIncrement)
        {
            double diffSquared = TestDifferenceSquared(timeIncrement);
            if (diffSquared < UtilityFunctions.Square(Tolerance))
            {
                return base.Increment(timeIncrement);
            }
            else
            {
                SubdivideTrajectory(timeIncrement / 2);
                return SubdivideTrajectory(timeIncrement / 2);
            }
        }

        private double TestDifferenceSquared(double timeIncrement)
        {
            double maxDifference = 0;
            foreach (var proj in Projectiles)
            {
                Vector position1step = TestProjectile(proj, timeIncrement, 1);
                Vector position2step = TestProjectile(proj, timeIncrement, 2);

                double difference = (position1step - position2step).MagnitudeSquared;
                if (difference > maxDifference)
                {
                    maxDifference = difference;
                }
            }

            return maxDifference;
        }

        private Vector TestProjectile(Projectile proj, double timeIncrement, int nSteps)
        {
            double oldTime = Time;
            var positions = new List<Vector>();
            var velocities = new List<Vector>();
            int index = Projectiles.FindIndex((a) => a == proj);

            foreach (var projectile in Projectiles)
            {
                positions.Add(projectile.Position);
                velocities.Add(projectile.Velocity);
            }

            RunEngine(timeIncrement, nSteps);
            Vector response = Projectiles[index].Position;

            // Reset everything
            for (int i = 0; i < Projectiles.Count; ++i)
            {
                Projectiles[i].Position = positions[i];
                Projectiles[i].Velocity = velocities[i];
            }
            Time = oldTime;

            return response;
        }

        private void RunEngine(double timeIncrement, int nSteps)
        {
            for (int i = 0; i < nSteps; ++i)
            {
                base.Increment(timeIncrement / nSteps);
            }
        }
    }
}
