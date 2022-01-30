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
            //var output = new double[] { 1,0,0,1,0,0,1, 1, 1, 0, 1, 0, 0, 1 };
            //var input = new double[,]
            // {
            //    {0,0,0,0},
            //    {0,0,0,1},
            //    {0,0,1,0},
            //    {0,0,1,1},
            //    {0,1,0,0},
            //    {0,1,0,1},
            //    {0,1,1,0},
            //    {0,1,1,1},
            //    {1,0,0,0},
            //    {1,0,0,1},
            //    {1,0,1,0},
            //    {1,1,0,1},
            //    {1,1,1,0},
            //    {1,1,1,0},

            // };
            var MassDate = new DateInput();
            var topology = new Topologi(MassDate.inputNorm.GetLength(1), 1,0.1,8,6,4,2);
            var neuralNework = new NeuralNetworks(topology);
            var difference = neuralNework.Learn(MassDate.outputNorm, MassDate.inputNorm, 1000);
            Console.WriteLine(difference);
            var resault = new List<double>();


            for (int i = 0; i <MassDate.output.Length; i++)
            {
                Console.WriteLine($"{MassDate.outputNorm[i]} {MassDate.output[i]}   {MassDate.ReNormalization(neuralNework.FeedForward(neuralNework.GetRow(MassDate.inputNorm, i)).Output)}   {neuralNework.FeedForward(neuralNework.GetRow(MassDate.inputNorm, i)).Output}   ");
            }
            
            Console.ReadKey();
        }

    }
}
