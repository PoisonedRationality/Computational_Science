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
            const double maxSpeed = 10;
            const double mass = 1;
            Color color = Colors.Lavender;
            const int nParticles = 1000;
            const double deltaTime = .01;
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

            const int histogramBins = 50;
            
            //viz.Manager.AddSingleGraph("Temperature", ConvertColor(Colors.CornflowerBlue), () => visualization.Time, () => cont.Temperature, "Time (s)", "Temperature (K)");

            viz.Manager.AddHist(histogramBins, ConvertColor(Colors.BlueViolet), () => cont.GetParticlePropertyList((Particle part) => part.Velocity.Magnitude), "Speed (m/s)");
            //viz.Manager.AddText("Temperature (K)", Colors.CadetBlue, () => cont.Temperature );

            viz.Show();
        }
    }
}
