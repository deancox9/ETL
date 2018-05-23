using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MGRE.ETL.Service
{
    [ServiceContract]
    public interface IImportService
    {
        [OperationContract]
        void RunImportNowByName(string importName);

        [OperationContract]
        void RunImportNowByGUID(Guid importGUID);

        //[OperationContract]
        //void WriteToLog(Queue<LogEntry> queue, int incidentId);

    }
}
