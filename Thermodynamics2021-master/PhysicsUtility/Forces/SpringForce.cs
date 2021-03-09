using DongUtility;

namespace PhysicsUtility
{
    /// <summary>
    /// A force from a spring
    /// The position of the other end of the spring is abstract
    /// </summary>
    abstract public class SpringForce : SingleProjectileForce
    {
        private readonly double springConstant;
        private readonly double unstretchedLength;
        protected abstract Vector SpringPosition();

        public SpringForce(Projectile projectile, double springConstant, double unstretchedLength = 0) :
            base(projectile)
        {
            this.springConstant = springConstant;
            this.unstretchedLength = unstretchedLength;
        }

        protected override Vector GetForce()
        {
            Vector difference = Particle.Position - SpringPosition();
            double magnitude = springConstant * (unstretchedLength - difference.Magnitude);
            return magnitude * difference.UnitVector();
        }
    }
}