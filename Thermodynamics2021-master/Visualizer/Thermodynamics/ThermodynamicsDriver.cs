using DongUtility;
using MotionVisualizer3D;
using System;
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
            const double minSpeed = 0;
            const double maxSpeed = 4200;
            const double mass = 1.7e-25;
            Color color = Colors.Lavender;
            const int nParticles = 1000;
            const double deltaTime = 0.1;
            const string name = "Molecule";

            double temperature = 293.15;

            
            var cont = new ParticleContainer(containerSize);
            var info = new ParticleInfo(name, mass, ConvertColor(color));
            var generator = new BoltzmannGenerator(cont, minSpeed, maxSpeed);
            

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



            viz.Manager.AddSingleGraph("Temperature", ConvertColor(Colors.CornflowerBlue), () => visualization.Time, () => cont.GetTemperature(), "Time (s)", "Temperature (K)");

            viz.Manager.AddHist(histogramBins, ConvertColor(Colors.BlueViolet), () => cont.GetParticlePropertyList((Particle part) => part.Velocity.Magnitude), "Speed (m/s)");

            //viz.Manager.AddText("Temperature (K)", ConvertColor(Colors.CadetBlue), () => GetTemperature(cont)+"");

            viz.Manager.AddSingleGraph("Pressure", ConvertColor(Colors.Red), () => visualization.Time, () => cont.Pressure, "Time (s)", "Pressure (Pa)");

            viz.Manager.AddSingleGraph("# Particles", ConvertColor(Colors.Green), () => visualization.Time, () => cont.Particles.Count, "Time (s)", "# Particles");
            viz.Manager.AddSingleGraph("Volume", ConvertColor(Colors.Orange), () => visualization.Time, () => Math.Pow(cont.Size.X,3), "Time (s)", "Volume (m^3)");



            viz.Show();
        }

         
    }
}
