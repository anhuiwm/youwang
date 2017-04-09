using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace mSeed.Common.mLogger
{
    public enum logLevel
    {
        Critical = 0,
        Error,
        Warn,
        Info,
        Debug,
        All,
        MaxLog
    }

    /// <summary>
    /// Represents a log message
    /// </summary>
    public class LoggerMessage
    {
        public static void setLog(string e)
        {
            mSeed.Common.mLogger.mLogger.Debug(string.Format(e, "tb log"));
            mSeed.Common.mLogger.mLogger.GetLoggerInstance().FlushLog();
        }

        public logLevel level;
        public string tag;
        public string message;
        public long time;

        public LoggerMessage()
        {
            level = logLevel.Debug;
            tag = message = string.Empty;
            time = System.DateTime.Now.ToFileTime();
        }

        public LoggerMessage(logLevel setLevel, string setTag, string setMessage)
        {
            level = setLevel;
            tag = setTag;
            message = setMessage;
            time = System.DateTime.Now.ToFileTime();
        }

        override public string ToString()
        {
            return string.Format("[{0}] [{1} ({2}) : {3}]", System.DateTime.FromFileTime(time).ToString("yyyy_MM_dd HH:mm:ss.fff"), level, tag, message);
        }
    }
    
    public class mLogger
    {
        // singleton SystemConfig instance
        public static mLogger _logger = new mLogger();
        private DateTime logStartTime;

        /// <summary> get _logger Instance </summary>
        public static mLogger GetLoggerInstance()
        {
            return _logger;
        }

        private string logfilename = "log";
        public string LogfileName
        {
            get { return logfilename; }
            set
            {
                    logfilename = value;
                    newfileOpen();
            }
        }
        
        StreamWriter streamWriter = null;
        int _logSize = 0;
        int _logLine = 0;
        int _logFileCount = 0;

        private static int logMaxSize = 1024 * 1024 * 10; // 10Mb
        private static int logMaxLine = 1024 * 128;


        private mLogger()
        {
            _logSize = _logLine = _logFileCount = 0;
        }

        ~mLogger()
        {
            try
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter.Dispose();
                    streamWriter = null;
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        public void FlushLog()
        {
            //streamWriter.Flush();
            bool bRotate = ((DateTime.Now - logStartTime).Days != 0);
            if (_logSize > logMaxSize || _logLine > logMaxLine || bRotate)
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter.Dispose();
                    streamWriter = null;
                    newfileOpen();
                }
                _logSize = 0;
                _logLine = 0;

                if (bRotate)
                    _logFileCount = 0;
            }
        }

        private string setlogfile()
        {
            string PhysicPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            string setLogFileName = string.Format("{0}_{1}_{2}.txt", logfilename, DateTime.Now.ToString("yyyyMMdd"), _logFileCount);
            setLogFileName = System.IO.Path.Combine(PhysicPath, setLogFileName);
            return setLogFileName;
        }

        public void log(string message, string tag = "LOG", logLevel level = logLevel.Info)
        {
            LoggerMessage lm = new LoggerMessage(level, tag, message);
            mSeed.Common.BackgroundWorker.BackgroundTaskRunner.FireAndForgetTask(() => writelog(lm.ToString()));
        }

        private void newfileOpen()
        {
            if (string.IsNullOrEmpty(logfilename))
                throw new FileNotFoundException("file name empty");

            try
            {
                _logFileCount++;
                string fileName = setlogfile();
                while (File.Exists(fileName))
                {
                    _logFileCount++;
                    fileName = setlogfile();
                }

                streamWriter = new StreamWriter(fileName);
                streamWriter.AutoFlush = true;
                logStartTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed to create log file : {0} , call stack : {1}", ex.Message, ex.StackTrace));
                //throw new Exception("Failed to create log file: " + ex.Message);
            }

        }

        private void writelog(string setLog)
        {
            if (streamWriter == null)
                newfileOpen();

            if (streamWriter != null)
            {
                streamWriter.WriteLine(setLog);
                _logSize += setLog.Length;
                _logLine++;
            }
        }

        public static void Debug(string msg, string tag = "")
        {
            GetLoggerInstance().log(msg, tag, logLevel.Debug);
        }
        public static void Info(string msg, string tag = "")
        {
            GetLoggerInstance().log(msg, tag, logLevel.Info);
        }
        public static void Warning(string msg, string tag = "")
        {
            GetLoggerInstance().log(msg, tag, logLevel.Warn);
        }
        public static void Error(string msg, string tag = "")
        {
            GetLoggerInstance().log(msg, tag, logLevel.Error);
        }
        public static void Critical(string msg, string tag = "")
        {
            GetLoggerInstance().log(msg, tag, logLevel.Critical);
        }
        public static void Log(logLevel setLevel, string msg, string tag = "")
        {
            GetLoggerInstance().log(msg, tag, setLevel);
        }
    }
}
