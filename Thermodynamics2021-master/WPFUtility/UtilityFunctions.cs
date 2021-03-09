using DongUtility;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Color = System.Windows.Media.Color;

namespace WPFUtility
{
    static public class UtilityFunctions
    {
        /// <summary>
        /// Gets the x and y dpi of the current device as a pair.
        /// </summary>
        static public Tuple<double, double> GetDPI()
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

            var dpiX = (int)dpiXProperty.GetValue(null, null);
            var dpiY = (int)dpiYProperty.GetValue(null, null);
            return new Tuple<double, double>(dpiX, dpiY);
        }

        /// <summary>
        /// Converts from a .NET Standard-compatible System.Drawing.Color to
        /// a WPF-compatible System.Windows.Media.Color
        /// </summary>
        static public System.Windows.Media.Color ConvertColor(System.Drawing.Color color)
        {
            return new System.Windows.Media.Color()
            {
                R = color.R,
                G = color.G,
                B = color.B,
                A = color.A
            };
        }

        static public System.Drawing.Color ConvertColor(System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        static public Vector3D ConvertVector(DongUtility.Vector vec)
        {
            return new Vector3D(vec.X, vec.Y, vec.Z);
        }

        static public BitmapSource MakeScreenshot(int width, int height, Visual visual)
        {
            var upperLeft = visual.PointToScreen(new System.Windows.Point(0, 0));
            double screenLeft = upperLeft.X;
            double screenTop = upperLeft.Y;
            double screenWidth = width;
            double screenHeight = height;

            using (Bitmap bmp = new Bitmap((int)screenWidth,
                (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                }
                return Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }

        static public void SaveScreenshot(int width, int height, Visual visual)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Screenshot",
                DefaultExt = ".jpg"
            };

            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;

                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(MakeScreenshot(width, height, visual)));
                using (Stream fileStream = File.Create(filename))
                {
                    encoder.Save(fileStream);
                }
            }
        }

        static public Vector3D SphericalCoordinates(double radius, double azimuthal, double polar)
        {
            double x = radius * Math.Cos(azimuthal) * Math.Sin(polar);
            double y = radius * Math.Sin(azimuthal) * Math.Sin(polar);
            double z = radius * Math.Cos(polar);

            return new Vector3D(x, y, z);
        }

        static public Point3D ConvertToPoint3D(Vector3D vector)
        {
            return new Point3D(vector.X, vector.Y, vector.Z);
        }

        static public Vector3D ConvertToVector3D(Point3D point)
        {
            return new Vector3D(point.X, point.Y, point.Z);
        }

        static public Vector3D ConvertToVector3D(Geometry.Geometry3D.Point point)
        {
            return new Vector3D(point.X, point.Y, point.Z);
        }

        static public Vector3D Midpoint(Vector3D one, Vector3D two)
        {
            return (one + two) / 2;
        }

        static public MatrixTransform3D ConvertToRotation3D(DongUtility.Rotation rotation)
        {
            var matrix = ConvertToMatrix3D(rotation.Matrix);

            return new MatrixTransform3D(matrix);
        }

        static public Matrix3D ConvertToMatrix3D(DongUtility.Matrix matrix)
        {
            return new Matrix3D(matrix[0, 0], matrix[0, 1], matrix[0, 2], 0,
                matrix[1, 0], matrix[1, 1], matrix[1, 2], 0,
                matrix[2, 0], matrix[2, 1], matrix[2, 2], 0,
                0, 0, 0, 1);
        }

        static public Color InvertColor(Color color)
        {
            return Color.FromArgb(color.A, (byte)(Constants.MaxByte - color.R),
                (byte)(Constants.MaxByte - color.G), (byte)(Constants.MaxByte - color.B));
        }
    }
}
