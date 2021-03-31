using DongUtility;
using static DongUtility.UtilityFunctions;
using System;

namespace DongUtility
{
    abstract public class Path
    {
        /// <summary>
        /// The function that determines the path itself.
        /// </summary>
        abstract protected Vector Function(double parameter);
        
        /// <summary>
        /// The starting and ending times.  It is the programmers responsibility to make sure 
        /// these are at the right positions!
        /// </summary>
        abstract public double InitialParameter { get; }
        abstract public double FinalParameter { get; }
        
        /// <summary>
        /// The tolerance, in meters, considered acceptable for a linear approximation of the curve
        /// </summary>
        public double Tolerance { get; set; } = .001;

        /// <summary>
        /// The minimum change in the parameter to be considered.
        /// There should be no substantial change in the function anywhere close to this scale.
        /// </summary>
        public double MinimumStep { get; set; } = .0001;

        /// <summary>
        /// Returns the position at a given time.
        /// </summary>
        public Vector GetPosition(double parameter) => Function(parameter);

        /// <summary>
        /// Finds the next parameter that will approximate the function linearly to within the given precision
        /// </summary>
        /// <param name="finalParameter">The last point allowed, to avoid weird overflows</param>
        public double GetNextParameter(double currentParameter, double finalParameter)
        {
            double parameterStep = MinimumStep;
            const double changeFactor = 2;

            while (GoodGuess(currentParameter, currentParameter + parameterStep))
            {
                parameterStep *= changeFactor;
                if (currentParameter + parameterStep > finalParameter)
                    return finalParameter;
            }
            parameterStep /= changeFactor;

            return currentParameter + parameterStep;
        }

        /// <summary>
        /// Returns the change in position (as a vector) for a small change in the parameter
        /// equal to MinimumStep, evaluated at parameter
        /// </summary>
        public Vector Dx(double parameter)
        {
            Vector currentPosition = GetPosition(parameter);
            Vector nextPosition = GetPosition(parameter + MinimumStep);
            return nextPosition - currentPosition;
        }

        /// <summary>
        /// Returns the derivative (as a vector) of position with respect to parameter
        /// Evaluated at parameter.
        /// </summary>
        public Vector Derivative(double parameter)
        {
            return Dx(parameter) / MinimumStep;
        }

        /// <summary>
        /// Returns whether predictionTime can be used as a good linear appoximation. 
        /// It checks whether a change of twice as much time still matches the function to within the stated tolerance
        /// </summary>
        private bool GoodGuess(double currentParameter, double predictionParameter)
        {
            // Linearly extrapolate the position at currentTime + 2*dT
            double deltaT = predictionParameter - currentParameter;
            double extrapolatedParameter = predictionParameter + deltaT;
            Vector currentPosition = GetPosition(currentParameter);
            Vector predictedPosition = GetPosition(predictionParameter);
            Vector deltaX = predictedPosition - currentPosition;
            Vector dTdX = deltaX / deltaT;
            Vector extrapolatedPosition = predictedPosition + dTdX * deltaT;

            // Calculate the true value
            Vector trueFinalPosition = GetPosition(extrapolatedParameter);

            // Compare them
            double distance2 = Vector.Distance2(extrapolatedPosition, trueFinalPosition);
            return distance2 < Square(Tolerance);
        }
    }
}
