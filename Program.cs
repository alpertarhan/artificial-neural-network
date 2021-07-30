using System;
using System.Linq;

namespace YSAProject

{
    internal static class Program
    {
        private static readonly Test Test = new();

        private static void Main(string[] args)
        {
            _hiddenWeigths = new double[HiddenNeuronSize, NumOfInput];
            _outputWeights = new double[NumOfOutput, HiddenNeuronSize];
            _lastChangeHiddenWeigths = new double[HiddenNeuronSize, NumOfInput];
            _lastChangeOutputWeights = new double[NumOfOutput, HiddenNeuronSize];
            double[,] inputData, outputData, inputTrainData, inputTestData, outputTrainData, outputTestData;
            var epoch = 0;

            #region DosyaOkuma

            IReadFile read = new ReadExcel(Path);
            var data = read.ReadData();

            #endregion

            #region VerilerinKaristirilipAyrilmasi

            var splitandShuffle = new Shuffle(NumOfInput, NumOfOutput);
            (inputData, outputData) = splitandShuffle.Allocate(data);

            #endregion

            #region Train-TestDagitimi

            var split = new Split(inputData, outputData, TrainPercent);
            (inputTrainData, outputTrainData, inputTestData, outputTestData) = split.split();

            #endregion

            #region Agirlik-BiasAtamasi

            _hiddenWeigths = RandomWeights.GetRandom(HiddenNeuronSize, NumOfInput);
            _outputWeights = RandomWeights.GetRandom(NumOfOutput, HiddenNeuronSize);
            _bias = RandomWeights.GetBias(HiddenNeuronSize + OutputNeuronSize);

            #endregion

            #region EpochDongusu

            var hiddenOutput = new double[HiddenNeuronSize];
            do
            {
                _mse = 0;
                _mape = 0;
                for (var a = 0; a < inputTrainData.GetLength(0); a++)
                {
                    #region HiddenLayerVerileri

                    for (var i = 0; i < HiddenNeuronSize; i++)
                    {
                        //HiddenLayerVerilerininHesaplamasi
                        _neuronOutput = Neuron.Calculate(GetRow(_hiddenWeigths, i), GetRow(inputTrainData, a),
                            _bias[i]);
                        hiddenOutput[i] = _neuronOutput;
                    }

                    #endregion

                    //hidden layerdan gelen verilerin output layerına gönderilmesi
                    for (var i = 0; i < OutputNeuronSize; i++)
                        //output layera gelen verilerin hesaplanması
                        _lastOutput = Neuron.Calculate(GetRow(_outputWeights, i), hiddenOutput,
                            _bias[HiddenNeuronSize + i]);
                    _error = outputTrainData[a, 0] - _lastOutput;
                    (_hiddenWeigths, _lastChangeHiddenWeigths) = Education.EducateFirstLayer(_hiddenWeigths, _error,
                        GetRow(inputTrainData, a),
                        _lastChangeHiddenWeigths,
                        GetRow(_outputWeights,
                            0));
                    (_outputWeights, _lastChangeOutputWeights) = Education.EducateSecondLayer(_outputWeights, _error,
                        hiddenOutput,
                        _lastChangeOutputWeights);

                    if (outputTrainData[a, 0] == 0) continue;
                    _error = Math.Abs(_error);
                    _mse += Math.Pow(_error, 2);
                    _mape += Math.Abs(_error) / outputTrainData[a, 0];
                }

                #region MSE-MAPE Hesaplanmasi

                _mse /= inputTrainData.GetLength(0);
                _mape = _mape / inputTrainData.GetLength(0) * 100;

                Console.WriteLine(epoch + 1 + " MSE: " + _mse);
                Console.WriteLine(epoch + 1 + " MAPE: " + _mape);
                epoch++;
            } while (epoch < 100 && _mape > 3);

            Test.test(inputTestData,
                outputTestData,
                _hiddenWeigths,
                _outputWeights, _bias, HiddenNeuronSize, OutputNeuronSize);

            Console.ReadLine();

            #endregion

            #endregion
        }

        private static double[] GetRow(double[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1)).Select(x => matrix[rowNumber, x]).ToArray();
        }

        public static double[] GetCol(double[,] matrix, int colNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0)).Select(x => matrix[x, colNumber]).ToArray();
        }

        #region Degiskenler

        private const int NumOfInput = 8;
        private const int NumOfOutput = 1;
        private const int TrainPercent = 70;
        private const int HiddenNeuronSize = 10;
        private const int OutputNeuronSize = 1;
        private const string Path = "C:/Users/Administrator/Downloads/AiProject/enerji_verimliliği_veri_seti.xls";
        private static double[,] _hiddenWeigths, _outputWeights;
        private static double[,] _lastChangeHiddenWeigths, _lastChangeOutputWeights;
        private static double[] _bias;
        private static readonly Neuron Neuron = new();
        private static readonly RandomWeights RandomWeights = new();
        private static readonly Education Education = new();
        private static double _neuronOutput, _lastOutput, _error;
        private static double _mse, _mape;

        #endregion
    }
}