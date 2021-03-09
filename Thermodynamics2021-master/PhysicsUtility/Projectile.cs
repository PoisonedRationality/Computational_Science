using System;

using DongUtility;

namespace PhysicsUtility
{
    /// <summary>
    /// A basic projectile which can have forces applied to it
    /// </summary>
    public class Projectile
    {
        public Vector Position { get; set; }
        public Vector Velocity { get; set; }
        public Vector Acceleration { get; private set; } = Vector.NullVector();
        public Vector NetForce { get; private set; } = Vector.NullVector();

        private double mass;
        public double Mass
        {
            get
            {
                return mass;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("mass", "Mass cannot be zero or negative!");
                }
                else
                {
                    mass = value;
                }
            }
        }

        public Projectile(Vector position, Vector velocity, double mass)
        {
            Position = position;
            Velocity = velocity;
            this.mass = mass;
        }

        override public string ToString()
        {
            return Position + "\t" + Velocity + "\t" + Acceleration;
        }

        private void UpdateAcceleration()
        {
            Acceleration = NetForce / mass;
        }

        private void UpdateVelocity(double timeIncrement)
        {
            Velocity += Acceleration * timeIncrement;
        }

        private void UpdatePosition(double timeIncrement)
        {
            Position += Velocity * timeIncrement + .5 * Acceleration * UtilityFunctions.Square(timeIncrement);
        }

        static private object forceLocker = new object();

        /// <summary>
        /// Adds a force to the particle.
        /// This lasts only until the next time Update() is called
        /// </summary>
        public void AddForce(Vector force)
        {
            // Add locking to allow concurrency
            lock (forceLocker)
            {
                NetForce += force;
            }
        }

        /// <summary>
        /// Updates the position, velocity, and acceleration of the particle
        /// </summary>
        public void Update(double timeIncrement)
        {
            UpdateAcceleration();
            NetForce = Vector.NullVector(); // Reset forces
            UpdatePosition(timeIncrement);
            UpdateVelocity(timeIncrement);
        }
    }
}
