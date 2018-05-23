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

namespace MGRE.ETL.Import
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

        public DataSet GetTableDataSet(string tableName, string procName)
        {
            DataSet ds = new DataSet();

            base.LoadDataSet(procName, ds, new string[] { tableName });

            return ds;
        }


        private DbType GetDBType(Type type)
        {
            switch (type.Name)
            {
                case "STRING":
                    return DbType.String;
                case "DATETIME":
                    return DbType.DateTime;
                case "INT32":
                    return DbType.Int32;
                case "DECIMAL":
                    return DbType.Decimal;
            }
            return DbType.String;
        }

        public int InsertImportData(DataSet ds, string tableName, string procName)
        {
            int rowsAffected = 0;

            #region Command Wrappers

            DBCommandWrapper inCommandWrapper = db.GetStoredProcCommandWrapper(procName);
            inCommandWrapper.Command.UpdatedRowSource = UpdateRowSource.None;

            foreach (DataColumn col in ds.Tables[tableName].Columns)
            {
                inCommandWrapper.AddInParameter("@" + col.ColumnName, GetDBType(col.DataType), col.ColumnName, DataRowVersion.Current);
            }
            #endregion

            try
            {
                //using (TransactionScope scope = new TransactionScope())
                //{

                    // Submit the DataSet, capturing the number of rows that were affected.  
                    // Note that UpdateBehavior must be set as Standard.  This will
                    // ensure that all updates are rolled back on failure.
                    rowsAffected = db.UpdateDataSet(ds,
                        tableName,
                        inCommandWrapper,
                        null,
                        null,
                        UpdateBehavior.Standard);

                //    scope.Complete();

                //}
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

        public void DeleteData(string tableName, string procName)
        {
 
            #region Command Wrappers

            DBCommandWrapper upCommandWrapper = db.GetStoredProcCommandWrapper(procName);
            upCommandWrapper.Command.UpdatedRowSource = UpdateRowSource.None;

            #endregion

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(upCommandWrapper);

                    scope.Complete();

                }
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
        }

        public int RecordHistory(Guid importGUID, string userName, DateTime startDateTime, bool success)
        {
            int rowsAffected = 0;

            try
            {
                    // Submit the DataSet, capturing the number of rows that were affected.  
                    // Note that UpdateBehavior must be set as Standard.  This will
                    // ensure that all updates are rolled back on failure.
                    rowsAffected = db.ExecuteNonQuery("usp_ETL_InsertImportHistory", importGUID, startDateTime, userName, DateTime.Now, (success ? 1 : 0));

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
