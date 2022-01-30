using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronPikcher
{
    class Topologi
    {
        public int InputCount { get; }
        public int OutputCount { get; }
        public List<int> hiddenLayerd { get; }
        public double LearningRate;
        public Topologi(int inputCount, int outputCount,double learningRate , params int[] layers)
        {
            LearningRate = learningRate;
            InputCount = inputCount;
            OutputCount = outputCount;
            hiddenLayerd = new List<int>();
            hiddenLayerd.AddRange(layers);
            
        }

    }
}
