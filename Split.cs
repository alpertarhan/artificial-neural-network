using System;

namespace YSAProject
{
    internal class Split
    {
        private readonly double[,] _inputData;
        private readonly double[,] _outputData;
        private readonly int _trainPercent;
        private double[,] _inputTrainData, _inputTestData, _outputTrainData, _outputTestData;

        public Split(double[,] inputData, double[,] outputData, int trainPercent)
        {
            _inputData = inputData;
            _outputData = outputData;
            _trainPercent = trainPercent;
        }

        public (double[,], double[,], double[,], double[,]) split()
        {
            var inputNumofCol = _inputData.GetLength(1);
            var outputNumofCol = _outputData.GetLength(1);
            var percentBuffer = (_inputData.GetUpperBound(0) + 1) * _trainPercent / 100;
            const int doubleSize = sizeof(double);
            _inputTrainData = new double[percentBuffer, inputNumofCol];
            _outputTrainData = new double[percentBuffer, outputNumofCol];
            _inputTestData = new double[_inputData.GetLength(0) - percentBuffer, inputNumofCol];
            _outputTestData = new double[_outputData.GetLength(0) - percentBuffer, outputNumofCol];

            for (var i = 0; i < percentBuffer; i++)
            {
                Buffer.BlockCopy(_inputData,
                    i * inputNumofCol * doubleSize,
                    _inputTrainData,
                    i * inputNumofCol * doubleSize,
                    inputNumofCol * doubleSize);
                Buffer.BlockCopy(_outputData,
                    i * outputNumofCol * doubleSize,
                    _outputTrainData,
                    i * outputNumofCol * doubleSize,
                    outputNumofCol * doubleSize);
            }

            for (var i = percentBuffer; i < _inputData.GetLength(0); i++)
            {
                Buffer.BlockCopy(_inputData,
                    i * inputNumofCol * doubleSize,
                    _inputTestData,
                    (i - percentBuffer) * inputNumofCol * doubleSize,
                    inputNumofCol * doubleSize);
                Buffer.BlockCopy(_outputData,
                    i * outputNumofCol * doubleSize,
                    _outputTestData,
                    (i - percentBuffer) * outputNumofCol * doubleSize,
                    outputNumofCol * doubleSize);
            }

            return (_inputTrainData, _outputTrainData, _inputTestData, _outputTestData);
        }
    }
}