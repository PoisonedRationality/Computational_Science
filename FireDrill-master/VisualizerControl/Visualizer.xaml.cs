using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisualizerControl
{
    /// <summary>
    /// Interaction logic for ArenaVisualizerStandalone.xaml
    /// </summary>
    public partial class Visualizer : UserControl
    {
        public Visualizer()
        {
            InitializeComponent();
        }

        private void Redraw(object sender, ElapsedEventArgs e)
        {
            //if (Display != null && ShowVisual)
            //Display.Redraw();
        }

        private Application app;
        private IntPtr hwndListBox;
        private Window myWindow;
        internal Visualizer3DCoreInterface CoreInterface { get; set; } = null;

        private bool UIinitialized = false;

        private void OnUIReady(object sender, EventArgs e)
        {
            if (!UIinitialized)
            {

                app = Application.Current;
                myWindow = app.MainWindow;
                //myWindow.SizeToContent = SizeToContent.WidthAndHeight;
                CoreInterface = new Visualizer3DCoreInterface(Visualizer3DCoreInterfaceHolder.ActualWidth,
                    Visualizer3DCoreInterfaceHolder.ActualHeight);
                Visualizer3DCoreInterfaceHolder.Child = CoreInterface;
                hwndListBox = CoreInterface.HwndListBox;

                UIinitialized = true;
            }
        }

        public void WhenLoaded(object sender, EventArgs e)
        {
            var window = Window.GetWindow(this);
            if (!initialized)
            {
                // Call it as a task, or else everything hangs here
                Task.Run(() => Visualizer3DCoreInterface.SetupDirectX());

                initialized = true;
            }
            //InvalidateVisual();
        }
        private bool initialized = false;

        /// <summary>
        /// Adds a particle with a user-defined index for later manipulation
        /// </summary>
        public void AddParticle(Object3D part, int index)
        {
            CoreInterface.AddObject(part, index);
        }

        /// <summary>
        /// Removes a particle with a given index
        /// </summary>
        public void RemoveParticle(int index)
        {
            CoreInterface.RemoveObject(index);
        }

        public void MoveParticle(int index, Vector3D newPosition)
        {
            CoreInterface.MoveObject(index, newPosition);
        }

        public void TransformParticle(int index, Vector3D newPosition,
            Vector3D newScale, Matrix3D newRotation)
        {
            CoreInterface.TransformObject(index, newScale, newRotation, newPosition);
        }

        /// <summary>
        /// Clears all objects from the visualizer
        /// </summary>
        public void Clear()
        {
            CoreInterface.RemoveAllObjects();
        }

        /// <summary>
        /// Moves and rotates the camera
        /// </summary>
        public void MoveCamera(Point3D newPosition, Vector3D newDirection, Vector3D upDirection)
        {
            CoreInterface.MoveCamera(newPosition, newDirection, upDirection);
        }

        /// <summary>
        /// Moves camera without rotation
        /// </summary>
        /// <param name="newPosition"></param>
        public void MoveCamera(Point3D newPosition)
        {
            CoreInterface.MoveCamera(newPosition);
        }

        /// <summary>
        /// Turns the camera to look at a given point
        /// </summary>
        public void LookAt(Point3D newPosition, Point3D target, Vector3D upDirection)
        {
            CoreInterface.LookAt(newPosition, target, upDirection);
        }

        public void AdjustLens(double pointOfView, double aspectRatio, double nearZ, double farZ)
        {
            CoreInterface.AdjustLens(pointOfView, aspectRatio, nearZ, farZ);
        }


        /// <summary>
        /// Activates auto-camera mode
        /// </summary>
        public bool AutoCamera
        {
            set
            {
                CoreInterface.SetAutoCamera(value);
            }
        }
    }
}
