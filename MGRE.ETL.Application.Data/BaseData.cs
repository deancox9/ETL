using System;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using MGRE.ETL.Common;
using System.Xml;

namespace MGRE.ETL.Application.Data
{
    #region .Net Class Documentation
    /// <summary>
    /// Base data class for all new data access classes
    /// </summary>
    /// <remarks> </remarks> 
    #endregion
    public class BaseData
    {

        protected Database db;

        /// <summary>
        ///     connect to non prime database instance
        /// </summary>
        /// <param name="databaseInstance">non prime database instance</param>
        public BaseData(string databaseInstance)
        {
            db = DatabaseFactory.CreateDatabase(databaseInstance);
        }

        /// <summary>
        ///     connect to default HUB database instance with no transaction
        /// </summary>
        public BaseData()
        {
            db = DatabaseFactory.CreateDatabase("HUB");
        }

        #region enterprise library command functions
        /// <summary>
        /// Execute a given stored proc - with params - as a non-query
        /// Uses PrimeDataAccess transaction if present but does not commit nor rollback
        /// </summary>
        protected int ExecuteStoredProcAsNonQuery(string spName, params object[] parameters)
        {
            int retVal = 0;

            try
            {
                LogCommand(spName, parameters);

                DBCommandWrapper command = db.GetStoredProcCommandWrapper(spName, parameters);

                db.ExecuteNonQuery(command);

                retVal = command.RowsAffected;

                MGRELog.Write("Rows Affected: " + retVal.ToString());
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return retVal;
        }

        /// <summary>
        /// Execute a given stored proc - with params - as a non-query
        /// Uses PrimeDataAccess transaction if present but does not commit nor rollback
        /// </summary>
        protected void ExecuteCommandAsNonQuery(DBCommandWrapper command)
        {
            try
            {
                LogCommand(command);

                db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        /// <summary>
        /// Load a given dataset, using the supplied stored proc (with parameters) and use the list of table names to dictate the order in wchi the proc supplies the data for the tables
        /// Uses PrimeDataAccess transaction if present but does not commit nor rollback
        /// Throws exception without 
        /// </summary>
        protected void LoadDataSet(string spName, DataSet ds, string[] tableNames, params object[] parameters)
        {
            try
            {
                LogCommand(spName, parameters);

                DBCommandWrapper command = db.GetStoredProcCommandWrapper(spName, parameters);
  
                db.LoadDataSet(command, ds, tableNames);

                string logMessage = "";

                foreach (DataTable tbl in ds.Tables)
                {
                    logMessage += tbl.TableName + " rows:" + tbl.Rows.Count.ToString() + Environment.NewLine;
                }

                MGRELog.Write(logMessage);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Load a given dataset, using the supplied stored proc (with parameters) and use the list of table names to dictate the order in wchi the proc supplies the data for the tables
        /// Uses PrimeDataAccess transaction if present but does not commit nor rollback
        /// Throws exception without 
        /// </summary>
        protected void LoadDataSetFromSql(string sql, DataSet ds, string[] tableNames)
        {
            try
            {
                MGRELog.Write("Sql:" + sql);

                DBCommandWrapper command = db.GetSqlStringCommandWrapper(sql);

                db.LoadDataSet(command, ds, tableNames);

                string logMessage = "";

                foreach (DataTable tbl in ds.Tables)
                {
                    logMessage += tbl.TableName + " rows:" + tbl.Rows.Count.ToString() + Environment.NewLine;
                }

                MGRELog.Write(logMessage);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Executes scalar command
        /// Uses PrimeDataAccess transaction if present but does not commit nor rollback
        /// </summary>
        /// <param name="dbCommandWrapper">command to execute</param>
        /// <returns></returns>
        protected object ExecuteScalar(DBCommandWrapper command)
        {
            object retVal = null;

            try
            {
                LogCommand(command);

                retVal = db.ExecuteScalar(command);

                if (retVal == null)
                {
                    MGRELog.Write("Returned: NULL");
                }
                else
                {
                    MGRELog.Write("Returned: " + retVal.ToString());
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return retVal;
        }

        protected int UpdateDataSet(DataSet ds, string tableName, DBCommandWrapper inCommand, DBCommandWrapper upCommand, DBCommandWrapper deCommand)
        {
            int rowsAffected = 0;

            try
            {

                rowsAffected = db.UpdateDataSet(ds, tableName, inCommand, upCommand, deCommand, UpdateBehavior.Transactional);

                MGRELog.Write("Rows affected: " + rowsAffected.ToString());

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return rowsAffected;
        }


        #endregion

        #region helper functions

        public string GetIdXml(DataTable tbl, string columnName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("IdList");
            xmlDoc.AppendChild(xmlRoot);

            foreach (DataRow row in tbl.Rows)
            {
                string idValue = row[columnName].ToString();

                XmlElement xmlId = xmlDoc.CreateElement("Id");
                xmlId.InnerText = idValue;
                xmlRoot.AppendChild(xmlId);
            }

            return xmlDoc.InnerXml;

        }

        public DbType ConvertToDbType(Type t)
        {

            System.Collections.Hashtable dbTypeTable = new System.Collections.Hashtable();
            dbTypeTable.Add(typeof(System.Boolean), DbType.Boolean);
            dbTypeTable.Add(typeof(System.Int16), DbType.Int16);
            dbTypeTable.Add(typeof(System.Int32), DbType.Int32);
            dbTypeTable.Add(typeof(System.Int64), DbType.Int64);
            dbTypeTable.Add(typeof(System.Double), DbType.Double);
            dbTypeTable.Add(typeof(System.Decimal), DbType.Decimal);
            dbTypeTable.Add(typeof(System.String), DbType.String);
            dbTypeTable.Add(typeof(System.DateTime), DbType.DateTime);
            dbTypeTable.Add(typeof(System.Guid), DbType.Guid);

            DbType dbtype;

            try
            {
                dbtype = (DbType)dbTypeTable[t];
            }
            catch
            {
                dbtype = DbType.Object;
            }

            return dbtype;
        }

        protected void HandleException(Exception ex)
        {
            MGRELog.Write(ex);

            string msg = "A data error has occurred. Please report to the Service Desk";

            if (ex.Message.StartsWith("Timeout"))
            {
                msg = "Timeout";
            }

            MGREException replaceEx = MGRELog.GetDerivedException(ex, msg);
            replaceEx.Source = "DataAccess";

            #if DEBUG //in debug mode return raw sql error
                        throw ex;
            #else

                                    throw replaceEx;
            #endif
        }

        protected void LogCommand(DBCommandWrapper cmd)
        {
            string logMessage = "";

            foreach (IDataParameter param in cmd.Command.Parameters)
            {
                if (param.Value == null)
                {
                    logMessage += (logMessage == "" ? "" : ",") + "Null";
                }
                else
                {
                    logMessage += (logMessage == "" ? "" : ",") + param.Value.ToString();
                }
            }

            logMessage = "Calling " + cmd.Command.CommandText + " " + logMessage;

            MGRELog.Write(logMessage);
        }

        protected void LogCommand(string spName, params object[] parameters)
        {
            string logMessage = "";

            foreach (object obj in parameters)
            {
                if (obj == null)
                {
                    logMessage += (logMessage == "" ? "" : ",") + "Null";
                }
                else
                {
                    logMessage += (logMessage == "" ? "" : ",") + obj.ToString();
                }
            }

            logMessage = "Calling " + spName + " " + logMessage;

            MGRELog.Write(logMessage);
        }


        #endregion

    }
}

