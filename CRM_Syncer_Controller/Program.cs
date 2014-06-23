using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataControls;

namespace CRM_Syncer_Controller
{
    class Program
    {
        static void Main(string[] args)
        {
            RemoveTerminatedAccounts RTA = new RemoveTerminatedAccounts();
            RTA.RemoveTermsNow();
            UpdateExigoGuids UEG = new UpdateExigoGuids();
            UEG.ExecuteUpdate();
            UpdateModifiedAccounts UMA = new UpdateModifiedAccounts();
            UMA.IncrementalUpdate();
            NewAccountsControl MAC = new NewAccountsControl();
            MAC.CreateNewAccounts();


        }
    }
}
