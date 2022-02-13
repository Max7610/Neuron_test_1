

namespace Neuron_1_2.Classes
{
    class NeuronBias:Neuron
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
}
