using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataLogger
{
    public class Settings
    {
        public class LogFileSettings
        {
            public static string Path =@"C:\Exigo_CRM_Sync_Logs\" + DateTime.Now.Year + "_" + DateTime.Now.Month.ToString() +@"\";
            
            public static void CreateFolder()
            {
                if(!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
            }


            
        }


        public static class Mail
        {
            public static string SMTPServerUrl = "mail.exigo.com";
            public static int SMTPServerPort = 26;
            public static bool SMTPSecureConnectionRequired = false;
            public static string SMTPServerLoginName = "noreply@exigonow.com";
            public static string SMTPServerPassword = "whodaman";
            public static string ReplyEmailAddress = "customercare@frezzor.com";
            public static string FromEmail = "Logger@frezzor.com";
            public static string FromName = "The Logger";
        }
    }
}
