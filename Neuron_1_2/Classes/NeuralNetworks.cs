using System;
using System.Collections.Generic;
using System.Text;

namespace Neuron_1_2.Classes
{
    class NeuralNetworks
    {
        public Layer[] Layers { get; }
        public Topologi Topologi { get; }

        public NeuralNetworks(Topologi topologi)
        {
            Topologi = topologi;
            Layers = new Layer[topologi.CountLayers()];//создаю нужное количество слоев 
            CreateInputLauer();//создаю входной слой
            CreateHidenLauers();//создаю скрытые слои
            CreateOutputLauer();//создаю выходной слой 
        }

        //TODO : остлось  функция обучения ...... эх
        public void Learn(double[,] input,double[,] output,int epoch)
        {// функция обучения, проверяю топологию входных выходных данных 
            if(input.GetLength(1) !=Topologi.InputCount || output.GetLength(1) != Topologi.OutputCount || input.GetLength(0) !=output.GetLength(0))
            {
                Console.WriteLine($"Ошибка количества входных значений и выходных значений \n" +
                $"inp={input.Length} inpTopol={Topologi.InputCount} out={output.Length} outTop={Topologi.OutputCount} \n" +
                $"{input.GetLength(0)}   {output.GetLength(0)}");
                return;
            }
            for(int i = 0; i < epoch; i++)
            {
                for(int j=0;j< input.GetLength(0);j++)
                {
                    BackPropagation(ConvertRowArray(input, j), ConvertRowArray(output, j));
                    //Console.WriteLine(j);
                }
                //Console.WriteLine($"i={i}");
            }
        }

        private void BackPropagation(double[] input, double[] output)
        {
            double[] outputDelta = new double[Topologi.OutputCount];//создаю массив для разници
            var outputResault = FeedForward(input);// между результатом и требумым 
            for(int i = 0; i < Topologi.OutputCount; i++)
            {//заполняю массив требуемыми значениями
                outputDelta[i] = outputResault[i] - output[i];
            }//ввожу полученный массив в последний слой 
            Layers[^1].LearnOutputLayer(outputDelta,Topologi.LearningRate);
            //начинаю  обучение с слоя ниже до предпследнего (на входных нейронах вес всегда 1 )
            for(int i = Layers.Length - 2; i > 0; i--)//перебираю слои  
            {
                for(int j = 0; j < Layers[i].NeuronCount; j++)
                {// в каждый нейрон отправляю значение взвешанной суммы слоя выше 
                    Layers[i].Neurons[j].LearnSigmoid(Layers[i + 1].WeightedAmount(j), Topologi.LearningRate);
                 //   Console.WriteLine($"{j}тый нейрон {i}того слоя из {Layers.Length} готов");
                }//найденной специально для этого нейрона  
              //  Console.WriteLine($"{i }тый слой готов");
            }
        }
        private double[] ConvertRowArray(double[,] array,int n)
        {// беру указанную строку из масссива 
            double[] resault = new double[array.GetLength(1)];
            for(int i=0;i< array.GetLength(1); i++)
            {
                resault[i] = array[n, i];
            }// и возвращаю ее в виде массива 
            return resault;
        }

        public double[] FeedForward(double[] inputSignal)
        {//прогоняем вводимый массив через сеть
            SendSignalsToInputNeurons(inputSignal); //вводим данные в первый слой 
            DrawDataThroughInvisibleLayers(); //можно прогнать все результаты прошлого слоя в новый 
            return Layers[^1].Output;
        }

      
        private void DrawDataThroughInvisibleLayers()
        {//в каждый следующий слой вводим массив значений прошлого слоя 
            for(int i = 1; i <= Topologi.HiddenLayerd.Length+1; i++)
            {//При этом начинаем с 1 недидимого слоя до последнего глобального слоя 
                Layers[i].GetSignal(Layers[i-1].Output);
            }//вводим результат прошлого слоя в текущий слой 
        }

        private void SendSignalsToInputNeurons(double[] inputSignal)
        {//вводим данные в первый слой 
            Layers[0].SetInputLayers(inputSignal);
        }

        private void CreateOutputLauer()
        {
            //Создаю массив выходных значений  
            Neuron[] neuron = new Neuron[Topologi.OutputCount];// 
            for (int i = 0; i < Topologi.OutputCount; i++)
            {
                var n = new Neuron(Layers[^2].NeuronCount);
                neuron[i] = n;
            }
            Layer layer = new Layer(neuron);
            Layers[^1] = layer;
            //в нейроне надо создать количество связей равное количеству связей на последнем скрытом слое 
        }

        private void CreateHidenLauers()
        {
           for(int i = 1; i < Layers.Length - 1; i++)
            {//заполняю скрытые слои, они расположены с 1 до пред последнего слоя 
                Layers[i] = CreateHidenLauer(i);
            }
        }
        private Layer CreateHidenLauer(int n)
        {// n значение глобального слоя который создаем, но 1 глобальному слою сооветсвует 0 слой скрытого  
            Neuron[] resaylr = new Neuron[Topologi.HiddenLayerd[n - 1] + 1];//
            for (int i = 0; i < Topologi.HiddenLayerd[n-1]; i++)
            {   //заполняю нейронами стандартного (скрытого) типа 
                var neuron = new Neuron(Layers[n-1].NeuronCount);//количество связей равно количеству нейронов
                resaylr[i] = neuron;// на предыдущем слое 
            }//добавляю пороговое смещение 
            var neuronBias = new NeuronBias(1);
            resaylr[Topologi.InputCount-1] = neuronBias;
            //формирую слой 
            Layer layer = new Layer(resaylr);
            return layer;
        }

        private void CreateInputLauer()
        {//Создаю на один больше для порогового смещения
            Neuron[] resaylr = new Neuron[Topologi.InputCount + 1];
            for(int i=0;i< Topologi.InputCount; i++)
            {   //заполняю нейронами входного типа 
                var neuron = new NeuronInput(1);
                resaylr[i] = neuron;
            }//добавляю пороговое смещение 
            var neuronBias = new NeuronBias(1);
            resaylr[Topologi.InputCount] = neuronBias;
            Layer layer = new Layer(resaylr);
            Layers[0] =layer;
        }
    }
}
