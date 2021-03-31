using DongUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsUtility
{
    public static class LightFunctions
    {
        private const double gamma = .8;

        /// <summary>
        /// Converts a wavelength to a Color object
        /// Taken from http://www.efg2.com/Lab/ScienceAndEngineering/Spectra.htm
        /// Based on Dan Bruton's work
        /// </summary>
        /// <param name="wavelength">In meters, NOT nanometers</param>
        static public Color ConvertWavelengthToColor(double wavelength)
        {
            double red = 0, green = 0, blue = 0;

            wavelength *= 1e9;

            // Lots of magic numbers here!
            if (wavelength < 380)
            {
                // Default to (0, 0, 0) for invisible light
            }
            else if (wavelength < 440)
            {
                red = -(wavelength - 440) / (440 - 380);
                green = 0;
                blue = 1;
            }
            else if (wavelength < 490)
            {
                red = 0;
                green = (wavelength - 440) / (490 - 440);
                blue = 1;
            }
            else if (wavelength < 510)
            {
                red = 0;
                green = 1;
                blue = -(wavelength - 510) / (510 - 490);
            }
            else if (wavelength < 580)
            {
                red = (wavelength - 510) / (580 - 510);
                green = 1;
                blue = 0;
            }
            else if (wavelength < 645)
            {
                red = 1;
                green = -(wavelength - 645) / (645 - 580);
                blue = 0;
            }
            else if (wavelength <= 780)
            {
                red = 1;
                green = 0;
                blue = 0;
            }

            double factor = 0;
            if (wavelength < 380)
            {
                // do nothing
            }
            else if (wavelength < 420)
            {
                factor = .3 + .7 * (wavelength - 380) / (420 - 380);
            }
            else if (wavelength < 701)
            {
                factor = 1;
            }
            else if (wavelength <= 780)
            {
                factor = .3 + .7 * (780 - wavelength) / (780 - 700);
            }

            return Color.FromArgb(byte.MaxValue, Adjust(red, factor), Adjust(green, factor), Adjust(blue, factor));
        }


        static private byte Adjust(double wavelength, double factor)
        {
            if (wavelength == 0)
                return 0;
            else
                return (byte)Math.Round(Constants.MaxByte * Math.Pow(wavelength * factor, gamma));
        }
    }

}

