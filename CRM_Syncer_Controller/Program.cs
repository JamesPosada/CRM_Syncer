using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataControls;
using System.Runtime.InteropServices;

namespace CRM_Syncer_Controller
{
    class Program
    {

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            Console.Title = "The Syncer";
            IntPtr hWnd = FindWindow(null, "The Syncer"); 
            if(hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 0);
            }

            // Step one we first find all new terminated accounts in Exigo with GUID's
            RemoveTerminatedAccounts RTA = new RemoveTerminatedAccounts();
            // Step two we change the status and customer type of the account in CRM and remove the GUID from Exigo so changes to account
            /// no longer affect crm.
            RTA.RemoveTermsNow();
            /// Following 2 lines are step 3, here we are adding the guid's for all accounts in CRM that have exigo id numbers on them 
            /// where CRM GUID (Other 8) is blank in Exigo this allows end users to add an ID number to a CRM account and the Syncer will populate/overwrite with Exigo Record.
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
