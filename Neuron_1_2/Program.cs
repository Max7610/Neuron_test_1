using System;
using Neuron_1_2.Classes;

namespace Neuron_1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] lauers = new int[] { 4 };

            Topologi topologi = new Topologi(5, 3, 0.01, lauers);
            NeuralNetworks NN = new NeuralNetworks(topologi);
            double[,] inputs = new double[,] {
                {1,1,0,0,1 },
                {1,1,0,0,0 },
                {0,1,0,0,1 },
                {0,0,0,0,1 },
                {1,0,0,0,1 },
                {1,1,0,1,0 },
                {0,1,0,1,1 },
                {0,0,0,1,1 }
            };
            double[,] output = new double[,]
            {
                {1,1,0 },
                {1,0,0 },
                {0,1,0 },
                {0,0,1 },
                {1,0,0 },
                {1,0,0 },
                {1,1,0 },
                {1,0,1 }
            };
           //var resault = NN.FeedForward(inputs);
           // PrintResault(resault);
            NN.Learn(inputs, output, 100000);
            Console.WriteLine("ok");
            PrintResault(NN.FeedForward(ConvertRowArray(inputs, 0)));
            PrintResault(ConvertRowArray(output, 0));
            Console.WriteLine();
            PrintResault(NN.FeedForward(ConvertRowArray(inputs, 1)));
            PrintResault(ConvertRowArray(output, 1));
            Console.WriteLine();
            PrintResault(NN.FeedForward(ConvertRowArray(inputs, 2)));
            PrintResault(ConvertRowArray(output, 2));
            Console.WriteLine();
            PrintResault(NN.FeedForward(ConvertRowArray(inputs, 3)));
            PrintResault(ConvertRowArray(output, 3));
            Console.WriteLine();
            Console.ReadKey();
        }
        static void PrintResault(double[] resault)
        {
            string s = "";
            for(int i = 0; i < resault.Length; i++)
            {
               s+=$"{resault[i]} ";
            }
            Console.WriteLine(s);
        }
        static private double[] ConvertRowArray(double[,] array, int n)
        {// беру указанную строку из масссива 
            double[] resault = new double[array.GetLength(1)];
            for (int i = 0; i < array.GetLength(1); i++)
            {
                resault[i] = array[n, i];
            }// и возвращаю ее в виде массива 
            return resault;
        }

    }
}
