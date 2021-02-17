using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class World
    {

        public World(double time, double c, double g_accel, double ground, List<Projectile> list)
        {
            Time = time;
            
            C = c;

            G_Accel = g_accel;

            Ground = ground;

            Projectiles = list;
        }


        public double Time { get; set; }

        public double C { get; set; }

        public double G_Accel { get; set; }

        public double Ground { get; set; }

        public List<Projectile> Projectiles { get; set; }

        public void ApplyForcesToProjectiles()
        {
            foreach (Projectile proj in Projectiles)
            {
                var f_gravity = new Vector(0, 0, G_Accel)*proj.Mass;
                var f_air = -C * proj.Velocity.Magnitude * proj.Velocity.Magnitude * Vector.Normalize(proj.Velocity);
                var f_spring = Vector.Zero;

                foreach (Spring spring in proj.Springs)
                {
                    f_spring += -spring.K * (proj.Position.Magnitude - spring.Length) * Vector.Normalize(proj.Position);
                }

                var f_net = f_gravity + f_air + f_spring;

                proj.ApplyForce(f_net);
            }
            
        }

        public bool Tick(double dt)
        {
            bool running = true;
            Time += dt;
            ApplyForcesToProjectiles();
            foreach (Projectile proj in Projectiles)
            {
                proj.Move(dt);
                if (proj.Position.Z < Ground)
                {
                    running = false;
                }
            }

            return running;
            
        }
    }
}
