using System;

namespace NeuronNetwork
{
    
    public class Layer
    {
        public Neuron[] Neurons { get; }
        public int NeuronCount => Neurons?.Length ?? 0;
        public double[] Output { get; }
        public Layer(Neuron[] neurons)
        {//заполняем слой нейронами
            Neurons = new Neuron[neurons.Length];
            neurons.CopyTo(Neurons, 0);
            Output = new double[NeuronCount];
        }
        public double[] GetSignal(double[] input)
        {//принимаем массив значений 

            for (int i = 0; i < NeuronCount; i++)
            {   //прогоняем массив значений через каждый массив 
                Output[i] = Neurons[i].FeedForwardSigmoid(input);
            }
            return Output;
        }
        public double WeightedAmount(int n)
        {//взвешанная сумма расчитается для слоя ниже, каждому нейрону индивидуально 
            double resault = 0.0;
            for (int i = 0; i < NeuronCount; i++)
            {
                resault += Neurons[i].MultiLocalGradientAndWeight(n);
            }
            return resault;
        }
        public void SetInputLayers(double[] input)
        {//вводим значения в первый слой поштучно в каждый слой каждое значение 
            for (int i = 0; i < input.Length; i++)
            {
                Output[i] = Neurons[i].FeedForwardInput(input[i]);
            }
            Output[Output.Length-1] = Neurons[Neurons.Length-1].Output;//последний нейрон это пороговое смещение
        }
        public void LearnOutputLayer(double[] outputDelta, double learningRate)
        {//Ввожу разнику между массив разниц полеченного и требуемого
            for (int i = 0; i < NeuronCount; i++)
            {
                Neurons[i].LearnSigmoid(outputDelta[i], learningRate);
            }
        }

    }
    public class NeuralNetworks
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

        
        public void Learn(double[,] input, double[,] output, int epoch)
        {// функция обучения, проверяю топологию входных выходных данных 
            if (input.GetLength(1) != Topologi.InputCount || output.GetLength(1) != Topologi.OutputCount || input.GetLength(0) != output.GetLength(0))
            {
                Console.WriteLine($"Ошибка количества входных значений и выходных значений \n" +
                $"inp={input.Length} inpTopol={Topologi.InputCount} out={output.Length} outTop={Topologi.OutputCount} \n" +
                $"{input.GetLength(0)}   {output.GetLength(0)}");
                return;
            }
            for (int i = 0; i < epoch; i++)
            {
                for (int j = 0; j < input.GetLength(0); j++)
                {
                    BackPropagation(ConvertRowArray(input, j), ConvertRowArray(output, j));
                   // Console.WriteLine($"картинка номер {j}");
                }
                //Console.WriteLine($"i={i}");
            }
        }

        private void BackPropagation(double[] input, double[] output)
        {
            double[] outputDelta = new double[Topologi.OutputCount];//создаю массив для разници
            var outputResault = FeedForward(input);// между результатом и требумым 
            for (int i = 0; i < Topologi.OutputCount; i++)
            {//заполняю массив требуемыми значениями
                outputDelta[i] = outputResault[i] - output[i];
               // for(int k = 0; k < outputDelta.Length; k++)
              //  {
                  //  Console.WriteLine($"Ошибка {k} ={outputDelta[k]} результат {outputResault[k]}  требовалось{output[k]}");
               // }
            }//ввожу полученный массив в последний слой 
            Layers[Layers.Length-1].LearnOutputLayer(outputDelta, Topologi.LearningRate);
            //начинаю  обучение с слоя ниже до предпследнего (на входных нейронах вес всегда 1 )
            for (int i = Layers.Length - 2; i > 0; i--)//перебираю слои  
            {
                for (int j = 0; j < Layers[i].NeuronCount; j++)
                {// в каждый нейрон отправляю значение взвешанной суммы слоя выше 
                    Layers[i].Neurons[j].LearnSigmoid(Layers[i + 1].WeightedAmount(j), Topologi.LearningRate);
                    //   Console.WriteLine($"{j}тый нейрон {i}того слоя из {Layers.Length} готов");
                }//найденной специально для этого нейрона  
                 //  Console.WriteLine($"{i }тый слой готов");
            }
        }
        public double[] ConvertRowArray(double[,] array, int n)
        {// беру указанную строку из масссива 
            double[] resault = new double[array.GetLength(1)];
            for (int i = 0; i < array.GetLength(1); i++)
            {
                resault[i] = array[n, i];
            }// и возвращаю ее в виде массива 
            return resault;
        }

