using DongUtility;
using MotionVisualizer3D;
using System.Windows.Media;
using Thermodynamics;
using static WPFUtility.UtilityFunctions;

namespace Visualizer.Thermodynamics
{
    class ThermodynamicsDriver
    {
        static internal void Run()
        {
            const double containerSize = 50;
            const double minSpeed = 1;
            const double maxSpeed = 300;
            const double mass = 1e-25;
            Color color = Colors.Lavender;
            const int nParticles = 1000;
            const double deltaTime = 0.01;
            const string name = "Molecule";

            var cont = new ParticleContainer(containerSize);
            var info = new ParticleInfo(name, mass, ConvertColor(color));
            var generator = new FlatGenerator(cont, minSpeed, maxSpeed);
            

            cont.Dictionary.AddParticle(info);
            cont.AddRandomParticles(generator, name, nParticles);

            var visualization = new ThermodynamicsVisualization(cont)
            {
                BoxColor = Colors.IndianRed
            };

            var viz = new MotionVisualizer3DControl(visualization)
            {
                TimeIncrement = deltaTime,
                TimeScale = 1
            };

            viz.FastDraw = true;

            const int histogramBins = 50;

            double temperature = GetTemperature(cont);


            viz.Manager.AddSingleGraph("Temperature", ConvertColor(Colors.CornflowerBlue), () => visualization.Time, () => temperature, "Time (s)", "Temperature (K)");

            viz.Manager.AddHist(histogramBins, ConvertColor(Colors.BlueViolet), () => cont.GetParticlePropertyList((Particle part) => part.Velocity.Magnitude), "Speed (m/s)");
            viz.Manager.AddText("Temperature (K)", ConvertColor(Colors.CadetBlue), () => GetTemperature(cont)+"");

            viz.Show();
        }

        static double GetTemperature(ParticleContainer cont)
        {
            double temperature;
            double totalKE = 0;
            foreach (Particle particle in cont.Particles)
            {
                totalKE += particle.KineticEnergy;
            }
            //T = (2/3)*KE/k_b
            temperature = (2 * totalKE/cont.Particles.Count) / (3 * Constants.BoltzmannConstant);

            return temperature;

        }

    

         
    }
}
