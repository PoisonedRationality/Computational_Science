using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    class Projectile
    {

        public Projectile(Vector pos, Vector velo, Vector accel, double mass)
        {
            Position = pos;

            Velocity = velo;

            Acceleration = accel;

            Mass = mass;
        }

        public Projectile(double mass)
        {
            Position = Velocity = Acceleration = new Vector(0, 0, 0);
            Mass = mass;
        }

        public Vector Position { get; set; }

        public Vector Velocity { get; set; }

        public Vector Acceleration { get; set; }

        public double Mass { get; }

        public void Move(double dt) 
        {
            UpdateVelocity(dt);
            UpdatePosition(dt);
        }

        public void ApplyForce(Vector force) 
        {
            Acceleration = force / Mass;
        }

        private void UpdatePosition(double dt)
        {
            Position += Velocity * dt;
        }

        private void UpdateVelocity(double dt)
        {
            Velocity += Acceleration * dt;
        }
    }
}
