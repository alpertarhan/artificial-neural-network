using System;

namespace YSAProject
{
    internal class RandomWeights
    {
        private readonly Random _rnd = new();

        public double[,] GetRandom(int numofRow, int numofCol)
        {
            var weights = new double[numofRow, numofCol];

            for (var i = 0; i < numofRow; i++)
            for (var j = 0; j < numofCol; j++)
                weights[i, j] = _rnd.NextDouble();

            return weights;
        }

        public double[] GetBias(int num)
        {
            var bias = new double[num];

            for (var i = 0; i < num; i++) bias[i] = _rnd.NextDouble();

            return bias;
        }
    }
}