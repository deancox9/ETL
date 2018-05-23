using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using MGRE.ETL.Service;
using MGRE.ETL.Business.Rules;
using MGRE.ETL.Contracts;
using MGRE.ETL.Common;

using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MGRE.ETL.Web.Service
{
    public class ExportService : IExportService
    {

        public void RunExportNowByName(string exportName)
        {
            try
            {
                MGRE.ETL.Data.Contracts.IETLDAL etlDAL = new Data.ETLData();

                ETLExportBO bo = new ETLExportBO(etlDAL);

                bo.RunExportNow(exportName, System.ServiceModel.ServiceSecurityContext.Current.WindowsIdentity.Name);
            }
            catch (MGREException vex)
            {
                MGRELog.Write(vex);
                throw new FaultException<MGREExceptionData>(vex.AppError, new FaultReason(vex.Message));
            }
            catch (Exception ex)
            {
                MGRELog.Write(ex);
                throw;
            }
        }

        public void RunExportNowByGUID(Guid exportGUID)
        {
            try
            {
                MGRE.ETL.Data.Contracts.IETLDAL etlDAL = new Data.ETLData();

                ETLExportBO bo = new ETLExportBO(etlDAL);

                bo.RunExportNow(exportGUID, System.ServiceModel.ServiceSecurityContext.Current.WindowsIdentity.Name);
            }
            catch (MGREException vex)
            {
                MGRELog.Write(vex);
                throw new FaultException<MGREExceptionData>(vex.AppError, new FaultReason(vex.Message));
            }
            catch (Exception ex)
            {
                MGRELog.Write(ex);
                throw;
            }
        }

        //#region Logging function
        ///// <summary>
        ///// Used for client to log client generated messages
        ///// </summary>
        ///// <param name="queue"></param>
        ///// <param name="incidentId"></param>
        //public void WriteToLog(Queue<LogEntry> queue, int incidentId)
        //{
        //    MGREServerLog.CommitToStore(queue, incidentId);
        //}

        //#endregion
    }
}