        public double[] FeedForward(double[] inputSignal)
        {//прогоняем вводимый массив через сеть
            SendSignalsToInputNeurons(inputSignal); //вводим данные в первый слой 
            DrawDataThroughInvisibleLayers(); //можно прогнать все результаты прошлого слоя в новый 
            return Layers[Layers.Length-1].Output;
        }


        private void DrawDataThroughInvisibleLayers()
        {//в каждый следующий слой вводим массив значений прошлого слоя 
            for (int i = 1; i <= Topologi.HiddenLayerd.Length + 1; i++)
            {//При этом начинаем с 1 недидимого слоя до последнего глобального слоя 
                Layers[i].GetSignal(Layers[i - 1].Output);
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
                var n = new Neuron(Layers[Layers.Length-2].NeuronCount);
                neuron[i] = n;
            }
            Layer layer = new Layer(neuron);
            Layers[Layers.Length - 1] = layer;
            //в нейроне надо создать количество связей равное количеству связей на последнем скрытом слое 
        }

        private void CreateHidenLauers()
        {
            for (int i = 1; i < Layers.Length - 1; i++)
            {//заполняю скрытые слои, они расположены с 1 до пред последнего слоя 
                Layers[i] = CreateHidenLauer(i);
            }
        }
        private Layer CreateHidenLauer(int n)
        {// n значение глобального слоя который создаем, но 1 глобальному слою сооветсвует 0 слой скрытого  
            Neuron[] resaylr = new Neuron[Topologi.HiddenLayerd[n - 1]+1];//
            for (int i = 0; i < Topologi.HiddenLayerd[n - 1]; i++)
            {   //заполняю нейронами стандартного (скрытого) типа 
                var neuron = new Neuron(Layers[n - 1].NeuronCount);//количество связей равно количеству нейронов
                resaylr[i] = neuron;// на предыдущем слое 
            }//добавляю пороговое смещение 
            var neuronBias = new NeuronBias(1);
            resaylr[resaylr.Length-1] = neuronBias;
            //формирую слой 
            Layer layer = new Layer(resaylr);
            return layer;
        }

