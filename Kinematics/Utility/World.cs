using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class World
    {

        public World(double time, double c, double g_accel, List<Projectile> projectiles)
        {
            Time = time;
            C = c;
            G_Accel = g_accel;

            Projectiles = projectiles;
        }

        public double Time { get; set; }

        public double C { get; set; }

        public double G_Accel { get; set; }

        public List<Projectile> Projectiles { get; set; }

        public void ApplyForcesToProjectiles()
        {
            foreach (Projectile proj in Projectiles)
            {
                var f_gravity = new Vector(0, 0, G_Accel);
                var f_air = -C * proj.Velocity.Magnitude * proj.Velocity.Magnitude * Vector.Normalize(proj.Velocity);
                var f_net = f_gravity + f_air;

                proj.ApplyForce(f_net);
            }
            
        }

        public void Tick(double dt)
        {
            Time += dt;
            ApplyForcesToProjectiles();
            foreach (Projectile proj in Projectiles)
            {
                proj.Move(dt);
            }
        }
    }
}
