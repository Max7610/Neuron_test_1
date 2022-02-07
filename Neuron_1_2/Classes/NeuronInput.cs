﻿

namespace Neuron_1_2.Classes
{
    class NeuronInput:Neuron
    {
        public NeuronInput(int countInput) : base(countInput) { }
        protected override void RandomWeights(int countInput)
        {// входной нейрон нейрон имеет 1 синапс с весом в 1 
            for (int i = 0; i < countInput; i++)
            {
                Weights[i] = 1;
            }
        }
        public override double FeedForwardSigmoid(double[] inputs)
        {//выводит просто одно значение, поэтому функция активации ненужна 
            inputs.CopyTo(Inputs, 0);
            Output =SumInputs();
            return Output;
        }
    }
}
