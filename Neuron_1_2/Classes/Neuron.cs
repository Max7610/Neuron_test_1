using System;

namespace Neuron_1_2.Classes
{
    class Neuron
    {
        public double[] Weights { get; private set; }//вес аксона 
        public double[] Inputs { get; private set; }//массив входных значений
        public double Output { get; protected set; }// исходящий объем 
        public double LocalGradient { get; protected set; } // докальный градиент 
        

        public Neuron(int countInput)//Количество связей с нижним слоем 
        {// объявляю и заполняю массивы значений 
            Weights = new double[countInput];
            Inputs = new double[countInput];
            RandomWeights(countInput);
        }
        protected virtual void RandomWeights(int countInput)
        {// случайные числа для начальных весов 
            var r = new Random();
            for(int i =0;i< countInput; i++)
            {
                Weights[i] = r.NextDouble()/2;
            }
        }
        virtual public double FeedForwardSigmoid(double[] inputs)
        {//получаю в нейрон массив значений 
            inputs.CopyTo(Inputs,0);
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
        public virtual void LearnSigmoid(double weightedAmount,double learningRate)
        {// нахожу лакальный градиент как произведение взыешенной суммы слоя выше на производную
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
    }
}
