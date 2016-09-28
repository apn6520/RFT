using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using TransferDesk.Contracts.Logging;
using System.Diagnostics;
using System.Text;

//Lazy instantiation is implemented for filelogger instance
//fileLogger variable follows a singleton instance as it is static
//interfaces for logger follow interface segeration principle
//each logger class e.g filelogger follows single responsibility principle
//to make the class thread safe, synchroziation is done with a lock object as static to persist locks for all calls to proc. 
//a Object factory method to get a logger type as per the target type.
//helper class works on the adapter pattern 
//Stopwatch property added to helper class to check performance while writing to log, commented due to mutliple threads
//each date will have a new log file in same folder
//plus filename suffix can create seperate file for a module.
//File size limited to 1000 lines per file
//helper can have extension methods for logger classes so it is extensible to change but core will be with logger classes.
//exception.Tostring will show all inner exceptions if not null


namespace TransferDesk.Logger
{
    public class LogHelper : IFileLogger
    {
        private static FileLogger _fileLogger = null; //a singleton pattern

        //interface defined below
        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileNameSuffix { get; set; }

        //extra properties below
        public LogTarget LogTarget { get; set; }

        public LogHelper()
        {
            LogTarget = LogTarget.File; //default

        }

        public void Dispose()
        {
            _fileLogger.Dispose();
        }

        public void WriteStringBuilderToUserLogAndClear(StringBuilder stringBuilder, string userId = null)
        {
            ILogger logger = GetLogger();
            IFileLogger fileLogger = logger as IFileLogger;
            fileLogger.FilePath = this.FilePath;
            fileLogger.FileName = this.FileName;
            fileLogger.WriteStringBuilderToUserLogAndClear(stringBuilder, userId);
        }

        public void WriteStringBuilderToAppLogAndClear(StringBuilder stringBuilder)
        {
            ILogger logger = GetLogger();
            IFileLogger fileLogger = logger as IFileLogger;
            fileLogger.FilePath = this.FilePath;
            fileLogger.FileName = this.FileName;
            fileLogger.WriteStringBuilderToAppLogAndClear(stringBuilder);
        }

        public void ApplicationLog(string message)
        {
            switch (LogTarget)
            {
                case LogTarget.File:
                    ILogger logger = GetLogger();
                    IFileLogger fileLogger = logger as IFileLogger;
                    fileLogger.FilePath = this.FilePath;
                    fileLogger.FileName = this.FileName;
                    fileLogger.ApplicationLog(message);

                    break;
            }

        }

        public void Log(string userId, string message)
        {
            switch (LogTarget)
            {
                case LogTarget.File:
                    ILogger logger = GetLogger();
                    IFileLogger fileLogger = logger as IFileLogger;
                    fileLogger.FilePath = this.FilePath;
                    fileLogger.FileName = this.FileName;
                    fileLogger.Log(userId, message);

                    break;
            }

        }

        public void LogException(Exception exception, StringBuilder stringBuilder)
        {
            if (stringBuilder == null)
            {
                _LogException(exception, null);
            }
            else
            {
                _LogException(exception, stringBuilder);
            }
        }

        public void LogException(Exception exception)
        {
            _LogException(exception, null);

        }
        private void _LogException(Exception exception, StringBuilder stringBuilder)
        {
            switch (LogTarget)
            {

                case LogTarget.File:

                    ILogger logger = GetLogger();
                    IFileLogger fileLogger = logger as IFileLogger;
                    fileLogger.FilePath = this.FilePath;
                    fileLogger.FileName = this.FileName;
                    fileLogger.LogException(exception, stringBuilder);

                    break;


            }

        }

        public void ApplicationExceptionLog(Exception exception, StringBuilder stringBuilder)
        {
            switch (LogTarget)
            {

                case LogTarget.File:

                    ILogger logger = GetLogger();
                    IFileLogger fileLogger = logger as IFileLogger;
                    fileLogger.FilePath = this.FilePath;
                    fileLogger.FileName = this.FileName;
                    fileLogger.LogException(exception, stringBuilder);

                    break;


            }

        }

        private ILogger GetLogger() //factory method to return objects
        {
            switch (LogTarget)
            {
                case LogTarget.File:
                    if (_fileLogger == null)
                    {
                        _fileLogger = new FileLogger();

                    }
                    return _fileLogger;
            }
            return null;
        }

    }

    public class FileLogger : IFileLogger
    {
        private static readonly object LockObj = new object();

        protected static Stopwatch PerformanceStopWatch;


        public string FileName { get; set; }

        public string FileNameSuffix { get; set; }

        public string FilePath { get; set; }

        public long LogLineCounter { get; private set; }

        public int FileNameSuffixCounter { get; private set; }

        public int LogLineLimitCount { get; set; }

        public FileLogger()
        {
            LogLineCounter = 1;
            LogLineLimitCount = 1000;
            //this will help control file size limit 1 character = 1 byte appox. each line 100 char average
            PerformanceStopWatch = new Stopwatch();
            PerformanceStopWatch.Start();

        }


