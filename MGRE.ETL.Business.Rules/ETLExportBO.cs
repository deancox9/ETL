using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGRE.ETL.Contracts;
using MGRE.ETL.Data.Contracts; 
using MGRE.ETL.Common;

namespace MGRE.ETL.Business.Rules
{
    public class ETLExportBO
    {
        private IETLDAL etlDAL;  //interface which defines which data methods are required

        private ETLExportConfiguration config;

        public ETLExportBO(IETLDAL etlDAL)
        {
            this.etlDAL = etlDAL;

            config = etlDAL.GetETLExportConfiguration();

            if (config.AvailabillityIndicator == false)
            {
                throw new MGREException("ETL Export is unavailble");
            }
        }


        /// <summary>
        /// Runs ETL Export Immediately for particular export name
        /// </summary>
        public void RunExportNow(string exportName, string userName)
        {
            if (exportName.Length == 0)
            {
                throw new ArgumentNullException("exportName");
            }

            //Retrieve GUID
            ETLExportDefinition export = etlDAL.GetETLExportByExportName(exportName);

            if (export == null)
            {
                throw new Exception("ETL Export does not exist - " + exportName);
            }

            RunExportNow(export, userName);
        }

        /// <summary>
        /// Runs ETL Export Immediately for particular export guid
        /// </summary>
        public void RunExportNow(Guid exportGUID, string userName)
        {
            if (exportGUID == Guid.Empty)
            {
                throw new ArgumentNullException("exportGUID");
            }

            //Retrieve GUID
            ETLExportDefinition export = etlDAL.GetETLExportByExportGUID(exportGUID);

            if (export == null)
            {
                throw new Exception("ETL Export does not exist - " + exportGUID.ToString());
            }

            RunExportNow(export, userName);
        }

        /// <summary>
        /// Runs ETL Export Immediately for particular export
        /// </summary>
        public void RunExportNow(ETLExportDefinition exportDefinition, string userName)
        {

            if (exportDefinition.ExportType == (int)MGRE.ETL.Common.Enums.ExportType.ETL)
            {
                Export.ETLCreate export = new Export.ETLCreate();
                export.CreateETLFiles(exportDefinition, config.ETLExportDirectoryLocation, userName);
            }
            else if (exportDefinition.ExportType == (int)MGRE.ETL.Common.Enums.ExportType.Email)
            {
                Email.EmailETL email = new Email.EmailETL();
                email.Email(exportDefinition, userName);
            }
            else if (exportDefinition.ExportType == (int)MGRE.ETL.Common.Enums.ExportType.EmailETL)
            {
                Export.ETLCreate export = new Export.ETLCreate();
                export.CreateETLFilesForEmail(exportDefinition, config.ETLExportDirectoryLocation, userName);
            }
        }

    }
}
