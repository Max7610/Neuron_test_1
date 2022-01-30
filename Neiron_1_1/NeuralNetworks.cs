using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neiron_1_1
{
    class NeuralNetworks
    {
        public List<Layer> Layers { get; }
        public Topologi Topologi { get; }

        public NeuralNetworks(Topologi topologi)
        {
            Topologi = topologi;
            Layers = new List<Layer>();
            CreateInputLauer();
            CreateHidenLauers();
            CreateInputLauers();
        }
        public Neuron FeedForward(params double[] inputSignal)
        {
            SendSignalsToInputNeurons(inputSignal);
            FeedForwardAllLayersAfterInput();
            if (Topologi.OutputCount == 1)
            {
                return Layers.Last().Neurons[0];
            } else
            {
                return Layers.Last().Neurons.OrderByDescending(n => n.Output).First();
            }
        }
        public double Learn(double[] expected,double[,] inputs, int epoch)
        {
            var error = 0.0;
            for (int i = 0; i < epoch; i++)//такты отладки
            {
                for (int j = 0; j < expected.Length; j++)//перебор массива значений для отладки значений
                {
                    var output = expected[j];
                    var input = GetRow(inputs, j);
                    error += Backpropagation(output, input); // запуск одного такта отладки
                }
                PrintProgress(i, epoch);
            }
            var result = error / epoch;
            return result;
        }
        public double LearnWhile(double[] expected, double[,] inputs)
        {
            var error = 0.0;
            int count = 0;
            do//такты отладки
            {
                count++;
                for (int j = 0; j < expected.Length; j++)//перебор массива значений для отладки значений
                {
                    var output = expected[j];
                    var input = GetRow(inputs, j);
                    error += Backpropagation(output, input); // запуск одного такта отладки
                }
                 PrintError((error/count));
            } while ((error / count)  > 1);
            var result = error / count ;
            return result;
        }
        void PrintError(double error)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"обучение, прогресс {error }");
        }
        private double Backpropagation(double exrected, params double[] inputSignal)//цикл отадки
        {
            var actual = FeedForward(inputSignal).Output;
            var difference = actual - exrected;// находим ошибку
            foreach (var neuron in Layers.Last().Neurons)// на всех выходных нейронах запускаем 
            {
                neuron.Learn(difference, Topologi.LearningRate);// менеям веса последнего слоя 

            }
            for (int j = Layers.Count - 2; j >= 0; j--)//начинаем перебирать слои с права налево 
            {
                var lauer = Layers[j]; // сохраняем слой в перенную 
                var previosLauer = Layers[j + 1];//запоминаем следующий слой 
                for (int i = 0; i < lauer.NeuronCount; i++)// перебираем неироны в слое 
                {
                    var neuron = lauer.Neurons[i];//сохраняем нейрон в переменную 
                    var localGradirn = 0.0; // значение лакального градиента для  данного нейрона 
                    var weightedAmount = 0.0; // взвешанная сумма равна сумме произведений 
                    //локального градиента слоя выше и связи с ним 
                 
                    for (int k = 0; k < previosLauer.NeuronCount; k++)//перебираем веса связей
                    {
                        var privionsNeuron = previosLauer.Neurons[k];// берем связь выше 
                        weightedAmount += privionsNeuron.Weight[i] * privionsNeuron.Delta; //уножаем вес на градиент
                    }// складывая эти значения находим взвешанную сумму 
                    localGradirn = weightedAmount*neuron.SigmoidDxOutpur();
                    neuron.Learn(localGradirn, Topologi.LearningRate);
                }
            }
            var res = difference * difference;
            return res;

        }
        public double[] GetRow(double[,] matrix,int row)
        {
            var columns = matrix.GetLength(1);
            var array = new double[columns];
            for (int i=0;i<columns;i++)
            
                array[i] = matrix[row, i];
            return array;
        }
        private double[,] Scalling(double[,] inputs)
        {
            var resault = new double[inputs.GetLength(0), inputs.GetLength(1)];
            for(int column = 0; column < inputs.GetLength(1); column++)
            {
                var max = inputs[0, column];
                var min = inputs[0, column];

                for(int row =1; row< inputs.GetLength(0); row++)
                {
                    var item = inputs[row, column];
                    if(item < min)
                    {
                        min = item;
                    }
                    if(item > max)
                    {
                        max = item;
                    }
                }
                for(int row =1; row < inputs.GetLength(0); row++)
                {
                    resault[row, column] = (inputs[row, column] - min) / (max - min);
                }
            }
            return resault;
        }
        private double[,] Normalization(double[,] inputs)
        {
            var resault = new double[inputs.GetLength(0), inputs.GetLength(1)];
            for (int column = 0; column < inputs.GetLength(1); column++)
            {
                var sum = 0.0;
                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    sum += inputs[row,column] ;
                }
                var average = sum / inputs.GetLength(0);
                var error = 0.0;
                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    error += Math.Pow((inputs[row, column]), 2);
                }
                var standsrtError = Math.Sqrt(error / inputs.GetLength(0)); 
                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    resault[row, column] = (inputs[row, column] - average) / standsrtError;
                }
            }
            return resault;
        }

            void PrintProgress(int i,int epohc)
        {   if (100 * i % epohc == 0)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"обучение, прогресс {100 * i/ epohc }");
            }
        }
       
        private void FeedForwardAllLayersAfterInput()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                var previousLayerSignal = Layers[i - 1].GetSignals();
                foreach (var neuron in layer.Neurons)
                {
                    neuron.FeedForwafd(previousLayerSignal);
                }
            }
        }
        void SendSignalsToInputNeurons(params double[] inputSignal)
        {
            for (int i = 0; i < inputSignal.Length; i++)
            {
                var signal = new List<double>() { inputSignal[i] };
                var neuron = Layers[0].Neurons[i];

                neuron.FeedForwafd(signal);
            }
        }
        private void CreateInputLauers()
        {
            var outuptNeuron = new List<Neuron>();
            var lastLayer = Layers.Last();
            for (int i = 0; i < Topologi.OutputCount; i++)
            {
                var neuron = new Neuron(lastLayer.NeuronCount, NeuronType.Output);
                outuptNeuron.Add(neuron);
            }
            var outuptLauer = new Layer(outuptNeuron, NeuronType.Output);
            Layers.Add(outuptLauer); 
        }

        private void CreateHidenLauers()
        {   for (int j = 0; j < Topologi.hiddenLayerd.Count; j++)
            {
                var hiddenNeuron = new List<Neuron>();
                var lastLayer = Layers.Last();
                for (int i = 0; i < Topologi.hiddenLayerd[j]; i++)
                {
                    var neuron = new Neuron(lastLayer.NeuronCount,NeuronType.Normal);
                    hiddenNeuron.Add(neuron);
                }
                var outuptLauer = new Layer(hiddenNeuron, NeuronType.Output);
                Layers.Add(outuptLauer);
            }
        }

        private void CreateInputLauer()
        {
            var inputNeuron = new List<Neuron>();
            for(int i=0;i< Topologi.InputCount; i++)
            {
                var neuron = new Neuron(1, NeuronType.Input);
                inputNeuron.Add(neuron);
            }
            var inputLauer = new Layer(inputNeuron, NeuronType.Input);
            Layers.Add(inputLauer);
        }
    }
}