        public virtual void Dispose()
        {
            //dispose here
            if (PerformanceStopWatch.IsRunning)
                PerformanceStopWatch.Stop();
        }


        public void Log(string userId, string message)
        {
            try
            {
                WriteToDiskFile(message, userId);

            }
            catch (Exception loggerException)
            {
                TryWriteForLoggerException(loggerException, message);
            }

        }

        public void ApplicationLog(string message)
        {
            try
            {
                WriteToDiskFile(message);

            }
            catch (Exception loggerException)
            {
                TryWriteForLoggerException(loggerException, message);
            }

        }

        private void WriteToDiskFile(string message, string userId = "")
        {
            //this check can be eleminated for non windows logon outside interanet
            if (userId.Length >= 15 || userId.Contains(" "))
            {
                throw new Exception("Invalid UserID length greater than 9 characters or contains space in windows user Id ");
            }

            if (userId.Trim() == "") { userId = "App"; } // app as a logger File name user description

            lock (LockObj)
            {
                using (StreamWriter streamWriter = new StreamWriter(FilePath + FileName + FileNameSuffix
                    + "_" + DateTime.Today.Day + "_" + DateTime.Today.Month + "_" + DateTime.Today.Year + "_" + FileNameSuffixCounter + "_" + userId + "_" + "log.txt", true))
                {
                    string timeStamp = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff");

                    streamWriter.WriteLine(LogLineCounter + " " +
                        timeStamp + " " +
                        PerformanceStopWatch.ElapsedMilliseconds + " " + message);

                    streamWriter.Close();

                }

                PerformanceStopWatch.Restart();

            }//lock ends

            LogLineCounter += 1;

            //check and file split not required as app and each user log will be now seperate
            //if (LogLineCounter > LogLineLimitCount)
            //{
            //    FileNameSuffixCounter += 1;
            //}

        }

        public void WriteStringBuilderToUserLogAndClear(StringBuilder stringBuilder, string userId = null)
        {
            try
            {
                if (userId == null || userId.Trim() == "")
                {
                    if (@System.Web.HttpContext.Current != null && @System.Web.HttpContext.Current.User != null)
                    {
                        userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
                    }
                }

                WriteToDiskFile(stringBuilder.ToString(), userId);
            }
            catch (Exception loggerException)
            {
                TryWriteForLoggerException(loggerException, stringBuilder.ToString());
            }
            finally
            {
                stringBuilder.Clear();

            }

        }

        public void WriteStringBuilderToAppLogAndClear(StringBuilder stringBuilder)
        {
            try
            {
                WriteToDiskFile(stringBuilder.ToString());
            }
            catch (Exception loggerException)
            {
                TryWriteForLoggerException(loggerException, stringBuilder.ToString());
            }
            finally
            {
                stringBuilder.Clear();

            }

        }

        public void LogException(Exception exception, StringBuilder stringBuilder = null)
        {
            var userId = "";
            string message = null;
            try
            {
                if (@System.Web.HttpContext.Current != null && @System.Web.HttpContext.Current.User != null)
                {
                    userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
                }

                if (stringBuilder != null)
                {
                    //write Stringbuilder to disk and clear
                    WriteStringBuilderToAppLogAndClear(stringBuilder);
                }
                //exception.tostring will include all inner exception details

                message = "userID : " + userId + " exception : " + exception.ToString();

                WriteToDiskFile(message, userId);

                //throw new Exception("test logger exception");
            }
            catch (Exception loggerException)
            {
                TryWriteForLoggerException(loggerException, message, userId);
            }

        }


        public void ApplicationExceptionLog(Exception exception, StringBuilder stringBuilder)
        {
            var userId = "";
            string message = null;
            try
            {
                //write Stringbuilder to disk and clear
                WriteStringBuilderToAppLogAndClear(stringBuilder);

                //exception.tostring will include all inner exception details

                message = "exception : " + exception.ToString();

                WriteToDiskFile(message, userId);

                //throw new Exception("test logger exception");
            }
            catch (Exception loggerException)
            {
                TryWriteForLoggerException(loggerException, message, userId);
            }

        }


        public void TryWriteForLoggerException(Exception loggerException, string message, string userId = "")
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(FilePath + FileName + FileNameSuffix
                  + "_" + DateTime.Today.Day + "_" + DateTime.Today.Month + "_" + DateTime.Today.Year + "_" + FileNameSuffixCounter + "_" + userId + "_Loggerexception_" + "log.txt", true))
                {
                    string timeStamp = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff");

                    streamWriter.WriteLine(LogLineCounter + " " +
                        timeStamp + " " +
                        PerformanceStopWatch.ElapsedMilliseconds + " " + "message that was being written when exception occured --" + message);

                    streamWriter.WriteLine(LogLineCounter + " " +
                        timeStamp + " " +
                        PerformanceStopWatch.ElapsedMilliseconds + " " + loggerException.ToString());

                    streamWriter.Close();

                }
            }
            catch (Exception exception)
            {
                Trace.TraceWarning("TransferDesk unable to write log due to " + exception.ToString());
            }

        }

    }


}

