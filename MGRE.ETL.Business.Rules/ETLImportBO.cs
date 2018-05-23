using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGRE.ETL.Contracts;
using MGRE.ETL.Data.Contracts;
using MGRE.ETL.Common;

namespace MGRE.ETL.Business.Rules
{
    public class ETLImportBO
    {
        private IETLDAL etlDAL;  //interface which defines which data methods are required

        private ETLImportConfiguration config;

        public ETLImportBO(IETLDAL etlDAL)
        {
            this.etlDAL = etlDAL;

            config = etlDAL.GetETLImportConfiguration();

            if (config.AvailabillityIndicator == false)
            {
                throw new MGREException("ETL Import is unavailble");
            }
        }

        /// <summary>
        /// Runs ETL Import Immediately for particular export name
        /// </summary>
        public void RunImportNow(string importName, string userName)
        {
            if (importName.Length == 0)
            {
                throw new ArgumentNullException("importName");
            }

            //Retrieve GUID
            ETLImportDefinition import = etlDAL.GetETLImportByImportName(importName);

            if (import == null)
            {
                throw new Exception("ETL Import does not exist - " + importName);
            }

            RunImportNow(import, userName);
        }

        /// <summary>
        /// Runs ETL Export Immediately for particular export guid
        /// </summary>
        public void RunImportNow(Guid importGUID, string userName)
        {
            if (importGUID == Guid.Empty)
            {
                throw new ArgumentNullException("importGUID");
            }

            //Retrieve GUID
            ETLImportDefinition import = etlDAL.GetETLImportByImportGUID(importGUID);

            if (import == null)
            {
                throw new Exception("ETL Import does not exist - " + importGUID);
            }

            RunImportNow(import, userName);
        }

        /// <summary>
        /// Runs ETL Export Immediately for particular export
        /// </summary>
        public void RunImportNow(ETLImportDefinition importDefinition, string userName)
        {

            Import.ImportETL import = new Import.ImportETL();
            import.ImportETLFile(importDefinition, config.ETLImportDirectoryLocation, userName);

        }
    }
}
