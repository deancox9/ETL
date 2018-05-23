using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data.Objects;

using Microsoft.Practices.EnterpriseLibrary.Data;

namespace MGRE.ETL.Data
{
    #region .Net Class Documentation
    /// <summary>
    /// Data class to provide methods for ETL data access. Implements IETLDAL for use by business objects
    /// </summary>
    /// <remarks> </remarks> 
    /// <history>
    ///	  <revision by="deanc" ref="24063" date="2013-07-30">created</revision>
    /// </history>
    #endregion
    public class ETLData : BaseData, MGRE.ETL.Data.Contracts.IETLDAL
    {
        /// <summary>
        /// Get a list of currently configured ETL Exports
        /// </summary>
        public List<ETL.Contracts.ETLExportDefinition> GetETLExports()
        {
            try
            {
                List<ETLExportDefinition> exports = dataContext.ETLExportDefinitions.ToList<ETLExportDefinition>();

                return DataTransform.ToETLExportDefinitionContract(exports);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        /// <summary>
        /// Get a configured ETL Export for a particular Export Name
        /// </summary>
        public ETL.Contracts.ETLExportDefinition GetETLExportByExportName(string exportName)
        {
            try
            {
                ETLExportDefinition export = (from e in dataContext.ETLExportDefinitions
                                              where e.ExportName == exportName
                                                    select e).FirstOrDefault();

                return DataTransform.ToETLExportDefinitionContract(export);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        /// <summary>
        /// Get a configured ETL Export for a particular Export Guid
        /// </summary>
        public ETL.Contracts.ETLExportDefinition GetETLExportByExportGUID(Guid exportGUID)
        {
            try
            {
                ETLExportDefinition export = (from e in dataContext.ETLExportDefinitions
                                              where e.ETLExportGUID == exportGUID
                                              select e).FirstOrDefault();

                return DataTransform.ToETLExportDefinitionContract(export);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }



        public ETL.Contracts.ETLExportConfiguration GetETLExportConfiguration()
        {
            try
            {
                ETLExportConfiguration config = (from c in dataContext.ETLExportConfigurations
                                              select c).FirstOrDefault();

                return DataTransform.ToETLExportConfigurationContract(config);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public ETL.Contracts.ETLImportConfiguration GetETLImportConfiguration()
        {
            try
            {
                ETLImportConfiguration config = (from c in dataContext.ETLImportConfigurations
                                                 select c).FirstOrDefault();

                return DataTransform.ToETLImportConfigurationContract(config);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        /// <summary>
        /// Get a configured ETL Import for a particular Import Name
        /// </summary>
        public ETL.Contracts.ETLImportDefinition GetETLImportByImportName(string importName)
        {
            try
            {
                ETLImportDefinition import = (from e in dataContext.ETLImportDefinitions
                                              where e.ImportName == importName
                                              select e).FirstOrDefault();

                return DataTransform.ToETLImportDefinitionContract(import);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        /// <summary>
        /// Get a configured ETL Import for a particular Import Guid
        /// </summary>
        public ETL.Contracts.ETLImportDefinition GetETLImportByImportGUID(Guid importGUID)
        {
            try
            {
                ETLImportDefinition import = (from e in dataContext.ETLImportDefinitions
                                              where e.ETLImportGUID == importGUID
                                              select e).FirstOrDefault();

                return DataTransform.ToETLImportDefinitionContract(import);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }


        public ETL.Contracts.DualAppShortcutSetting GetDualAppSettings(int appID)
        {
            try
            {
                DualAppShortcutSetting settings = (from d in dataContext.DualAppShortcutSettings
                                                   where d.AppID == appID
                                              select d).FirstOrDefault();

                return DataTransform.ToDualAppShortcutSettingContract(settings);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public List<ETL.Contracts.vw_FundsAndProperties> GetFundsAndProperties()
        {
            try
            {
                List<vw_FundsAndProperties> ents = dataContext.vw_FundsAndProperties.ToList<vw_FundsAndProperties>();

                return DataTransform.ToFundsAndPropertiesContract(ents);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }
    }
}
