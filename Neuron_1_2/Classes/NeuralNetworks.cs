using System;
using System.Collections.Generic;
using System.Text;

namespace Neuron_1_2.Classes
{
    class NeuralNetworks
    {
        public Layer[] Layers { get; }
        public Topologi Topologi {get;}

        public NeuralNetworks(Topologi topologi)
        {
            Topologi = topologi;
            Layers = new Layer[topologi.CountLayers()];//создаю нужное количество слоев 
            CreateInputLauer();//создаю входной слой
            CreateHidenLauers();//создаю скрытые слои
            CreateOutputLauer();//создаю выходной слой 
        }

        private void CreateOutputLauer()
        {
            //как я и сказал в топологии, будет только одно выходное значение 
            Neuron[] neuron = new Neuron[1];// да да делаю массив из 1го нейрона (уже позно и я не соображаю) 
            neuron[0] = new Neuron(Topologi.hiddenLayerd[Topologi.hiddenLayerd.Length-1]);
            //в нейроне надо создать количество связей равное количеству связей на последнем скрытом слое 
        }

        private void CreateHidenLauers()
        {
           for(int i = 1; i < Layers.Length - 1; i++)
            {//заполняю скрытые слои, они расположены с 1 до пред последнего слоя 
                Layers[i] = CreateHidenLauer(i);
            }
        }
        private Layer CreateHidenLauer(int n)
        {// n значение глобального слоя который создаем, но 1 глобальному слою сооветсвует 0 слой скрытого  
            Neuron[] resaylr = new Neuron[Topologi.hiddenLayerd[n - 1] + 1];//
            for (int i = 0; i < Topologi.hiddenLayerd[n-1]; i++)
            {   //заполняю нейронами стандартного (скрытого) типа 
                var neuron = new Neuron(Layers[n-1].NeuronCount);//количество связей равно количеству нейронов
                resaylr[i] = neuron;// на предыдущем слое 
            }//добавляю пороговое смещение 
            var neuronBias = new NeuronBias(1);
            resaylr[Topologi.InputCount] = neuronBias;
            Layer layer = new Layer(resaylr);
            return layer;
        }

        private void CreateInputLauer()
        {//Создаю на один больше для порогового смещения
            Neuron[] resaylr = new Neuron[Topologi.InputCount + 1];
            for(int i=0;i< Topologi.InputCount; i++)
            {   //заполняю нейронами входного типа 
                var neuron = new NeuronInput(1);
                resaylr[i] = neuron;
            }//добавляю пороговое смещение 
            var neuronBias = new NeuronBias(1);
            resaylr[Topologi.InputCount] = neuronBias;
            Layer layer = new Layer(resaylr);
            Layers[0] =layer;
        }
    }
}
