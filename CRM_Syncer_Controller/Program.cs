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
