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




            //ExigoGetCustomers exigo = new ExigoGetCustomers();
            //DataControls.NewContactProccessor proccessor = new DataControls.NewContactProccessor(exigo);
            //proccessor.SetListToCheck(exigo.GetAllAccountsWithoutCrmGUIDs());
            //proccessor.ProcessList();
            DataControls.UpdateModifiedAccounts up = new DataControls.UpdateModifiedAccounts();
            up.IncrementalUpdate();
            //DataControls.NewAccountsControl nac = new DataControls.NewAccountsControl();
            //nac.CreateNewAccounts();
            DataControls.Settings.Logging.logger.WriteAllLogs();
            //DataControls.RemoveTerminatedAccounts rt = new DataControls.RemoveTerminatedAccounts();
            //rt.RemoveTermsNow();



            
            
            

        }
    }
}
