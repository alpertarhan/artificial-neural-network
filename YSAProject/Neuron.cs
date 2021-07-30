using System;
using System.Collections.Generic;
using System.Linq;

namespace YSAProject
{
    internal class Neuron
    {
        public static double Calculate(double[] weights, IEnumerable<double> inputs, double bias)
        {
            var fNet = inputs.Select((t, i) => t * weights[i]).Sum();

            return 1 / (1 + Math.Exp(-(fNet + bias)));
        }
    }
}