using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thermodynamics;
using DongUtility;

namespace Thermodynamics
{
    /// <summary>
    /// A particle container that implements a grid strategy to deal with collisions
    /// </summary>
    public class ParticleContainerGrid : ParticleContainer
    { 

        /// <summary>
        /// The distance between two cells, given as a vector
        /// The x component is the distance in x between two cells, and so forth
        /// </summary>
        private Vector intervals;

        /// <summary>
        /// The maximum number of cells in each direction
        /// The x component is the number of x divisions, and so forth
        /// </summary>
        internal Coordinate3D Limits;

        /// <summary>
        /// All the partices and the grid coordinate that they map to
        /// </summary>
        protected Dictionary<Particle, Coordinate3D> ParticleMap { get; set; } = new Dictionary<Particle, Coordinate3D>();

        /// <summary>
        /// All coordinates and the list of particles in each grid division
        /// </summary>
        internal GridCell[,,] Grid { get; set; }

        public ParticleContainerGrid(double xsize, double ysize, double zsize, int xdiv, int ydiv, int zdiv) :
            base(xsize, ysize, zsize)
        {
            Limits = new Coordinate3D(xdiv, ydiv, zdiv);
            intervals = new Vector(xsize / xdiv, ysize / ydiv, zsize / zdiv);
            Grid = new GridCell[Limits.X, Limits.Y, Limits.Z];

            for (int ix = 0; ix < xdiv; ++ix)
                for (int iy = 0; iy < ydiv; ++iy)
                    for (int iz = 0; iz < zdiv; ++iz)
                    {
                        Grid[ix, iy, iz] = new GridCell(new Coordinate3D(ix, iy, iz));
                    }
        }

        /// <summary>
        /// Converts a grid size to the number of grid cells needed
        /// </summary>
        static private int ConvertGridSizeToGridNumber(double side, double gridSize)
        {
            return (int)(Math.Ceiling(side / gridSize));
        }

        public ParticleContainerGrid(double side, double gridSize) :
            this(side, side, side,
                ConvertGridSizeToGridNumber(side, gridSize),
                ConvertGridSizeToGridNumber(side, gridSize),
                ConvertGridSizeToGridNumber(side, gridSize))
        { }

        /// <summary>
        /// Make sure the position is valid
        /// </summary>
        protected bool CheckCoord(Coordinate3D coord)
        {
            return (coord.X >= 0 && coord.Y >= 0 && coord.Z >= 0 && coord.X < Limits.X && coord.Y < Limits.Y && coord.Z < Limits.Z);
        }

        internal GridCell GetCell(Coordinate3D coord)
        {
            return Grid[coord.X, coord.Y, coord.Z];
        }

        private GridCell GetCell(Particle particle)
        {
            return GetCell(TransformToCoord(particle.Position));
        }

        protected override void AddParticleDirectly(Particle part)
        {
            base.AddParticleDirectly(part);
            Coordinate3D coord = TransformToCoord(part.Position);
            ParticleMap.Add(part, coord);
            GetCell(coord).AddParticle(part);
        }

        protected override void RemoveParticleDirectly(Particle part)
        {
            base.RemoveParticleDirectly(part);

            if (ParticleMap.ContainsKey(part))
            {
                Coordinate3D coord = ParticleMap[part];
                GetCell(coord).RemoveParticle(part);
                ParticleMap.Remove(part);
            }
        }

        /// <summary>
        /// Convert from a position to a coordinate in a single dimension
        /// </summary>
        /// <param name="loc">The current position</param>
        /// <param name="interval">The distance between two cells</param>
        /// <param name="max">The maximum number of cells</param>
        static private int TransformToInt(double loc, double interval, int max)
        {
            int returnVal = ConvertGridSizeToGridNumber(loc, interval);
            if (returnVal < 0)
            {
                return 0;
            }
            else if (returnVal >= max)
            {
                return max - 1;
            }
            else
            {
                return returnVal;
            }
        }

        /// <summary>
        /// Transforms from a position to a 3D integer coordinate
        /// </summary>
        private Coordinate3D TransformToCoord(Vector vec)
        {
            return new Coordinate3D(TransformToInt(vec.X, intervals.X, Limits.X),
                TransformToInt(vec.Y, intervals.Y, Limits.Y),
                TransformToInt(vec.Z, intervals.Z, Limits.Z));
        }

        /// <summary>
        /// Advance all particles
        /// </summary>
        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            foreach (var part in ParticleMap.Keys.ToList()) // The ToList() makes it modifiable
            {
                Coordinate3D oldCoord = ParticleMap[part];
                Coordinate3D newCoord = TransformToCoord(part.Position);
                if (oldCoord != newCoord)
                {
                    ParticleMap[part] = newCoord;
                    GetCell(oldCoord).RemoveParticle(part);
                    GetCell(newCoord).AddParticle(part);
                }
            }
        }

        /// <summary>
        /// Find all particles near a given particle
        /// </summary>
        /// <param name="center">The position of the current particle</param>
        /// <param name="rad">The radius to look within</param>
        /// <param name="toBeRemoved">A list of particles that have already been removed from simulation</param>
        /// <returns>All particles within the radius rad from the given particle</returns>
        public IEnumerable<Particle> GetNearbyParticles(Particle center, double rad)
        {
            var cell = GetCell(center);
            return cell.GetParticlesNearby(center, rad);
        }
    }
}
