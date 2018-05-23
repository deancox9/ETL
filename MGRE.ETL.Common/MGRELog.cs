using System;
using System.Collections;
using System.Text;
using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

using System.Data;

namespace MGRE.ETL.Common
{
    #region .Net Class Documentation
    /// <summary>
    /// Static class to writing to db log. Holds log entries in a queue until an entry with the configured logging tracelevel is reached at which
    /// point the abstract method CommitToStore is called for either the client or server to send entries to db. Typically the server
    /// will call the logging enterprise library to log the entries and the client will send the entries via a web service to do the same.
    /// 
    /// </summary>
    /// <remarks> </remarks> 
    /// <history>
    ///	  <revision by="barryc" ref="24063" date="2012-08-10">copied from ewatch</revision>
    /// </history>
    #endregion
    public static class MGRELog
    {
        public const string IncidentKey = "IncidentId";
        public const string IncidentKeyFormat = "0000000";
        public delegate void CommitToStoreHandler(System.Collections.Generic.Queue<LogEntry> traceQueue, int incidentId);
        public static CommitToStoreHandler CommitToStore;

        private static TraceLevel loggingLevel;
        private static System.Collections.Generic.Queue<LogEntry> traceQueue;
        private static int traceQueueSize;
        private static int incidentId;

        /// <summary>
        /// initialises settings from config on first use
        /// </summary>
        static MGRELog()
        {
            loggingLevel = AppSettingsWrapper.tracelLevelSetting.Value;
            traceQueueSize = AppSettingsWrapper.tracelQueueSizeSetting.Value;

            if (traceQueueSize > 0)
            {
                traceQueue = new System.Collections.Generic.Queue<LogEntry>(traceQueueSize);
            }

            incidentId = new Random().Next(100, 1000000);
        }

        /// <summary>
        ///     Format and write an exception to the log. 
        /// </summary>
        /// <param name="ex"></param>
        public static void Write(Exception ex)
        {

            if (ex.Data.Contains(IncidentKey))
            {
                //exception has already been logged further up the chain so do not write again.
                return;
            }

            string message = ex.Message;
            ex.Data.Add(IncidentKey, incidentId);//add an incidentId to the exception so it can be identified if logged by more than 1 handler

            try
            {
                System.IO.StringWriter str = new System.IO.StringWriter();
                XmlExceptionFormatter xmlFormat = new XmlExceptionFormatter(str, ex);
                xmlFormat.Format();
                message += str.ToString();

                str.Close();
            }
            catch (Exception formattingEx)
            {
                message += "Couldn't format exception: " + formattingEx.Message;
            }

            MGRELog.Write(message, getCallingMethodName(), TraceLevel.Error);

            incidentId++;
        }

        /// <summary>
        /// Write an info message
        /// </summary>
        public static void Write(string methodName, string message)
        {
            Write(message, methodName, TraceLevel.Info);
        }

        /// <summary>
        /// Write an info message
        /// </summary>
        /// <param name="message"></param>
        public static void Write(string message)
        {
            //dont automatically add Method name for info messages (saves traversing stack)
            Write(message, "", TraceLevel.Info);
        }


        public static void WriteValidationResult(ValidationResult res)
        {
            WriteValidationResult(getCallingMethodName(), res);
        }
        /// <summary>
        /// Writes the result of validation as an info message
        /// </summary>
        public static void WriteValidationResult(string methodName, ValidationResult res)
        {
            if (res.HasErrors || res.HasWarnings)
            {
                string message = res.WriteErrors() + res.WriteWarnings();
                Write(message, "", TraceLevel.Warning);
            }
            else
            {
                Write("Validation passed", methodName, TraceLevel.Warning);
            }    
        }

        /// <summary>
        /// Write a warning message
        /// </summary>
        public static void WriteWarning(string methodName, string message)
        {
            Write(message, methodName, TraceLevel.Warning);
        }

        /// <summary>
        /// Write a warning message
        /// </summary>
        /// <param name="message"></param>
        public static void WriteWarning(string message)
        {
            Write(message, getCallingMethodName(),  TraceLevel.Warning);
        }

        /// <summary>
        /// Write "Method start" message. Includes method name
        /// </summary>
        public static void WriteMethodStart(string methodName)
        {
            Write("Method start", methodName, TraceLevel.Info);
        }

        /// <summary>
        /// Write "Method start" message. Includes method name
        /// </summary>
        public static void WriteMethodStart()
        {
            Write("Method start", getCallingMethodName(), TraceLevel.Info);
        }

        /// <summary>
        /// Write a method return value info message. INcludes Method name
        /// </summary>
        /// <param name="returnValue"></param>
        public static void WriteMethodReturn(object returnValue)
        {
            Write("Method Return: " + returnValue.ToString(), getCallingMethodName(), TraceLevel.Info);
        }

        /// <summary>
        /// Write a method return value info message. INcludes Method name
        /// </summary>
        public static void WriteMethodReturn(string methodName,object returnValue)
        {
            Write("Method Return: " + returnValue.ToString(),methodName, TraceLevel.Info);
        }

        /// <summary>
        ///     Write a method success info message
        /// </summary>
        public static void WriteMethodSuccess(string methodName)
        {
            Write("Method Success", methodName, TraceLevel.Info);
        }

