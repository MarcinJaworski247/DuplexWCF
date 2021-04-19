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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
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
        internal void CounterThreadHandler(object callbackParam)
        {
            IDuplexCalcCallback callback = (IDuplexCalcCallback)callbackParam;
            timer = new System.Timers.Timer(interval);
            timer.Elapsed += (source, e) => TimerHandler(source, e, callback);
            timer.AutoReset = true;
            timer.Enabled = true;
        }
        internal void TimerHandler(Object source, ElapsedEventArgs e, IDuplexCalcCallback callback)
        {
            callback.CounterIncrement();
        }
        public void InitCounterThread()
        {
            counterThread = new Thread(CounterThreadHandler)
            {
                Name = "Counter thread",
                IsBackground = false
            };
            counterThread.Start(Callback);
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
