using DuplexCalcProgram.DuplexCalcServiceRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DuplexCalcProgram
{
    public class CallbackHandler : IDuplexCalcCallback
    {
        public long counter = 0;
        public void Wynik(double result)
        {
            Console.WriteLine("Wynik({0})", result);
        }
        public void Rownanie(string eqn)
        {
            Console.WriteLine("Rownanie({0})", eqn);
        }
        public void CounterIncrement()
        {
            counter++;
            Console.WriteLine("Stan licznika: {0}", counter);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            InstanceContext instanceContext = new InstanceContext(new CallbackHandler());

            DuplexCalcClient client = new DuplexCalcClient(instanceContext);

            client.Open();
            client.InitCounterThread();
            client.DodajDo(5.0);
            client.DodajDo(3.0);
            client.PomnozPrzez(2.0);
            Thread.Sleep(10 * 1000);
            client.PodzielPrzez(4.0);
            client.OdejmijOd(1);
            client.StopCounterThread();
            client.Wyczysc();
            Console.ReadLine();

            client.Close();
        }
    }
}
