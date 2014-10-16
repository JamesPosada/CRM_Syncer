using DataAccess;
using DataControls;
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
         
          //  DataControls.UpdateModifiedAccounts up = new DataControls.UpdateModifiedAccounts();
           // up.IncrementalUpdate();         
            //DataControls.Settings.Logging.logger.WriteAllLogs();
            //DataControls.Updater uma = new DataControls.Updater();
            //uma.DeleteAllOnes();

            RemoveTerminatedAccounts RTA = new RemoveTerminatedAccounts();
            RTA.RemoveTermsNow();
            UpdateExigoGuids UEG = new UpdateExigoGuids();
            UEG.ExecuteUpdate();
            UpdateModifiedAccounts UMA = new UpdateModifiedAccounts();
            UMA.IncrementalUpdate();
            NewAccountsControl MAC = new NewAccountsControl();
            MAC.CreateNewAccounts();
            DataControls.Settings.Logging.logger.WriteAllLogs();
        }
    }
}
