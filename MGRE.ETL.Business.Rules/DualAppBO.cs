using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGRE.ETL.Contracts;
using MGRE.ETL.Data.Contracts;
using MGRE.ETL.Common;

namespace MGRE.ETL.Business.Rules
{
    public class DualAppBO
    {
        private IETLDAL etlDAL;  //interface which defines which data methods are required

        public DualAppBO(IETLDAL etlDAL)
        {
            this.etlDAL = etlDAL;
        }



        public DualAppShortcutSetting GetSettings(int appID)
        {
            if (appID == 0)
            {
                throw new ArgumentNullException("appID");
            }

            //Retrieve Settings
            DualAppShortcutSetting settings = etlDAL.GetDualAppSettings(appID);

            if (settings == null)
            {
                throw new Exception("Settings does not exist for application - " + appID.ToString());
            }

            return settings;
        }

        public List<vw_FundsAndProperties> GetFundsAndProperties()
        {
            //Retrieve List of Funds and Properties
            List<vw_FundsAndProperties> ents = etlDAL.GetFundsAndProperties();

            return ents;
        }
    }
}
