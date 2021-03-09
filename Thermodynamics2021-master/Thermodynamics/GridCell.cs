using DongUtility;
using System;
using System.Collections.Generic;
using static DongUtility.UtilityFunctions;

namespace Thermodynamics
{
    internal class GridCell
    {
        private Coordinate3D location;

        private readonly LinkedList<Particle> particles = new LinkedList<Particle>();

        private readonly List<GridCell> cellsToSearch = new List<GridCell>();

        public GridCell(Coordinate3D location)
        {
            this.location = location;
            // Make sure to add itself to the list
            cellsToSearch.Add(this);
        }

        public void AddAdjacentCells(ParticleContainerGrid grid)
        {
            var (minX, maxX) = LowAndHighIndex(location.X, grid.Limits.X);
            var (minY, maxY) = LowAndHighIndex(location.Y, grid.Limits.Y);
            var (minZ, maxZ) = LowAndHighIndex(location.Z, grid.Limits.Z);

            for (int ix = minX; ix <= maxX; ++ix)
                for (int iy = minY; iy <= maxY; ++iy)
                    for (int iz = minZ; iz <= maxZ; ++iz)
                    {
                        cellsToSearch.Add(grid.GetCell(new Coordinate3D(ix, iy, iz)));
                    }
        }

        private Tuple<int, int> LowAndHighIndex(int center, int max)
        {
            int low = center <= 0 ? 0 : center - 1;
            int high = center >= max - 1 ? max - 1 : center + 1;
            return new Tuple<int, int>(low, high);
        }

        public void AddParticle(Particle particle)
        {
            particles.AddLast(particle);
        }

        public void RemoveParticle(Particle particle)
        {
            particles.Remove(particle);
        }

        public IEnumerable<Particle> GetParticlesNearby(Particle center, double radius)
        {
            foreach (var cell in cellsToSearch)
                foreach (var particle in particles)
                {
                    if (particle == center)
                        continue;
                    if (WithinRadius(center, particle, radius))
                        yield return particle;
                }
        }

        private bool WithinRadius(Particle center, Particle candidate, double radius)
        {
            double rad2 = Square(radius);
            if (NearbyInOneDirection(center.Position.X, candidate.Position.X, rad2)
                && NearbyInOneDirection(center.Position.Y, candidate.Position.Y, rad2)
                && NearbyInOneDirection(center.Position.Z, candidate.Position.Z, rad2))
            {
                Vector difference = center.Position - candidate.Position;
                return difference.MagnitudeSquared <= rad2;
            }
            else
                return false;

        }

        private bool NearbyInOneDirection(double p1, double p2, double radiusSquared)
        {
            double diff = p1 - p2;
            double delta2 = Square(diff);
            return delta2 < radiusSquared;
        }
    }
}
