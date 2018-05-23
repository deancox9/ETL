using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MGRE.ETL.Service
{
    [ServiceContract]
    public interface IExportService
    {
        [OperationContract]
        void RunExportNowByName(string exportName);

        [OperationContract]
        void RunExportNowByGUID(Guid exportGUID);

        //[OperationContract]
        //void WriteToLog(Queue<LogEntry> queue, int incidentId);
    }
}
