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
    public class DualAppService : IDualAppService
    {
        public DualAppShortcutSetting GetSettings(int appID)
        {
            try
            {
                MGRE.ETL.Data.Contracts.IETLDAL etlDAL = new Data.ETLData();

                DualAppBO bo = new DualAppBO(etlDAL);

                return bo.GetSettings(appID);
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

        public List<vw_FundsAndProperties> GetFundsAndProperties()
        {
            try
            {
                MGRE.ETL.Data.Contracts.IETLDAL etlDAL = new Data.ETLData();

                DualAppBO bo = new DualAppBO(etlDAL);

                return bo.GetFundsAndProperties();
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

    }
}
