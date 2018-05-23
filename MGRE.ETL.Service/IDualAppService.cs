using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using MGRE.ETL.Contracts;

namespace MGRE.ETL.Service
{
    [ServiceContract]
    public interface IDualAppService
    {
        [OperationContract]
        DualAppShortcutSetting GetSettings(int appID);

        [OperationContract]
        List<vw_FundsAndProperties> GetFundsAndProperties();
    }
}
