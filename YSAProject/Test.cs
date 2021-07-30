using System;
using System.Linq;

namespace YSAProject
{
    internal class Test
    {
        private double[] _hiddenOutput;
        private double _mape;
        private double _neuronOutput, _lastOutput, _error;

        public void test(double[,] inputData, double[,] outputData, double[,] hiddenLayerWeights,
            double[,] outputLayerWeights, double[] bias, int hiddenNeuronSize, int outputNeuronSize)
        {
            _hiddenOutput = new double[hiddenNeuronSize];

            for (var a = 0; a < inputData.GetLength(0); a++)
            {
                //giris verilerinin hidden layera gönderilmesi
                for (var i = 0; i < hiddenNeuronSize; i++)
                {
                    //hidden layera gelen verilerin hesaplanması
                    _neuronOutput = Neuron.Calculate(GetRow(hiddenLayerWeights, i), GetRow(inputData, a), bias[i]);
                    _hiddenOutput[i] = _neuronOutput;
                }

                //hidden layerdan gelen verilerin output layerına gönderilmesi
                for (var i = 0; i < outputNeuronSize; i++)
                    //output layera gelen verilerin hesaplanması
                    _lastOutput = Neuron.Calculate(GetRow(outputLayerWeights, i), _hiddenOutput,
                        bias[hiddenNeuronSize + i]);
                _error = outputData[a, 0] - _lastOutput;

                //Mape degerinin ilk basamagi (sonuc dizisinde 0 elemani olmasina karsin alinan onlem)
                if (outputData[a, 0] != 0) _mape += Math.Abs(_error) / outputData[a, 0];
            }

            //Mape değerinin son değerinin hesaplanması
            _mape = _mape / inputData.GetLength(0) * 100;
            Console.WriteLine("Test MAPE: " + _mape);
        }

        private static double[] GetRow(double[,] matrix, int rowNumber)
        {
            return Enumerable
                .Range(0, matrix
                    .GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
        }
    }
}