        /// <summary>
        ///     Write a method success info message
        /// </summary>
        public static void WriteMethodSuccess()
        {
            Write("Method Success", getCallingMethodName(), TraceLevel.Info);
        }

        /// <summary>
        /// Writes a command text and parameters info message
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        public static void WriteDBCommand(string spName, params object[] parameters)
        {
            string logMessage = "";

            foreach (object obj in parameters)
            {
                if (obj == null)
                {
                    logMessage += (logMessage == "" ? "" : ",") + "Null";
                }
                else
                {
                    logMessage += (logMessage == "" ? "" : ",") + obj.ToString();
                }
            }

            logMessage = "Calling " + spName + " " + logMessage;

            Write(logMessage);
        }

        /// <summary>
        ///     writes a command text and parameters info message
        /// </summary>
        /// <param name="cmd"></param>
        public static void WriteDBCommand(IDbCommand cmd)
        {
            string logMessage = "";

            foreach (IDataParameter param in cmd.Parameters)
            {
                if (param.Value == null)
                {
                    logMessage += (logMessage == "" ? "" : ",") + "Null";
                }
                else
                {
                    logMessage += (logMessage == "" ? "" : ",") + param.Value.ToString();
                }
            }

            logMessage = "Calling " + cmd.CommandText + " " + logMessage;

            Write(logMessage);
        }
                
        /// <summary>
        /// Writes the dataset table names, row counts and csv first row as an info message
        /// </summary>
        /// <param name="ds"></param>
        public static void WriteDataSet(DataSet ds)
        {
            string logMessage = "DataSetName:" + ds.DataSetName + Environment.NewLine;

            foreach (DataTable tbl in ds.Tables)
            {
                logMessage += tbl.TableName + " rows:" + tbl.Rows.Count.ToString() + Environment.NewLine;

                if (tbl.Rows.Count > 0)
                {
                    logMessage += "Row 1: ";
                    foreach (DataColumn col in tbl.Columns)
                    {
                        logMessage += tbl.Rows[0][col].ToString() + ",";
                    }
                }
            }

            Write(logMessage);
        }



        /// <summary>
        /// Write the supplied message to the queue. Will trigger WriteToStore if message level matches configured logging level
        /// </summary>
        /// <param name="message">Trace message</param>
        /// <param name="callingMethodName">Name of the calling method</param>
        /// <param name="messageLevel">Require tracing level</param>
        public static void Write(string message,  string callingMethodName, TraceLevel messageLevel)
        {
            try
            {

                // Stop immediately if logging is disabled.
                if (loggingLevel == TraceLevel.Off) return;

                LogEntry logEntry = new LogEntry();
                logEntry.Message = message;

                // Store the calling method's name as an extended property
                logEntry.ExtendedProperties.Add("MethodName", callingMethodName);
                logEntry.Category = messageLevel.ToString();//category in log table, also determines log distribution

                // Translate from TraceLevel to Severity
                switch (messageLevel)
                {
                    case TraceLevel.Error:
                        logEntry.Severity = Severity.Error;
                        break;
                    case TraceLevel.Warning:
                        logEntry.Severity = Severity.Warning;
                        break;
                    default:
                        logEntry.Severity = Severity.Information;
                        break;
                }

                // Check whether the supplied message level is currently being logged.
                if (messageLevel <= loggingLevel)
                {

                    //write log entries from the queue first, then the current entry
                    if (traceQueueSize > 0)
                    {
                        traceQueue.Enqueue(logEntry);

                        if (CommitToStore != null)
                        {
                            CommitToStore(traceQueue, incidentId);
                            traceQueue.Clear(); 
                        }
                    }
                    // Writes the log entry.
                }
                else
                {//keep message in queue and only write if there is a logged message with a severity matching the configured traceLevel
                    if (traceQueueSize > 0)
                    {
                        if (traceQueue.Count >= traceQueueSize)
                        {
                            traceQueue.Dequeue();
                        }

                        traceQueue.Enqueue(logEntry);
                    }
                }
            }
            catch
            {
                loggingLevel = TraceLevel.Off;
                throw;
            }
        }

        public static MGREException GetDerivedException(Exception sourceException, string newErrorMessage)
        {
            MGREException appEx = new MGREException(newErrorMessage);

            if (sourceException.Data.Contains(IncidentKey))
            {
                object id = sourceException.Data[IncidentKey];
                appEx.Data.Add(IncidentKey, (int)id);
            }

            return appEx;

        }

        #region Helper Methods

        /// <summary>
        /// Returns the calling methosd name by traversing the stack. Added to each message.
        /// </summary>
        /// <returns></returns>
        private static string getCallingMethodName()
        {
            //copied from log class
            string result = "Method Unknown";

            try
            {
                StackTrace stackTrace = new StackTrace();

                for (int i = 0; i < stackTrace.FrameCount; i++)
                {
                    StackFrame stackFrame = stackTrace.GetFrame(i);
                    System.Reflection.MethodBase methodBase = stackFrame.GetMethod();
                    if (methodBase.ReflectedType != typeof(MGRELog) && methodBase.Name.IndexOf("HandleException") < 0)
                    {
                        result = methodBase.ReflectedType.ToString() + "::" + methodBase.Name;
                        break;
                    }
                }
            }
            catch
            {
                result = "getCallingMethodName.Error";
            }

            return result.Replace("MGRE.Reporting.", "");
        }
        #endregion
    }


}