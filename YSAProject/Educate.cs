namespace YSAProject
{
    internal class Education
    {
        private const double LearningCoaffiction = 0.01;
        private const double MomentumCoaffiction = 0.02;
        private double[,] _firstLayerWeightsChange;
        private double[,] _secondLayerWeightsChange;

        public (double[,], double[,]) EducateFirstLayer(double[,] weights, double error, double[] input,
            double[,] lastChangeofWeight, double[] secondLayerWeights)
        {
            _firstLayerWeightsChange = new double[lastChangeofWeight.GetLength(0), lastChangeofWeight.GetLength(1)];
            var buffer = error;

            for (var i = 0; i < _firstLayerWeightsChange.GetLength(0); i++)
            {
                error *= secondLayerWeights[i];
                for (var j = 0; j < _firstLayerWeightsChange.GetLength(1); j++)
                    _firstLayerWeightsChange[i, j] = LearningCoaffiction * error * input[j] +
                                                     MomentumCoaffiction * lastChangeofWeight[i, j];
                error = buffer;
            }

            for (var i = 0; i < _firstLayerWeightsChange.GetLength(0); i++)
            for (var j = 0; j < _firstLayerWeightsChange.GetLength(1); j++)
                weights[i, j] += _firstLayerWeightsChange[i, j];
            return (weights, _firstLayerWeightsChange);
        }

        public (double[,], double[,]) EducateSecondLayer(double[,] weights, double error, double[] hiddenLayerOutput,
            double[,] lastChangeofWeight)
        {
            _secondLayerWeightsChange = new double[weights.GetLength(0), weights.GetLength(1)];

            for (var i = 0; i < _secondLayerWeightsChange.GetLength(0); i++)
            for (var j = 0; j < _secondLayerWeightsChange.GetLength(1); j++)
                _secondLayerWeightsChange[i, j] = LearningCoaffiction * error * hiddenLayerOutput[j] +
                                                  MomentumCoaffiction * lastChangeofWeight[i, j];
            for (var i = 0; i < _secondLayerWeightsChange.GetLength(0); i++)
            for (var j = 0; j < _secondLayerWeightsChange.GetLength(1); j++)
                weights[i, j] += lastChangeofWeight[i, j];
            return (weights, _secondLayerWeightsChange);
        }
    }
}