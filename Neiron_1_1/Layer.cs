using System;
using System.Collections.Generic;

namespace Neiron_1_1
{
    public class Layer
    {
        public List<Neuron> Neurons { get; }
        public int  NeuronCount => Neurons?.Count ?? 0;
        public NeuronType Type;

        public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Normal)
        {
            //TODO: проверить все входные нейроны на соответсвие типу 
            Neurons = neurons;
            Type = type;

        }
        public List<double> GetSignals()
        {
            var resault = new List<double>();
            foreach(var neuron in Neurons)
            {
                resault.Add(neuron.Output);
            }
            return resault;
        }
        public override string ToString()
        {
            return Type.ToString();
        }

    }
}
