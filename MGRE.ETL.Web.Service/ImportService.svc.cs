using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using MGRE.ETL.Service;
using MGRE.ETL.Business.Rules;
using MGRE.ETL.Contracts;
using MGRE.ETL.Common;

using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MGRE.ETL.Web.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ImportService" in code, svc and config file together.
    public class ImportService : IImportService
    {
        public void RunImportNowByName(string importName)
        {
            try
            {
                MGRE.ETL.Data.Contracts.IETLDAL etlDAL = new Data.ETLData();

                ETLImportBO bo = new ETLImportBO(etlDAL);

                bo.RunImportNow(importName, System.ServiceModel.ServiceSecurityContext.Current.WindowsIdentity.Name);
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

        public void RunImportNowByGUID(Guid importGUID)
        {
            try
            {
                MGRE.ETL.Data.Contracts.IETLDAL etlDAL = new Data.ETLData();

                ETLImportBO bo = new ETLImportBO(etlDAL);

                bo.RunImportNow(importGUID, System.ServiceModel.ServiceSecurityContext.Current.WindowsIdentity.Name);
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
