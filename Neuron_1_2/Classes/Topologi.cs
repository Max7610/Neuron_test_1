using System;
using System.Collections.Generic;
using System.Text;

namespace Neuron_1_2.Classes
{
    class Topologi
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
            lauers.CopyTo(HiddenLayerd,0);
        }
        public int CountLayers()
        {// количество слоев равно количеству невидимых плюс 1 входной и 1 выходной 
            return HiddenLayerd.Length + 2;
        }
    }
}
