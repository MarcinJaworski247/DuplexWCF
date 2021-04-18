using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DuplexServiceLibrary
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IDuplexCalcCallback))]
    public interface IDuplexCalc
    {
        [OperationContract(IsOneWay = true)]
        void Wyczysc();
        [OperationContract(IsOneWay = true)]
        void DodajDo( double n);
        [OperationContract(IsOneWay = true)]
        void OdejmijOd(double n);
        [OperationContract(IsOneWay = true)]
        void PomnozPrzez(double n);
        [OperationContract(IsOneWay = true)]
        void PodzielPrzez(double n);
        [OperationContract(IsOneWay = true)]
        void InitCounterThread();
        [OperationContract(IsOneWay = true)]
        void StopCounterThread();
    }
    public interface IDuplexCalcCallback
    {
        [OperationContract(IsOneWay = true)]
        void Wynik(double result);
        [OperationContract(IsOneWay = true)]
        void Rownanie(string eqn);
        [OperationContract(IsOneWay = true)]
        void CounterIncrement();
    }
}
