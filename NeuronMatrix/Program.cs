using System;

namespace NeuronMatrix
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] vector = new double[] { -1, 1, 0 };

            double[,] layer = new double[,] {

            {-0.2,0.3,-0.1},  //веса первого нейрона  
            {0.1,-0.3,0.4},
            {0.2,0.3,-0.7},
            {0.5,-0.23,0.33}
            };

            double[] inputVector = new double[] {1,-1,1,0 ,0 };



            // Console.WriteLine(layer[0,1]); 
            int[] top = new int[] {5,4,3,2};
            var neuron = new NeuronNetworkMatrix(top);
            var resault = neuron.FeadForward(inputVector);
            neuron.PrintLayersOutput();
            // var resault= neuron.PassingToLayer(inputVector);
            Console.WriteLine("результат");
           for(int i = 0; i < top[^1]; i++)
            {
                Console.WriteLine($"{resault[i]}");
            } 
           
        }
        
    }
}
