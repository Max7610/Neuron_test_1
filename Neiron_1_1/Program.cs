using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neiron_1_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var output = new double[] { 1,0,0,1,0,0,1, 1, 1, 0, 1, 0, 0, 1 };
            var input = new double[,]
             {
                {0,0,0,0},
                {0,0,0,1},
                {0,0,1,0},
                {0,0,1,1},
                {0,1,0,0},
                {0,1,0,1},
                {0,1,1,0},
                {0,1,1,1},
                {1,0,0,0},
                {1,0,0,1},
                {1,0,1,0},
               {1,1,0,1},
               {1,1,1,0},
                {1,1,1,0},

             };
           // var MassDate = new DateInput();
            var topology = new Topologi(output.Length, 1,0.1,8,6,4,2);
            var neuralNework = new NeuralNetworks(topology);
            var difference = neuralNework.Learn(output, input, 1000);
            Console.WriteLine(difference);
            var resault = new List<double>();


            for (int i = 0; i < output.Length; i++)
            {
                Console.WriteLine($"{output[i]}   {neuralNework.FeedForward(neuralNework.GetRow(input, i)).Output}    ");
            }
            
            Console.ReadKey();
        }

    }
}
