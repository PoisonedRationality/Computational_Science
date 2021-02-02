using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Utility;

namespace VectorTests.cs
{
    [TestClass]
    public class VectorTest
    {
        [TestMethod]
        public void MagnitudeTest()
        {
            Vector vec = new Vector(1, 4, -7);
            double magnitude = vec.Magnitude;
            double trueAnswer = Math.Sqrt(1 + 16 + 49);
            const double tolerance = 1e-10;

            double difference = magnitude - trueAnswer;


            Assert.IsTrue(Math.Abs(difference) < tolerance);

            Vector vec2 = new Vector(0, 0, 0);
            magnitude = vec2.Magnitude;
            trueAnswer = Math.Sqrt(0);
            Assert.IsTrue(Math.Abs(difference) < tolerance);

        }

        [TestMethod]
        public void AddTest()
        {
            Vector vec = new Vector(-1, 5.3, 3.25634);
            Vector vec2 = new Vector(0, 32, -0.8);

            Vector sum = vec + vec2;

            Vector trueAnswer = new Vector(-1, 5.3 + 32, 3.25634 - 0.8);

            const double tolerance = 1e-10;

            Vector difference = sum - trueAnswer;

            Assert.IsTrue(Math.Abs(difference.X) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Y) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Z) < tolerance);
        }

        [TestMethod]
        public void SubtractionTest()
        {
            Vector vec = new Vector(-1, 5.3, 3.25634);
            Vector vec2 = new Vector(0, 32, -0.8);

            Vector diff = vec - vec2;

            Vector trueAnswer = new Vector(-1, 5.3 - 32, 3.25634 + 0.8);

            const double tolerance = 1e-10;

            Vector difference = diff - trueAnswer;

            Assert.IsTrue(Math.Abs(difference.X) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Y) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Z) < tolerance);

        }

        [TestMethod]
        public void MultiplyTest()
        {
            Vector vec = new Vector(1, 0, -2.34);
            double scalar = 3;
            Vector quotient = vec * scalar;
            Vector trueAnswer = new Vector(1 * 3, 0 * 3, -2.34 * 3);

            const double tolerance = 1e-10;

            Vector difference = quotient - trueAnswer;

            Assert.IsTrue(Math.Abs(difference.X) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Y) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Z) < tolerance);

            scalar = 0.03;
            quotient = vec * scalar;
            trueAnswer = new Vector(1 * 0.03, 0 * 0.03, -2.34 * 0.03);

            difference = quotient - trueAnswer;

            Assert.IsTrue(Math.Abs(difference.X) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Y) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Z) < tolerance);

            scalar = 0;

            quotient = vec * scalar;
            trueAnswer = new Vector(0, 0, 0);

            difference = quotient - trueAnswer;

            Assert.IsTrue(Math.Abs(difference.X) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Y) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Z) < tolerance);


        }

        [TestMethod]
        public void DivisionTest()
        {
            Vector vec = new Vector(1, 5, -2.34);
            double scalar = 3;
            Vector quotient = vec / scalar;
            Vector trueAnswer = new Vector(1.0 / 3, 5.0 / 3, -2.34 / 3);

            const double tolerance = 1e-10;

            Vector difference = quotient - trueAnswer;

            Assert.IsTrue(Math.Abs(difference.X) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Y) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Z) < tolerance);
        }

        [TestMethod]
        public void DotTest()
        {
            Vector vec = new Vector(-9.5, 5.5, -5.23);
            Vector vec2 = new Vector(0, 7, -0.8);

            double dot = Vector.Dot(vec,vec2);

            double trueAnswer = 0+(5.5*7)+(-5.23*-0.8);

            const double tolerance = 1e-10;

            double difference = dot - trueAnswer;

            Assert.IsTrue(Math.Abs(difference) < tolerance);

        }

        [TestMethod]
        public void CrossTest()
        {
            Vector vec = new Vector(0.7, 9.34, -2.3);
            Vector vec2 = new Vector(1, 0, 0);

            Vector cross = Vector.Cross(vec, vec2);

            Vector trueAnswer = new Vector(0, -2.3, -9.34);

            const double tolerance = 1e-10;

            Vector difference = cross - trueAnswer;


            Assert.IsTrue(Math.Abs(difference.X) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Y) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Z) < tolerance);

            Vector vec3 = new Vector(8.001, 3, 4);
            Vector vec4 = new Vector(-0.03, 2.3, 1.1);

            Vector cross2 = Vector.Cross(vec3, vec4);

            trueAnswer = new Vector(3 * 1.1 - 2.3 * 4, 4 * -0.03 - 8.001 * 1.1, 8.001 * 2.3 - -0.03 * 3);

            difference = cross2 - trueAnswer;

            Assert.IsTrue(Math.Abs(difference.X) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Y) < tolerance);
            Assert.IsTrue(Math.Abs(difference.Z) < tolerance);

        }

    }
}
