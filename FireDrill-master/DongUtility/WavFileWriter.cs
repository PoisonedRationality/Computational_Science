using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DongUtility
{
    /// <summary>
    /// A class which creaes a wav file from a list of numbers
    /// Normalization automatically applied
    /// </summary>
    public class WavFileWriter
    {
        // The number of channels to write to = 1 for mono, 2 for stereo
        public ushort NumChannels { get; set; } = 1;
        // The sample rate, in Hertz, of the file
        public uint SampleRate { get; set; } = 44100;
        // The bit depth of each sample
        public const ushort BitsPerSample = 32;

        // An amplification coefficient, which multiplies all numbers by a set value
        public double Amplification { get; set; } = 1;

        private List<int> samples = new List<int>();

        // Simple constants for normalization
        private const int maxVal = 2147483647;
        private const int minVal = -2147483648;

        /// <summary>
        /// Normalize the samples and add to the file
        /// </summary>
        /// <param name="samples">A list of sample values</param>
        public void AddSamples(IEnumerable<double> samples)
        {
            NormalizeAndAdd(samples);
        }

        /// <summary>
        /// Linearly interpolates, as needed, between samples at different times.
        /// Then adds them to the wav file
        /// </summary>
        /// <param name="original">A set of (time, value) pairs to be interpolated</param>
        public void CreateInterpolatedSamples(IEnumerable<Tuple<double, double>> original)
        {
            // Holding place for samples to add
            var templist = new List<double>();

            double timePerSample = 1.0 / SampleRate;

            // The previous value, for comparison
            var oldval = new Tuple<double, double>(0, 0);

            // Used for averaging
            double runningTotal = 0;
            int nSamples = 0;

            foreach (var pair in original)
            {
                double timeDiff = pair.Item1 - oldval.Item1;

                // Must be time-ordered!
                if (timeDiff <= 0)
                {
                    throw new ArgumentException("Input to CreateInterpolatedSamples() is not time-ordered");
                }

                if (timeDiff > timePerSample)
                {
                    // This means we are done averaging values when the samples are faster than our sample rate
                    if (runningTotal > 0)
                    {
                        templist.Add(runningTotal / nSamples);
                        runningTotal = 0;
                        nSamples = 0;
                    }
                    // This means it is slower, and we need to interpolate
                    else
                    {
                        Interpolate(oldval, pair, ref templist);
                    }
                    // Store the previous point to check time-ordering
                    oldval = pair;
                }
                // If the spacing is less than the sample speed, average the values in the gap
                else if (timeDiff < timePerSample)
                {
                    runningTotal += pair.Item2;
                    ++nSamples;
                }

            }

            NormalizeAndAdd(templist);
        }

        /// <summary>
        /// Linear interpolation method
        /// </summary>
        /// <param name="oldPair">A (time, value) pair for the first point</param>
        /// <param name="newPair">A (time, value) pair for the second point</param>
        /// <param name="list">A list of all the resulting interpolated values, at the fixed sample rate</param>
        private void Interpolate(Tuple<double, double> oldPair, Tuple<double, double> newPair, ref List<double> list)
        {
            double timeDiff = newPair.Item1 - oldPair.Item1;
            int nSteps = (int)Math.Round(timeDiff * SampleRate);

            double eachStep = (newPair.Item2 - oldPair.Item2) / nSteps;

            double current = oldPair.Item2;
            for (int i = 0; i < nSteps; ++i)
            {
                list.Add(current += eachStep);
            }
        }

        /// <summary>
        /// The method that actually does normalization and adding
        /// </summary>
        private void NormalizeAndAdd(IEnumerable<double> samples)
        {
            double max = samples.Max((x)=>Math.Abs(x));

            foreach (var sample in samples)
            {
                int pitch = (int)(sample * Amplification / max * maxVal);

                // To keep from clipping
                if (pitch > maxVal)
                    pitch = maxVal;
                if (pitch < minVal)
                    pitch = minVal;

                this.samples.Add(pitch);
            }
        }

        /// <summary>
        /// Calculates the number of bits in the file
        /// </summary>
        private int TotalFileSize()
        {
            return (samples.Count * BitsPerSample / 8);
        }

        /// <summary>
        /// Actually writes the wav file.
        /// All points must be added prior to writing.
        /// </summary>
        public void WriteFile(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create))
            {
                BinaryWriter wr = new BinaryWriter(file);

                // The header
                wr.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
                wr.Write(36 + TotalFileSize());
                wr.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));
                wr.Write((uint)16);
                wr.Write((ushort)1);
                wr.Write(NumChannels);
                wr.Write(SampleRate);
                ushort totalBytesPerSample = (ushort)(NumChannels * BitsPerSample / 8);
                uint byteRate = totalBytesPerSample * SampleRate;
                wr.Write(byteRate);
                wr.Write(totalBytesPerSample);
                wr.Write(BitsPerSample);

                // Begin data block
                wr.Write(System.Text.Encoding.ASCII.GetBytes("data"));
                wr.Write((uint)TotalFileSize());

                // Write samples
                foreach (var sample in samples)
                {
                    wr.Write(sample);
                }
            }
        }
    }
}
