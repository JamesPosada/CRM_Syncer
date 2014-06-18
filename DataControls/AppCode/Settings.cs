using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLogger;

namespace DataControls
{
    public class Settings
    {
        public static class Logging
        {
            static Logger _logger;
            public static Logger logger
            {
                get
                {
                    if (_logger == null)
                    {
                        _logger = new Logger();
                       
                    }
                    return _logger;
                }
            }

            public static class AccountsUpdateLog
            {
                public static string Name = "SyncerUpdatingLog";
                public static List<string> Headers = new List<string>()
                {
                    "CRM GUID",
                    "Exigo ID",                    
                    "Field Name",
                    "Old Value",
                    "New Value"
                };
            }

            public static class AccountCheckingLog
            {
                public static string Name = "SyncerAccountCheckLog";
                public static List<string> Headers = new List<string>()
                {
                    "Exigo ID",
                    "Crm ID for Exigo",
                    "Match Found",
                    "Match GUID",
                    "Exigo FName",
                    "Exigo LName",
                    "Exigo Email",
                    "Crm Fname",
                    "Crm Lname",
                    "Crm Email"

                };
            }
        }
    }
}
