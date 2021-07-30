using System;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace YSAProject
{
    internal class ReadExcel : IReadFile
    {
        private readonly string _path;
        private string[,] _data;
        private HSSFWorkbook _hssfwb;
        private ISheet _sheet;

        public ReadExcel(string path)
        {
            _path = path;
            OpenFolder();
        }

        public double[,] ReadData()
        {
            if (_sheet != null)
            {
                _data = new string[_sheet.LastRowNum, _sheet.GetRow(0).LastCellNum];

                for (var row = 1; row <= _sheet.LastRowNum + 1; row++)
                for (var column = 0; column < _sheet.GetRow(0).LastCellNum; column++)
                {
                    if (_sheet.GetRow(row) == null) continue;
                    var value = _sheet.GetRow(row).GetCell(column).ToString();
                    _data[row - 1, column] = value;
                }
            }

            var doubleData = TurnDouble(_data);
            var unused = new Normalization(_data.GetUpperBound(0) + 1, _data.GetUpperBound(1) + 1);

            return Normalization.Normalize(doubleData);
        }

        private void OpenFolder()
        {
            try
            {
                using (var file = new FileStream(_path, FileMode.Open, FileAccess.Read))
                {
                    _hssfwb = new HSSFWorkbook(file);
                }

                _sheet = _hssfwb.GetSheetAt(0);
            }
            catch (Exception e)
            {
                Console.WriteLine("Hata: " + e);
            }
        }

        private static double[,] TurnDouble(string[,] data)
        {
            var doubleData = new double[data.GetUpperBound(0) + 1, data.GetUpperBound(1) + 1];
            for (var i = 0; i <= data.GetUpperBound(0); i++)
            for (var j = 0; j <= data.GetUpperBound(1); j++)
                doubleData[i, j] = Convert.ToDouble(data[i, j]);
            return doubleData;
        }
    }
}