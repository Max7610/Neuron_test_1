

namespace Neuron_1_2.Classes
{
    class NeuronOutput:Neuron
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
}