        private void CreateInputLauer()
        {//Создаю на один больше для порогового смещения
            Neuron[] resaylr = new Neuron[Topologi.InputCount + 1];
            for (int i = 0; i < Topologi.InputCount; i++)
            {   //заполняю нейронами входного типа 
                var neuron = new NeuronInput(1);
                resaylr[i] = neuron;
            }//добавляю пороговое смещение 
            var neuronBias = new NeuronBias(1);
            resaylr[Topologi.InputCount] = neuronBias;
            Layer layer = new Layer(resaylr);
            Layers[0] = layer;
        }
    }
    public class Neuron
    {
        public double[] Weights { get; private set; }//вес аксона 
        public double[] Inputs { get; private set; }//массив входных значений
        public double Output { get; protected set; }// исходящий объем 
        public double LocalGradient { get; protected set; } // локальный градиент 


        public Neuron(int countInput)//Количество связей с нижним слоем 
        {// объявляю и заполняю массивы значений 
            Weights = new double[countInput];
            Inputs = new double[countInput];
            RandomWeights(countInput);
        }
        protected virtual void RandomWeights(int countInput)
        {// случайные числа для начальных весов 
            var r = new Random();
            for (int i = 0; i < countInput; i++)
            {
                Weights[i] = r.NextDouble() / 2;
            }
        }
        virtual public double FeedForwardSigmoid(double[] inputs)
        {//получаю в нейрон массив значений 
            inputs.CopyTo(Inputs, 0);
            Output = Sigmoid(SumInputs());// повожу через фомулу активации
            return Output;    //вывожу результат 
        }
        protected double SumInputs()
        {// нахожу сумму всех входных значений и умножаю их на веса 
            var sum = 0.0;
            for (int i = 0; i < Inputs.Length; i++)
            {
                sum += Inputs[i] * Weights[i];
            }
            return sum;
        }
        private double Sigmoid(double x)
        {//нахожу сигмойду (функция ативации)
            var result = 1.0 / (1.0 + Math.Pow(Math.E, -x));
            return result;
        }
        private double SigmoidDx(double x)
        {// производная сигмойды (функция деактивации) 
            var sigmoid = Sigmoid(x);
            var resault = sigmoid * (1 - sigmoid);
            return resault;

        }
        public virtual void LearnSigmoid(double weightedAmount, double learningRate)
        {// нахожу лакальный градиент как произведение взвешенной суммы слоя выше на производную
         // от функции активации
            LocalGradient = weightedAmount * SigmoidDx(Output);
            FixWeights(learningRate);
        }
        protected void FixWeights(double learningRate)
        {//перерасчитываю веса 
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = Weights[i] - learningRate * LocalGradient * Inputs[i];
            }
        }
        public virtual double MultiLocalGradientAndWeight(int n)
        {// возвращаем произведение градиента на вес указанного синапса
            return Weights[n] * LocalGradient;
        }
        public virtual double FeedForwardInput(double inputs)
        {
            return 0;
        }
    }
    public class NeuronBias : Neuron
    {
        public NeuronBias(int countInput) : base(countInput) { }
        protected override void RandomWeights(int countInput)
        {
            for (int i = 0; i < countInput; i++)
            {
                Weights[i] = 1;
            }
            Output = 1;
        }
        public override double FeedForwardSigmoid(double[] inputs)
        {//Базис не имеет ни каких значений, он всегда равен 1 
         //иначе мы не сможим сместить линию отбора ни по одной оси 
            return 1;
        }
        public override double MultiLocalGradientAndWeight(int n)
        {
            return 1;
        }
    }
    public class NeuronInput : Neuron
    {
        public NeuronInput(int countInput) : base(countInput) { }
        protected override void RandomWeights(int countInput)
        {// входной нейрон нейрон имеет 1 синапс с весом в 1 
            for (int i = 0; i < countInput; i++)
            {
                Weights[i] = 1;
            }
        }
        public override double FeedForwardInput(double inputs)
        {//выводит просто одно значение, поэтому функция активации ненужна 
            Output = inputs;
            return Output;
        }
    }
    public class NeuronOutput : Neuron
    {   /// <summary>
        /// 
        /// </summary>
        /// <param name="weightedAmount">Ввести разнику между выходны и требуемым значением</param>
        public NeuronOutput(int countInput) : base(countInput) { }

        public override void LearnSigmoid(double weightedAmount, double learningRate)
        {
            this.LocalGradient = weightedAmount * Output * (1 - Output);
            FixWeights(learningRate);
        }
    }
    public class Topologi
    {
        public int InputCount { get; }//количество входных нейронов 
        public int OutputCount { get; } //количество выходных нейронов(но у меня будет 1)
        public int[] HiddenLayerd { get; }// хранит количество нейронов в каждом скрытом слое 
        public double LearningRate;//шаг сходимости 

        public Topologi(int inputCount, int outputCount, double learningRate, int[] lauers)
        {//заполняем базовыми значениями
            LearningRate = learningRate;
            InputCount = inputCount;
            OutputCount = outputCount;
            HiddenLayerd = new int[lauers.Length];
            lauers.CopyTo(HiddenLayerd, 0);
        }
        public int CountLayers()
        {// количество слоев равно количеству невидимых плюс 1 входной и 1 выходной 
            return HiddenLayerd.Length + 2;
        }
    }
}
