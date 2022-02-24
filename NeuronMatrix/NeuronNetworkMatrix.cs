using System;
using System.Collections.Generic;
using System.Text;

namespace NeuronMatrix
{
    class NeuronNetworkMatrix
    {
        List<double[,]> Layers;
        List<double[]> LayersOutput;
        int[] Topology;
        public NeuronNetworkMatrix(int[] topology)
        {
            Topology = topology;
            Layers = new List<double[,]>();
            LayersOutput = new List<double[]>();
            RandomWeightHidden();//распределить случайные веса 
            RandoWeightOutput();//веса выходных нейронов
            //PrintLayers();
            
        }

        public void PrintLayersOutput()
        {
            for(int i=0;i<LayersOutput.Count;i++)
            {
                Console.WriteLine($"Слой {i}");
                for(int j = 0; j <LayersOutput[i].Length; j++)
                {
                    Console.WriteLine(LayersOutput[i][j]);
                }
            }
        }

        void PrintLayers()
        {
            for(int la = 0; la < Layers.Count; la++)
            {
                Console.WriteLine($"Слой {la}");
                for(int row =0; row < Layers[la].GetLength(0); row++)
                {
                    for(int col = 0; col < Layers[la].GetLength(1); col++)
                    {
                        Console.Write($"{Layers[la][row,col]} ");
                    }
                    Console.WriteLine();
                }
            }

        }
        void RandomWeightHidden()
        {// заполняю скрытый слой и выходной случайными значениниями 
            var r = new Random();
            for (int i = 1; i < Topology.Length - 1; i++)//перебираю слои в топологии
            {//начиная с 1, 0 - входной слой 
                var layer = new double[Topology[i] + 1, Topology[i - 1] + 1];
                //создаю слой на один нейрон больше, он будет пороговым смещением 
                //количество связей равно количеству нейронов на прошлом слое +1
                for (int j = 0; j <= Topology[i]; j++)//перебераю  все нейроны
                {
                    for (int k = 0; k <= Topology[i - 1]; k++)//перебераю веса 
                    {
                        layer[j, k] = r.NextDouble() - 0.5;//притваиваю случайное значение  -0,5 до 0,5 
                    }
                }
                Layers.Add(layer);
            }
        }
        void RandoWeightOutput()
        {//заполняю выходной слой 
            var r = new Random();
            double[,] output = new double[Topology[^1], Topology[^2]+1];
            for(int i = 0; i < Topology[^1]; i++)
            {
                for(int j = 0; j <= Topology[^2]; j++)
                {
                    output[i, j] = r.NextDouble() - 0.5;
                }
            }
            Layers.Add(output);
        }
        public double[] FeadForward(double[] input)
        {
            double[] Input = new double[input.Length+1];//в первом слое на 1 нейрон больше
           // Input.CopyTo(input, 0);
            input.CopyTo(Input, 0);
            Input[^1] = 1;//добавляю пороговое смещение 
            LayersOutput.Add(Input);// добавляю это значение в список
            //запускаю умнажение каждого нового слоя на результат умножения прошлого 
            CreateHiddenLayer();
            return LayersOutput[^1];
        }
        void CreateHiddenLayer()
        {
            for (int i=0;i<Layers.Count;i++) {//перебераю слои 
                //умножаю первый слой первый вектор
                var output = PassingToLayer(Layers[i], LayersOutput[i]);
                LayersOutput.Add(output);
                
            }
        }
        double act(double x)
        {// функция ативации 
            return (2 / (1 + Math.Exp(-x))) - 1;
        }
        double dAct(double x)
        {//функция деактивации
            return 1 / 2 * (1 + act(x)) * (1 - act(x));
        }
        public double[] PassingToLayer(double[,] matrix, double[] vector)
        {//умножение вектора на число 
            double[] resault = new double[matrix.GetLength(0)+1];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    resault[i] += vector[j] + matrix[i, j];
                }
                resault[i] = act(resault[i]);
            }
            resault[^1] = 1;
            return resault;
        }
    }
}
