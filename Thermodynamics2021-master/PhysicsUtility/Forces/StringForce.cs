using System;
using DongUtility;

namespace PhysicsUtility
{
    /// <summary>
    /// A force simulating a fixed-length string which does not allow a particle to get any farther from the anchor point
    /// </summary>
    public class StringForce : SingleProjectileForce
    {
        private readonly double stringLength;
        private KinematicsEngine engine;
        private Vector stringPosition;

        private Vector forceLastTime = Vector.NullVector();
        private double formerTime = 0;

        public StringForce(Projectile proj, Vector stringPosition, double stringLength, KinematicsEngine engine) :
            base(proj)
        {
            this.stringPosition = stringPosition;
            this.stringLength = stringLength;
            this.engine = engine;
        }

        protected Vector getStringPosition()
        {
            return stringPosition;
        }

        override protected Vector GetForce()
        {
            Vector radial = Particle.Position - getStringPosition();
            double distance = radial.Magnitude;

            Vector response;

            if (distance < stringLength)
            {
                response = Vector.NullVector();
            }
            else
            {
                double timeIncrement = engine.Time - formerTime;

                // Adjust for velocity
                Vector radialUnit = radial.UnitVector();
                Vector radialVelocity = radialUnit * Vector.Dot(Particle.Velocity, radialUnit);
                Vector counterAcc = radialVelocity / (-timeIncrement);
                Vector counterForce1 = counterAcc * Particle.Mass;

                // Adjust for other forces
                Vector netForce = Particle.Acceleration * Particle.Mass;
                netForce -= forceLastTime;
                Vector counterForce2 = -radialUnit * Vector.Dot(netForce, radialUnit); 

                // Adjust for position offset
                double overshoot = distance - stringLength;
                double dv = overshoot / timeIncrement;
                double da = dv / timeIncrement;
                double force3 = da * Particle.Mass;
                Vector counterForce3 = radialUnit * (-force3);

                response = counterForce1 + counterForce2 + counterForce3;
            }

            forceLastTime = response;
            formerTime = engine.Time;
            return response;
        }
    }
}