using System;
using System.Collections.Generic;

namespace PhysicsUtility
{
    /// <summary>
    /// The class responsible for running the motion of all projectiles
    /// </summary>
    public class KinematicsEngine
    {
        /// <summary>
        /// The current time of the simulation
        /// </summary>
        public double Time { get; set; } = 0;

        private double oldTime = 0;
        public double DeltaTime => Time - oldTime;
        /// <summary>
        /// All the projectiles that are in motion
        /// </summary>
        public List<Projectile> Projectiles { get; } = new List<Projectile>();
        /// <summary>
        /// All the forces that act on the particles
        /// </summary>
        protected List<Force> Forces { get; } = new List<Force>();

        /// <summary>
        /// Add a projectile to the simulation
        /// </summary>
        public void AddProjectile(Projectile projectile)
        {
            if (Projectiles.Contains(projectile))
            {
                throw new InvalidOperationException("Attempted to add Projectile that already exists!");
            }
            else
            {
                Projectiles.Add(projectile);
            }
        }

        /// <summary>
        /// Add a Force object to apply to the simulation
        /// </summary>
        /// <param name="force"></param>
        public void AddForce(Force force)
        {
            Forces.Add(force);
        }

        private List<StopCondition> stopConditions = new List<StopCondition>();

        /// <summary>
        /// Add a StopCondition to the engine
        /// </summary>
        public void AddStopCondition(StopCondition condition)
        {
            stopConditions.Add(condition);
        }

        protected bool CheckStopConditions()
        {
            // Check whether it's time to stop
            foreach (var condition in stopConditions)
            {
                if (!condition.ShouldContinue(this))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Runs one tick of the clock
        /// </summary>
        virtual public bool Increment(double timeIncrement)
        {
            // Increment time
            oldTime = Time;
            Time += timeIncrement;

            // Add all forces
            foreach (Force force in Forces)
            {
                force.AddForce();
            }

            // Update all projectiles
            foreach (Projectile projectile in Projectiles)
            {
                projectile.Update(timeIncrement);
            }

            return CheckStopConditions();
        }



    }
}
