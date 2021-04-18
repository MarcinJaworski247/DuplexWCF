using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Timers;

namespace DuplexServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Single)]
    public class DuplexCalc : IDuplexCalc
    {
        double wynik = 0.0D;
        string rownanie;
        Thread counterThread;
        System.Timers.Timer timer;
        public int interval = 1 * 1000;

        public DuplexCalc()
        {
            rownanie = wynik.ToString();
        }
        public void Wyczysc()
        {
            Callback.Rownanie(rownanie + " + " + wynik.ToString());
            rownanie = wynik.ToString();
        }
        public void DodajDo(double n)
        {
            wynik += n;
            rownanie += " + " + n.ToString();
            Callback.Wynik(wynik);
        }
        public void OdejmijOd(double n)
        {
            wynik -= n;
            rownanie += " - " + n.ToString();
            Callback.Wynik(wynik);
        }
        public void PomnozPrzez(double n)
        {
            wynik += n;
            rownanie += " * " + n.ToString();
            Callback.Wynik(wynik);
        }
        public void PodzielPrzez(double n)
        {
            wynik /= n;
            rownanie += " / " + n.ToString();
            Callback.Wynik(wynik);
        }
        internal void CounterThreadHandler()
        {
            timer = new System.Timers.Timer(interval);
            timer.Elapsed += TimerHandler;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
        internal void TimerHandler(Object source, ElapsedEventArgs e)
        {
            this.Callback.CounterIncrement();
        }
        public void InitCounterThread()
        {
            counterThread = new Thread(new ThreadStart(CounterThreadHandler))
            {
                Name = "Counter thread",
                IsBackground = false
            };
            counterThread.Start();
        }
        public void StopCounterThread()
        {
            timer.Stop();
            timer.Dispose();
            counterThread.Abort();
        }
        IDuplexCalcCallback Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IDuplexCalcCallback>();
            }
        }
    }
}
