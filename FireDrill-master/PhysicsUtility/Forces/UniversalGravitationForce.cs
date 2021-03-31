using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DongUtility;

namespace PhysicsUtility
{
    /// <summary>
    /// A force simulating non-constant Newtonian gravity (for planets and similar projectiles)
    /// </summary>
    public class UniversalGravitationForce : GlobalForce
    {
        private const double gravConstant = 6.67384e-11;

        public UniversalGravitationForce(KinematicsEngine engine) : base(engine)
        { }

        protected override Vector GetForce(Projectile proj)
        {
            Vector netForce = Vector.NullVector();
            foreach (var point in Projectiles)
            {
                if (point.Position != proj.Position)
                {
                    Vector difference = point.Position - proj.Position;
                    double distance2 = difference.MagnitudeSquared;
                    Vector force = (gravConstant * proj.Mass * point.Mass / distance2) * difference.UnitVector();
                    netForce += force;
                }
            }

            return netForce;
        }
    }
}
