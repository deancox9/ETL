using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data.Objects;

using Microsoft.Practices.EnterpriseLibrary.Data;
using MGRE.ETL.Common;

namespace MGRE.ETL.Data
{
    #region .Net Class Documentation
    /// <summary>
    /// Data class to provide connectivity to database via an entity framework connection.
    /// </summary>
    /// <remarks> </remarks> 
    /// <history>
    ///	  <revision by="deanc" ref="24063" date="2013-07-29">created</revision>
    /// </history>
    #endregion
    public class BaseData : IDisposable
    {
        protected ETLEntities dataContext;
        protected EntityConnection entConn;
        protected static string connectionString = "";

        protected BaseData()
        {
            try
            {
                if (connectionString == "")
                {//use the enterprise library config to get a connection string
                    MGRELog.Write("Setting connection string");

                    Database db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("HUB");
                    System.Data.IDbConnection cnn = db.GetConnection();
                    connectionString = db.GetConnection().ConnectionString;
                    cnn.Dispose();

                    MGRELog.Write("Connection string set");
                }

                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();

                //Set the provider name.
                entityBuilder.Provider = "System.Data.SqlClient";

                // Set the provider-specific connection string.
                entityBuilder.ProviderConnectionString = connectionString;

                // Set the Metadata location.
                entityBuilder.Metadata = @"res://*/ETL.csdl|
                                    res://*/ETL.ssdl|
                                    res://*/ETL.msl";

                entConn = new EntityConnection(entityBuilder.ToString());
                dataContext = new ETLEntities(entConn);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected ETLEntities DataContext
        {
            get { return dataContext; }
        }

        public virtual void Dispose()
        {
            dataContext.Dispose();
            entConn.Dispose();
        }

        protected ApplicationException HandleException(Exception ex)
        {
            MGRELog.Write(ex);

            if (ex is System.Data.DBConcurrencyException)
            {
                return new ApplicationException(ex.Message, ex);
            }
            else
            {
#if DEBUG //in debug mode return raw sql error
                ApplicationException replaceEx = new ApplicationException(ex.Message, ex);
                replaceEx.Source = "DataAccess";
#else
                                string msg = "A data error has occurred. Please report to the Service Desk";

                                if (ex.Message.StartsWith("Timeout"))
                                {
                                    msg = "Timeout";
                                }
                                ApplicationException replaceEx = new ApplicationException(msg);
                                replaceEx.Source = "DataAccess";     
#endif

                return replaceEx;
            }
        }


    }
}

