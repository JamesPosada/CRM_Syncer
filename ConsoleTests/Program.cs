using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {




            ExigoGetCustomers exigo = new ExigoGetCustomers();
            DataControls.NewContactProccessor proccessor = new DataControls.NewContactProccessor(exigo);
            proccessor.SetListToCheck(exigo.GetAllAccountsWithoutCrmGUIDs());
            proccessor.ProcessList();
            DataControls.Settings.Logging.logger.WriteAllLogs();


            
            
            

        }
    }
}
