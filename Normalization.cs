namespace YSAProject
{
    internal class Normalization
    {
        private string[,] _data;

        public Normalization(int row, int col)
        {
            _data = new string[row, col];
        }

        private static double GetMax(double[,] data, int col)
        {
            var max = data[0, col];
            for (var i = 1; i < data.GetUpperBound(0); i++)
                if (data[i, col] > max)
                    max = data[i, col];
            return max;
        }

        private static double GetMin(double[,] data, int col)
        {
            var min = data[0, col];
            for (var i = 1; i < data.GetUpperBound(0); i++)
                if (data[i, col] < min)
                    min = data[i, col];
            return min;
        }

        private static double NormalizeFormule(double val, double valmin, double valmax)
        {
            return (val - valmin) / (valmax - valmin) * (1 - 0) + 0;
        }

        public static double[,] Normalize(double[,] data)
        {
            var matrix = new double[data.GetUpperBound(0) + 1, data.GetUpperBound(1) + 1];
            for (var i = 0; i <= data.GetUpperBound(1); i++)
            {
                double max = GetMax(data, i), min = GetMin(data, i);
                for (var j = 0; j <= data.GetUpperBound(0); j++) matrix[j, i] = NormalizeFormule(data[j, i], min, max);
            }

            return matrix;
        }
    }
}