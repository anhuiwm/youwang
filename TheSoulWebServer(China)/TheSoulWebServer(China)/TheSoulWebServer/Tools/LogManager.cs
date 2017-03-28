using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace TheSoulWebServer.Tools
{
    public class LogManager_Old
    {
        private static LogManager_Old m_Instance;
        private static object m_lock = new object();
        private string checkLogOnOff = TheSoulDBcon.GetInstance().GetLogOnOff();
        private string checkURL = TheSoulDBcon.GetInstance().GetLogDirectory() + "prameterlog.txt";
        private readonly RollOverLogText _fileLog;

        public LogManager_Old()
        {
            _fileLog = new RollOverLogText(checkURL);
        }

        public static LogManager_Old Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (m_Instance == null)
                    {
                        m_Instance = new LogManager_Old();
                    }

                    return m_Instance;
                }
            }
        }

        public void WriteLogMessage(string logMessage)
        {
            return;
            //if (checkLogOnOff == "On")
            //{
            //    if (Environment.UserInteractive)
            //    {
            //        //Console.WriteLine(logMessage);
            //        _fileLog.WriteLine(logMessage);
            //    }
            //    else
            //    {
            //        _fileLog.WriteLine(logMessage);
            //    }
            //}
        }
        public void WriteLogMessage(string logMessage, params object[] param)
        {
            return; 
            //if (checkLogOnOff == "On")
            //{
            //    string logMsg = string.Format(logMessage, param);
            //    if (Environment.UserInteractive)
            //    {
            //        //Console.WriteLine(logMsg);
            //        _fileLog.WriteLine(logMsg);
            //    }
            //    else
            //    {
            //        _fileLog.WriteLine(logMsg);
            //    }
            //}
        }
    }

    public class ErrorLogManager
    {
        private static ErrorLogManager m_Instance;
        private static object m_lock = new object();
        private string checkLogOnOff = TheSoulDBcon.GetInstance().GetLogOnOff();
        private string checkURL = TheSoulDBcon.GetInstance().GetLogDirectory() + "errorlog.txt";
        private readonly RollOverLogText _fileLog;

        public ErrorLogManager()
        {
            _fileLog = new RollOverLogText(checkURL);
        }

        public static ErrorLogManager Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (m_Instance == null)
                    {
                        m_Instance = new ErrorLogManager();
                    }

                    return m_Instance;
                }
            }
        }

        public void WriteLogMessage(string logMessage)
        {
            return; 
            //if (checkLogOnOff == "On")
            //{
            //    if (Environment.UserInteractive)
            //    {
            //        //Console.WriteLine(logMessage);
            //        _fileLog.WriteLine(logMessage);
            //    }
            //    else
            //    {
            //        _fileLog.WriteLine(logMessage);
            //    }
            //}
        }
        public void WriteLogMessage(string logMessage, params object[] param)
        {
            return; 
            //if (checkLogOnOff == "On")
            //{
            //    string logMsg = string.Format(logMessage, param);
            //    if (Environment.UserInteractive)
            //    {
            //        //Console.WriteLine(logMsg);
            //        _fileLog.WriteLine(logMsg);
            //    }
            //    else
            //    {
            //        _fileLog.WriteLine(logMsg);
            //    }
            //}
        }
    }

    public class RollOverLogText : TraceListener
    {
        private readonly string _logFileName = null;

        private DateTime _dateTimeNow;

        //private StreamWriter _fileStreamWriter = null;

        public RollOverLogText(string fileName)
        {
            return;
            //_logFileName = fileName;
            //_fileStreamWriter = new StreamWriter(GenerateFileName(), true)
            //{
            //    AutoFlush = true
            //};
        }

        public override void Write(string message)
        {
            return;
            //CheckRollover();
            //if (_fileStreamWriter.BaseStream.CanWrite)
            //{
            //    _fileStreamWriter.WriteAsync("[" + DateTime.Now + ":" + DateTime.Now.Millisecond + "]\r\n " + message + "");
            //}
        }

        public override void WriteLine(string message)
        {
            return;
            //CheckRollover();
            //if (_fileStreamWriter.BaseStream.CanWrite)
            //{
            //    _fileStreamWriter.WriteLineAsync("[" + DateTime.Now + ":" + DateTime.Now.Millisecond + "]\r\n" + message + "");
            //}
        }

        protected override void Dispose(bool disposing)
        {
            return;
            //if (disposing)
            //{
            //    _fileStreamWriter.Close();
            //}
        }

        private void CheckRollover()
        {
            return;
            //if (_dateTimeNow.CompareTo(DateTime.Today) != 0)
            //{
            //    _fileStreamWriter.Close();
            //    _fileStreamWriter = new StreamWriter(GenerateFileName(), true)
            //    {
            //        AutoFlush = true
            //    };
            //}
        }

        private string GenerateFileName()
        {
            _dateTimeNow = DateTime.Now;

            string directory = Path.GetDirectoryName(_logFileName);

            if (string.IsNullOrEmpty(directory) == false && Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            return Path.Combine(directory + "\\" + Path.GetFileNameWithoutExtension(_logFileName)) + "_" + String.Format("{0:0000}-{1:00}-{2:00}-{3:00}", _dateTimeNow.Year, _dateTimeNow.Month, _dateTimeNow.Day, _dateTimeNow.Hour) + Path.GetExtension(_logFileName);
        }
    }
}