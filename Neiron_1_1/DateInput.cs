using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neiron_1_1
{
    class DateInput
    {
        public double[] output ;//массив результатов 
        public double[,]  input;//вводимые  данные 

        public double[] outputNorm;//массив результатов 
        public double[,] inputNorm;//вводимые  данные 

        public double[] average;
        public double[] standsrtError;
        public DateInput()
        {
            var str= "EURUSD30.csv";
            var arrayInput = Normalization(ReadFile());
            inputNorm = new double[arrayInput.GetLength(0) - 1, arrayInput.GetLength(1)-1]; ;
            outputNorm = new double[arrayInput.GetLength(0)-1];
            for(int i =0;i< arrayInput.GetLength(0)-1; i++)
            {
                for (int j = 0; j < arrayInput.GetLength(1)-1; j++)
                {
                    inputNorm[i, j] = arrayInput[i, j];
                }
                outputNorm[i] = arrayInput[i + 1, 3];
            }
        }
        void SelectingFileToRead()//смотрю какие файлы есть в каталоге Date
        {

        }
        private double[,] ReadFile()// Читаю выбранный csv в _input
        {
            //var outputs = new List<double>();
            double[,] resault;
            var inputs = new List<double[]>();
            using (var sr = new StreamReader("EURUSD30.csv"))
            {
                var header = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    var row = sr.ReadLine();
                    row= row.Remove(0, 17);
                    var valuesString = row.Split(',');
                    List<double> values = new List<double>();
                    for(int i=0;i< valuesString.Length;i++)
                    {
                        var s = valuesString[i];
                        values.Add(Convert.ToDouble(s));
                    }
                    // var output = values[3];
                    var input = values.Take(values.Count).ToArray();

                    // outputs.Add(output);
                    inputs.Add(input);
                }
                //outputs.RemoveAt(0);

                resault = ConvertToArray(inputs.ToArray()); ;

            }
            return resault ;
        }
        private double[,] ConvertToArray( double[][] input)
        {
            var resault = new double[input.Count(), input[0].Length];
            for(int i=0; i < resault.GetLength(0); i++)
            {
                for(int j=0; j < resault.GetLength(1)-1; j++)
                {
                    resault[i, j] = input[i][j];
                }
            }
            return resault;
        }
        private double[,] Normalization(double[,] inputs)
        {
            input = new double[inputs.GetLength(0) - 1, inputs.GetLength(1)-1];
            output = new double[inputs.GetLength(0) - 1];
            for (int i = 0; i < inputs.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < inputs.GetLength(1)-1; j++)
                {
                    input[i, j] = inputs[i, j];
                }
                output[i] = inputs[i + 1, 3];
            }
            average =new double[inputs.GetLength(1)-1];
            standsrtError = new double[inputs.GetLength(1)-1];
            var resault = new double[inputs.GetLength(0), inputs.GetLength(1)];
            for (int column = 0; column < inputs.GetLength(1)-1; column++)
            {
                var sum = 0.0;
                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    sum += inputs[row, column];
                }
                 average[column] = sum / inputs.GetLength(0);//среднее значение
                var error = 0.0;
                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    error += Math.Pow((inputs[row, column]), 2);
                }
                standsrtError[column] = Math.Sqrt(error / inputs.GetLength(0));//средне квадратичное отклонение
                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    resault[row, column] = Math.Abs((inputs[row, column] - average[column]) / standsrtError[column]);
                }
            }
            return resault;
        }public double ReNormalization(double res)
        {
            res = standsrtError[3] * res + average[3];
            return res;
        }

    }
   
}
