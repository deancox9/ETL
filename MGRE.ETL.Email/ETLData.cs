using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using MGRE.ETL.Application.Data;
using MGRE.ETL.Common;


namespace MGRE.ETL.Email
{
    public class ETLData : BaseData
    {
        #region Constructors
        public ETLData(string applicationDatabase)
            : base(applicationDatabase)
        {
            MGRELog.Write("Connected to " + applicationDatabase + " database", "HubData.Constructor");

        }

        #endregion Constructors

        public DataSet GetETLData(string storedProc, string tableName)
        {
            DataSet ds = new DataSet();

            base.LoadDataSet(storedProc, ds, new string[] { tableName });

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else
            {
                return null;
            }
        }

        public int UpdateETLDataStatus(string storedProc, int status)
        {
            return base.ExecuteStoredProcAsNonQuery(storedProc, status);
        }
    }
}
