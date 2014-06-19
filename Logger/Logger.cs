using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataLogger
{
    public class Logger
    {
        public Dictionary<string, string> Logs
        {
            get
            {
                if (_logs == null)
                {
                    _logs = new Dictionary<string, string>();
                }
                return _logs;
            }
        }
        private Dictionary<string, string> _logs;
        private Dictionary<string, int> _headerCount;

        public Logger()
        {
            Settings.LogFileSettings.CreateFolder();
        }


        #region Public Methods
        public void WriteAllLogs()
        {
            List<string> LogNames = new List<string>();
            
            foreach (var log in _logs)
            {

                var file = File.AppendText(Settings.LogFileSettings.Path + log.Key + ".csv");
                file.Write(log.Value);
                file.Close();
                LogNames.Add(log.Key);
            }
            Logs.Clear();
            foreach(string name in LogNames)
            {
                Logs.Add(name, string.Empty);
            }
        }

        public string SetLogName(string logName, List<string> headers)
        {

            if (headers.Any(c => c.Contains(",")))
            {
                throw new Exception("No Commas allowed in Log");
            }
            if (Logs.ContainsKey(logName))
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

        public void LogData(string LogName, List<string>content)
        {
            if(!Logs.ContainsKey(LogName))
            {
                throw new Exception("No such log, please use SetLog method");
            }
            var log = Logs.Where(k => k.Key == LogName).FirstOrDefault();
            string logdata = log.Value + ParseContent(LogName, content);
            Logs.Remove(log.Key);
            Logs.Add(log.Key, logdata);
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

            if (!File.Exists(Settings.LogFileSettings.Path +"\\" + DateTime.Now.Year+ "_"+ DateTime.Now.Month + logName + ".csv"))
            {
                CreateFile(Settings.LogFileSettings.Path + "\\" + DateTime.Now.Year+ "_"+ DateTime.Now.Month + logName + ".csv", headers);
            }
          
            Logs.Add(logName, string.Empty);
            return logName;
        }

        private string ParseContent(string logName, List<string>content)
        {
            if (content.Count != _headerCount.Where(k => k.Key == logName).FirstOrDefault().Value)
            {
                throw new Exception("Not Enough Columns for Log, Log File:" + logName + " requires " +
                    _headerCount.Where(k => k.Key == logName).FirstOrDefault().Value + " values."
                );
            }

            string logLine = string.Empty;
            foreach(string entry in content)
            {
                logLine = logLine + entry + ",";
            }
            return logLine + DateTime.Now.ToString() + "," + "\r\n";
        }

        #endregion

    }
}
