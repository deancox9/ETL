using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGRE.ETL.Contracts;
using MGRE.ETL.Data.Contracts; 
using MGRE.ETL.Common;

namespace MGRE.ETL.Business.Rules
{
    public class ETLBO
    {
        private IETLDAL etlDAL;  //interface which defines which data methods are required

        public ETLBO(IETLDAL etlDAL)
        {
            this.etlDAL = etlDAL;
        }


        /// <summary>
        /// Runs ETL Export Immediately for particular export name
        /// </summary>
        public void RunExportNow(string exportName)
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
        }

        /// <summary>
        /// Runs ETL Export Immediately for particular export guid
        /// </summary>
        public void RunExportNow(Guid exportGUID)
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

            RunExportNow(export);
        }

        /// <summary>
        /// Runs ETL Export Immediately for particular export
        /// </summary>
        public void RunExportNow(ETLExportDefinition exportDefinition)
        {
            Export.ETLCreate export = new Export.ETLCreate();

            bool result = export.CreateETLFiles(exportDefinition);
        }

    }
}
