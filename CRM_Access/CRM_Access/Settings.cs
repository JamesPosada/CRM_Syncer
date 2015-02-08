using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrezzorXCrm
{
    public static class Settings {
        public static class CrmCredentials {
            public static string UserName = @"j.posada";

            public static string Domain = "datacenter";
            public static string Server = @"https://frezzor.dataresolution.net";
            public static byte[] Password {
                get {
                    return Utilities.PKEncode(@"YWRhc29wc2VtYWo=");
                }
            }
            public static string DomainWithUserName {
                get { return string.Format(@"{0}\{1}", Domain, UserName); }
            }
        }


    }
     
}
