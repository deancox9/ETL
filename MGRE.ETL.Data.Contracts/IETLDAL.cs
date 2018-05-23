using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGRE.ETL.Contracts;

namespace MGRE.ETL.Data.Contracts
{
    public interface IETLDAL
    {
        List<ETL.Contracts.ETLExportDefinition> GetETLExports();
        ETL.Contracts.ETLExportDefinition GetETLExportByExportName(string exportName);
        ETL.Contracts.ETLExportDefinition GetETLExportByExportGUID(Guid exportGUID);

        ETL.Contracts.ETLExportConfiguration GetETLExportConfiguration();

        ETL.Contracts.ETLImportConfiguration GetETLImportConfiguration();

        ETL.Contracts.ETLImportDefinition GetETLImportByImportName(string importName);
        ETL.Contracts.ETLImportDefinition GetETLImportByImportGUID(Guid importGUID);

        ETL.Contracts.DualAppShortcutSetting GetDualAppSettings(int appID);
        List<ETL.Contracts.vw_FundsAndProperties> GetFundsAndProperties();
    }
}
