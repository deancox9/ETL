using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Linq;
using System.Text;

using MGRE.ETL.Application.Data;
using MGRE.ETL.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace MGRE.ETL.Export
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


            // Always create a file even if there is no data
            return ds;

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    return ds;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public int UpdateETLDataStatus(string storedProc, int status)
        {
            return base.ExecuteStoredProcAsNonQuery(storedProc, status);
        }

        public int RecordHistory(Guid exportGUID, string userName, DateTime startDateTime, bool success)
        {
            int rowsAffected = 0;

            try
            {
                    // Submit the DataSet, capturing the number of rows that were affected.  
                    // Note that UpdateBehavior must be set as Standard.  This will
                    // ensure that all updates are rolled back on failure.
                rowsAffected = db.ExecuteNonQuery("usp_ETL_InsertExportHistory", exportGUID, startDateTime, userName, DateTime.Now, (success ? 1 : 0));

            }
            catch (DBConcurrencyException dbcEx)
            {
                bool rethrow = ExceptionPolicy.HandleException(dbcEx, "Data Logic Exception Policy");
                if (rethrow) throw;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Data Exception Policy");
                if (rethrow) throw;
            }

            return rowsAffected;

        }
    }
}
