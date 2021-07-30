namespace YSAProject
{
    internal class Shuffle
    {
        private readonly int _numofInput;
        private readonly int _numofOutput;

        public Shuffle(int numofInput, int numofOutput)
        {
            _numofInput = numofInput;
            _numofOutput = numofOutput;
        }

        public (double[,], double[,]) Allocate(double[,] data)
        {
            var inputData = new double[data.GetUpperBound(0) + 1, _numofInput];
            var outputData = new double[data.GetUpperBound(0) + 1, _numofOutput];
            var bufferCol = 0;
            for (var i = 0; i <= data.GetUpperBound(0); i++)
            for (var j = 0; j < _numofInput; j++)
                inputData[i, j] = data[i, j];
            for (var i = 0; i <= data.GetUpperBound(0); i++)
            {
                for (var j = _numofInput; j <= data.GetUpperBound(1); j++)
                {
                    outputData[i, bufferCol] = data[i, j];
                    bufferCol++;
                }

                bufferCol = 0;
            }

            return (inputData, outputData);
        }
    }
}