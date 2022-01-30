using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NeuronPikcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var converter = new PictureConvertot();
            var parasitDirectory = @"C:\Users\Смайлик\source\repos\Neiron_1_1\NeuronPikcher\bin\Debug\archive\cell_images\Parasitized\";
            var unparasitDirectory = @"C:\Users\Смайлик\source\repos\Neiron_1_1\NeuronPikcher\bin\Debug\archive\cell_images\Uninfected\";
            var parasitList = Directory.GetFiles(parasitDirectory);
            var unparasitList = Directory.GetFiles(unparasitDirectory);
            Console.WriteLine("Списки файлов составлены");
            var testImageInput = converter.Convert(parasitList[0]);

            converter.Save(@"C:\Users\Смайлик\source\repos\Neiron_1_1\NeuronPikcher\bin\Debug\123.png", testImageInput);

            int size = 10000;
            var topologi = new Topologi(testImageInput.Count, 1, 0.01,300,100,20,5);
            var neuralNrteorc = new NeuralNetworks(topologi);
            double[,] parasitInput = GetData(testImageInput, parasitList, size );
            double[,] unpasitInput = GetData(testImageInput, unparasitList, size);
           
            var inputArray = PlassArray(parasitInput, unpasitInput, size);
                        
            Console.WriteLine($"Бинарный массив из {inputArray.Count} для обучения создан");
            
            neuralNrteorc.Learn(inputArray.Outputs, inputArray.Inputs, 2);

            Console.Clear();
            Console.WriteLine("Обучение завершино");

            var inp = converter.Convert(parasitList[120]);
            string Messeg = $"{neuralNrteorc.FeedForward(inp.ToArray())}    1";
            inp = converter.Convert(unparasitList[120]);
            Messeg += $"\n{neuralNrteorc.FeedForward(inp.ToArray())}    0";
            Console.WriteLine(Messeg);
            File.WriteAllText(@"C:\Users\Смайлик\source\repos\Neiron_1_1\NeuronPikcher\bin\Debug\" + DateTime.Now.Minute.ToString() + ".txt", Messeg);
            Console.ReadKey();
        }

        private static InputOutput PlassArray(double[,] parasitInput, double[,] unpasitInput, int size)
        {
            var result = new InputOutput(parasitInput.GetLength(0) * 2, parasitInput.GetLength(1));

            for (int i = 0; i < parasitInput.GetLength(0); i++)
            {
                 double[] par = new double[parasitInput.GetLength(1)];
                double[] unpar = new double[parasitInput.GetLength(1)];
                for (int j = 0; j < parasitInput.GetLength(1); j++)
                {
                    par[j] = parasitInput[i, j];
                    unpar[j] = unpasitInput[i, j];
                }
                result.Add(par, 1);
                result.Add(unpar, 0);
            }
            return result;
        }


        private static double[,] GetData(List<double> testImageInput, string[] parasitList, int size)
        {
            var converter = new PictureConvertot();

            var images = new double[size, testImageInput.Count];
            for (int i = 0; i < size; i++)
            {
                var image = converter.Convert(parasitList[i]);
                for (int j = 0; j < image.Count; j++)
                {
                    images[i, j] = image[j];
                }
                int n = Console.CursorTop;
                Console.WriteLine(i.ToString()+"     ");
                Console.SetCursorPosition(0, n);
            }
            Console.WriteLine();
            return images;
        }
        
    }
    class InputOutput
    {
        public double[,] Inputs;
        public double[] Outputs;
        public int Count ;
        public InputOutput(int size,int lenght)
        {
            Outputs = new double[size];
            Inputs = new double[size,lenght];
            Count = 0;
        }
         public void Add(double [] input,double output )
        {
            Outputs[Count] = output;
            for (int i = 0; i < Inputs.GetLength(1); i++)
            {
                Inputs[Count, i] = input[i];
            }
            Count++;
        }
    }
}
