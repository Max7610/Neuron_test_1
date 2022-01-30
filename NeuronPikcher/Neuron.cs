using System;
using System.Collections.Generic;
using System.Threading;

namespace NeuronPikcher
{
    public class Neuron
    {
        public List<double> Weight { get; }
        public NeuronType Neuron_Type { get; }
        public List<double> Inputs { get; }
        public double Output { get; private set; }
        public double Delta { get; private set; }
        public Neuron(int inputCount, NeuronType type)
        {
            
            Neuron_Type = type;
            Weight = new List<double>();
            Inputs = new List<double>();

            InitWeightsRandomValue(inputCount);           
        }
        private void InitWeightsRandomValue(int inputCount)
        {
            var rnd = new Random();
            
            for (int i = 0; i < inputCount; i++)
            {
                if (Neuron_Type == NeuronType.Input)
                {
                    Weight.Add(1);
                }
                else
                {
                    Weight.Add(rnd.NextDouble()/2);
                }
               
                Inputs.Add(0);
            }
        }
      
        public double FeedForwafd(List<double> inputs)
        {   for(int i=0;i<inputs.Count;i++)
            {
                Inputs[i] = inputs[i];
            }

            var sum = 0.0;
            for(int i = 0; i < inputs.Count; i++)
            {
                sum += inputs[i] * Weight[i];
            }
            if(Neuron_Type!= NeuronType.Input)
            {
                Output = Sigmoid(sum);
            }
            else
            {
                Output = sum;
            }
            return Output; 
            

        }
       
        private double Sigmoid(double x)
        {
            var result = 1.0 / (1.0 + Math.Pow(Math.E, -x));
            return result;
        }
        private double SigmoidDx(double x)
        {
            var sigmoid = Sigmoid(x);
            var resault = sigmoid * (1 - sigmoid);
            return resault;

        }
        public double SigmoidDxOutpur()
        {
            return SigmoidDx(Output);
        }
        public void Learn(double error, double learningRate)
        {
            if(Neuron_Type == NeuronType.Input)
            {
                return;
            }
            var Delta  = error * SigmoidDx(Output);//лакальный градиент  
            for (int i=0;i<Weight.Count;i++)//перебераем веса каналов 
            {
                var weight = Weight[i]; //значение веса
                var input = Inputs[i];  //выходной сигнал 
                var newWeight = weight - input * Delta * learningRate;//рассчет новогового значения веса
                Weight[i] = newWeight;
            }
            
        }
        public override string ToString()
        {
            return Output.ToString();
        }
    }
}
