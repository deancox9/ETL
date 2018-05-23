using System;
using System.Collections;
using System.Text;
using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

using MGRE.ETL.Common;

namespace MGRE.ETL.Web.Service
{
    public static class MGREServerLog
    {
        static MGREServerLog()
        {
            MGRELog.CommitToStore += new MGRELog.CommitToStoreHandler(CommitToStore);
        }

        public static void AttachToLoggingHandler()
        {
            //nothing, just load static object if nt already loaded
        }

        public static void CommitToStore(System.Collections.Generic.Queue<LogEntry> traceQueue, int incidentId)
        {
            LogEntry[] queueToWrite = traceQueue.ToArray();

            traceQueue.Clear();

            foreach (LogEntry traceEntry in queueToWrite)
            {
                traceEntry.Win32ThreadId = incidentId.ToString();

                if (traceEntry.Message.Length > 20000)
                {
                    traceEntry.Message = traceEntry.Message.Substring(0, 20000);
                }

                Logger.Write(traceEntry);
            }
        }
    }
}