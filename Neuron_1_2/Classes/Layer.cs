

namespace Neuron_1_2.Classes
{
    class Layer
    {
        public Neuron[] Neurons { get; }
        public int NeuronCount => Neurons?.Length ?? 0;
        public double[] Output { get; }
        public Layer(Neuron[] neurons)
        {//заполняем слой нейронами
            Neurons = new Neuron[neurons.Length];
            neurons.CopyTo(Neurons,0);
            Output = new double[NeuronCount];
        }
        public double[] GetSignal(double[] input)
        {//принимаем массив значений 
            
            for(int i = 0; i < NeuronCount; i++)
            {   //прогоняем массив значений через каждый массив 
                Output[i] = Neurons[i].FeedForwardSigmoid(input);
            }
            return Output;
        }
        public double WeightedAmount(int n)
        {//взвешанная сумма расчитается для слоя ниже, каждому нейрону индивидуально 
            double resault = 0.0;
            for(int i = 0; i < NeuronCount; i++)
            {
                resault += Neurons[i].MultiLocalGradientAndWeight(n);
            }
            return resault;
        }
        public void SetInputLayers(double[] input)
        {//вводим значения в первый слой поштучно в каждый слой каждое значение 
            for(int i = 0; i < input.Length; i++)
            {
                Output[i] = Neurons[i].FeedForwardInput(input[i]);
            }
            Output[^1] = Neurons[^1].Output;//последний нейрон это пороговое смещение
        }
        public void LearnOutputLayer(double[] outputDelta,double learningRate)
        {//Ввожу разнику между массив разниц полеченного и требуемого
            for(int i=0;i<NeuronCount;i++)
            {
                Neurons[i].LearnSigmoid(outputDelta[i], learningRate);
            }
        }

    }
}
