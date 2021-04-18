using DuplexServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace DuplexServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:8001/DuplexCalcService/");

            ServiceHost selfHost = new ServiceHost(typeof(DuplexCalc), baseAddress);

            try
            {
                selfHost.AddServiceEndpoint(typeof(IDuplexCalc), new WSDualHttpBinding(), "CalculatorService");
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true
                };
                selfHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;
                selfHost.Description.Behaviors.Add(smb);

                selfHost.Open();
                Console.WriteLine("Serwis działa...");
                Console.WriteLine("Naciśnij ENTER by zakończyć");
                Console.ReadLine();

                selfHost.Close();
            } catch(CommunicationException ex)
            {
                Console.WriteLine(ex.Message);
                selfHost.Abort();
            }
        }
    }
}
