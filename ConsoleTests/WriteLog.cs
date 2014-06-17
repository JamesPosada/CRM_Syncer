using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleTests
{
   public class WriteLog
   {
       public WriteLog(string Log)
       {
           var x = File.AppendText(@"C:\SyncerLog.txt");
           x.Write(Log);
           x.Close();          
           
       }
   }
}
