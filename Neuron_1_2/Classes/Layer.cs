

namespace Neuron_1_2.Classes
{
    class Layer
    {
        public Neuron[] Neurons { get; }
        public int NeuronCount => Neurons?.Length ?? 0;
        public Layer(Neuron[] neurons)
        {//заполняем слой нейронами
            neurons.CopyTo(Neurons,0);
        }
        public double[] GetSignal(double[] input)
        {//принимаем массив значений 
            double[] resault = new double[NeuronCount];
            for(int i = 0; i < NeuronCount; i++)
            {   //прогоняем массив значений через каждый массив 
                resault[i] = Neurons[i].FeedForwardSigmoid(input);
            }
            return resault;
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

    }
}
