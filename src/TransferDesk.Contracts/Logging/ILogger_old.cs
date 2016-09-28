using System;
using System.Collections.Generic;
using System.Text;

namespace TransferDesk.Contracts.Logging
{
    public enum LogTarget
    {

        File, Database, EventLog

    }

    /// <summary>
    /// Defines the common logging interface, implementations can call wrappers with provided interface
    /// </summary>
    public interface ILogger:IDisposable
    {

        /// <summary>
        /// Writes a exception message to the user log
        /// </summary>
        /// <param name="exception">user exception to write</param>
        /// <param name="stringbuilder">write pending user writes</param> 
        void LogException(Exception exception, StringBuilder stringbuilder=null);
        
        void Log(string message, string userId=null);

        void WriteStringBuilderToLogAndClear(StringBuilder stringBuilder, string userId = ""); 

        //void LogException(Exception exception, StringBuilder stringbuilder);

        //void LogException(Exception exception);

        //void Log(string message, string userId);

        void WriteStringBuilderToUserLogAndClear(StringBuilder stringBuilder, string userIdForUserLog = "");
         
    }

    public interface IApplicationLog:ILogger
    {
        /// <summary>
        /// Writes a application exception message to the log
        /// </summary>
        /// <param name="exception">application exception to write</param>
        /// <param name="stringbuilder">write pending application writes</param>
        void ApplicationExceptionLog(Exception exception, StringBuilder stringbuilder);

        void ApplicationLog(string message);
 
        void WriteStringBuilderToAppLogAndClear(StringBuilder stringBuilder); 
    }

    /// <summary>
    /// Defines the File logging interface
    /// Following the interface segration principle, inherited from a base interface 
    /// </summary>
    public interface IFileLogger:IApplicationLog
    {
        /// <summary>
        /// Writes a message to the file log
        /// </summary>
     
        string FileName { get; set; }
        string FilePath { get; set; }
        string FileNameSuffix { get; set; }

        //long LogLineCounter { get; set; }

        

    }

  




}
