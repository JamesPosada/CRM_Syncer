using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace DataLogger
{
    public class Logger : LoggerBase
    {
      
        private Dictionary<string, int> _headerCount;

        public Logger()
        {
            Settings.LogFileSettings.CreateFolder();
        }


        #region Public Methods
        public void WriteAllLogs()
        {
            
            base.wrtiteLogs();

        }

        public string SetLogName(string logName, List<string> headers)
        {

            if (headers.Any(c => c.Contains(",")))
            {
                throw new Exception("No Commas allowed in Log");
            }
            if ( base.GetLogKey(logName) !="Key Not Found")
            {
                return logName;
            }
            StringBuilder csvHeaders = new StringBuilder();
            foreach (string header in headers)
            {
                csvHeaders.Append(header + ",");
            }

            if (_headerCount == null)
            {
                _headerCount = new Dictionary<string, int>();
            }
            _headerCount.Add(logName, headers.Count);
            csvHeaders.Append("Date" + "\r\n");
            return SetLog(logName, csvHeaders.ToString());

        }




        public void LogData(string LogName, List<string> content)
        {
            if (base.GetLogKey(LogName) == "Key Not Found")
            {
                throw new Exception("No such log, please use SetLog method");
               
            }
            var logName = base.GetLogKey(LogName);
            
            base.commitEntry(new KeyValuePair<string, string>(logName, ParseContent(LogName, content)));
           
        }

        #endregion
        #region Private Methods
        private void CreateFile(string path, string headers)
        {
            var file = File.CreateText(path);
            file.WriteAsync(headers).GetAwaiter().OnCompleted(file.Close);
        }

        private string SetLog(string logName, string headers)
        {

            if (!File.Exists(Settings.LogFileSettings.Path + logName + ".csv"))
            {
                CreateFile(Settings.LogFileSettings.Path + logName + ".csv", headers);
            }

            base.AddLog(logName);
            return logName;
        }

        private string ParseContent(string logName, List<string> content)
        {
            if (content.Count != _headerCount.Where(k => k.Key == logName).FirstOrDefault().Value)
            {
                throw new Exception("Not Enough Columns for Log, Log File:" + logName + " requires " +
                    _headerCount.Where(k => k.Key == logName).FirstOrDefault().Value + " values."
                );
            }

            string logLine = string.Empty;
            foreach (string entry in content)
            {
                logLine = logLine + entry + ",";
            }
            return logLine + DateTime.Now.ToString() + "," + "\r\n";
        }

        #endregion

    }


    public class LoggerBase
    {
        private bool Locked = false;
        private Dictionary<string, string> _logs;
        protected LoggerBase()
        {
            _logs = new Dictionary<string, string>();
        }
        protected void commitEntry(KeyValuePair<string, string> entry)
        {
            while (Locked == true)
            {
            Wait:
                Thread.SpinWait(15);

                if( Locked == true)
                {
                    goto Wait;
                }
            }
            
            lock (_logs)
            {
                Locked = true;
                KeyValuePair<string, string> newEntry = new KeyValuePair<string, string>(entry.Key, _logs.Where(k => k.Key == entry.Key).FirstOrDefault().Value + entry.Value);
                _logs.Remove(entry.Key);
                _logs.Add(newEntry.Key, newEntry.Value);
            }
            Locked = false;
        }


        internal string GetLogKey(string RequestedKey)
        {
            while (Locked == true)
            {
            Wait:
                Thread.SpinWait(15);

                if (Locked == true)
                {
                    goto Wait;
                }
            }
            lock (_logs)
            {
                Locked = true;

                if (_logs.ContainsKey(RequestedKey))
                {
                    Locked = false;
                    return _logs.Keys.Where(k => k == RequestedKey).FirstOrDefault();
                }
            }
            Locked = false;
            return "Key Not Found";
        }


        internal void AddLog(string logName)
        {
            while (Locked == true)
            {
            Wait:
                Thread.SpinWait(15);

                if (Locked == true)
                {
                    goto Wait;
                }
            }

            if (!_logs.ContainsKey(logName))
            {
                Locked = true;
                lock (_logs)
                {
                    _logs.Add(logName, string.Empty);
                }
            }
            Locked = false;
        }

        internal void wrtiteLogs()
        {
            while (Locked == true)
            {
            Wait:
                Thread.SpinWait(15);

                if (Locked == true)
                {
                    goto Wait;
                }
            }

            lock (_logs)
            {
                Locked = true;
                List<string> LogNames = new List<string>();
                foreach (var log in _logs)
                {

                    var file = File.AppendText(Settings.LogFileSettings.Path + log.Key + ".csv");
                    file.Write(log.Value);
                    file.Close();
                    LogNames.Add(log.Key);
                }
                _logs.Clear();
                foreach (string name in LogNames)
                {
                    _logs.Add(name, string.Empty);
                }
            }
            Locked = false;
        }
    }
}
