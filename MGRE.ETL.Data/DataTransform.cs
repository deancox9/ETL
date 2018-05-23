using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataContracts = MGRE.ETL.Contracts;
using DataRows = MGRE.ETL.Data;

namespace MGRE.ETL.Data
{
    #region .Net Class Documentation
    /// <summary>
    /// Provides automapper transforms between DataContracts to DataRow and vice versa
    /// AutoMapper is used to copy properties from objects to object based on naming convention
    /// </summary>
    /// <remarks> </remarks> 
    /// <history>
    ///	  <revision by="deanc" ref="24063" date="2012-07-30">created</revision>
    /// </history>
    #endregion
    public static class DataTransform
    {

        static DataTransform()
        {
            //AutoMapper is used to copy properties from data objects to DataContracts
            AutoMapper.Mapper.CreateMap<DataRows.ETLExportConfiguration, DataContracts.ETLExportConfiguration>();
            AutoMapper.Mapper.CreateMap<DataRows.ETLExportDefinition, DataContracts.ETLExportDefinition>();
            AutoMapper.Mapper.CreateMap<DataRows.ETLExportSchedule, DataContracts.ETLExportSchedule>();
            AutoMapper.Mapper.CreateMap<DataRows.ETLExportTablesDefinition, DataContracts.ETLExportTablesDefinition>();
            AutoMapper.Mapper.CreateMap<DataRows.ETLExportEmail, DataContracts.ETLExportEmail>();
            AutoMapper.Mapper.CreateMap<DataRows.ETLPeriodicity, DataContracts.ETLPeriodicity>();

            AutoMapper.Mapper.CreateMap<DataRows.ETLImportConfiguration, DataContracts.ETLImportConfiguration>();
            AutoMapper.Mapper.CreateMap<DataRows.ETLImportDefinition, DataContracts.ETLImportDefinition>();
            AutoMapper.Mapper.CreateMap<DataRows.ETLImportTablesDefinition, DataContracts.ETLImportTablesDefinition>();
            AutoMapper.Mapper.CreateMap<DataRows.ETLImportEmail, DataContracts.ETLImportEmail>();

            AutoMapper.Mapper.CreateMap<DataRows.DualAppShortcutSetting, DataContracts.DualAppShortcutSetting>();
            AutoMapper.Mapper.CreateMap<DataRows.vw_FundsAndProperties, DataContracts.vw_FundsAndProperties>();

            //Map TableDefinition collection to related TableDefinitions rows
            AutoMapper.Mapper.CreateMap<DataRows.ETLExportDefinition, DataContracts.ETLExportDefinition>()
                .ForMember(dest => dest.TableDefinitions, obj => obj.ResolveUsing(src => AutoMapper.Mapper.Map<List<DataContracts.ETLExportTablesDefinition>>(src.ETLExportTablesDefinitions)));

            ////Map TableDefinitions rows from TableDefinition collection 
            AutoMapper.Mapper.CreateMap<DataContracts.ETLExportDefinition, DataRows.ETLExportDefinition>()
               .ForMember(dest => dest.ETLExportTablesDefinitions, obj => obj.ResolveUsing(src => AutoMapper.Mapper.Map<List<DataRows.ETLExportTablesDefinition>>(src.TableDefinitions)));

            //Map TableDefinition collection to related TableDefinitions rows
            AutoMapper.Mapper.CreateMap<DataRows.ETLExportDefinition, DataContracts.ETLExportDefinition>()
                .ForMember(dest => dest.Emails, obj => obj.ResolveUsing(src => AutoMapper.Mapper.Map<List<DataContracts.ETLExportEmail>>(src.ETLExportEmails)));


            ////Map TableDefinitions rows from TableDefinition collection 
            AutoMapper.Mapper.CreateMap<DataContracts.ETLExportDefinition, DataRows.ETLExportDefinition>()
               .ForMember(dest => dest.ETLExportEmails, obj => obj.ResolveUsing(src => AutoMapper.Mapper.Map<List<DataRows.ETLExportEmail>>(src.Emails)));


            ////Map History collection to related History rows
            //AutoMapper.Mapper.CreateMap<DataRows.ETLExportDefinition, DataContracts.ETLExportDefinition>()
            //    .ForMember(dest => dest.History, obj => obj.ResolveUsing(src => AutoMapper.Mapper.Map<List<DataContracts.ETLExportHistory>>(src.ETLExportHistories)));

            //////Map History rows from History collection 
            //AutoMapper.Mapper.CreateMap<DataContracts.ETLExportDefinition, DataRows.ETLExportDefinition>()
            //   .ForMember(dest => dest.ETLExportHistories, obj => obj.ResolveUsing(src => AutoMapper.Mapper.Map<List<DataRows.ETLExportHistory>>(src.History)));


            //Map TableDefinition collection to related TableDefinitions rows
            AutoMapper.Mapper.CreateMap<DataRows.ETLImportDefinition, DataContracts.ETLImportDefinition>()
                .ForMember(dest => dest.TableDefinitions, obj => obj.ResolveUsing(src => AutoMapper.Mapper.Map<List<DataContracts.ETLImportTablesDefinition>>(src.ETLImportTablesDefinitions)));

            ////Map TableDefinitions rows from TableDefinition collection 
            AutoMapper.Mapper.CreateMap<DataContracts.ETLImportDefinition, DataRows.ETLImportDefinition>()
               .ForMember(dest => dest.ETLImportTablesDefinitions, obj => obj.ResolveUsing(src => AutoMapper.Mapper.Map<List<DataRows.ETLImportTablesDefinition>>(src.TableDefinitions)));

            //Map TableDefinition collection to related TableDefinitions rows
            AutoMapper.Mapper.CreateMap<DataRows.ETLImportDefinition, DataContracts.ETLImportDefinition>()
                .ForMember(dest => dest.Emails, obj => obj.ResolveUsing(src => AutoMapper.Mapper.Map<List<DataContracts.ETLImportEmail>>(src.ETLImportEmails)));


            ////Map TableDefinitions rows from TableDefinition collection 
            AutoMapper.Mapper.CreateMap<DataContracts.ETLImportDefinition, DataRows.ETLImportDefinition>()
               .ForMember(dest => dest.ETLImportEmails, obj => obj.ResolveUsing(src => AutoMapper.Mapper.Map<List<DataRows.ETLImportEmail>>(src.Emails)));


            ////Map History collection to related History rows
            //AutoMapper.Mapper.CreateMap<DataRows.ETLImportDefinition, DataContracts.ETLImportDefinition>()
            //    .ForMember(dest => dest.History, obj => obj.ResolveUsing(src => AutoMapper.Mapper.Map<List<DataContracts.ETLImportHistory>>(src.ETLImportHistories)));

            //////Map History rows from History collection 
            //AutoMapper.Mapper.CreateMap<DataContracts.ETLImportDefinition, DataRows.ETLImportDefinition>()
            //   .ForMember(dest => dest.ETLImportHistories, obj => obj.ResolveUsing(src => AutoMapper.Mapper.Map<List<DataRows.ETLImportHistory>>(src.History)));


        }

        public static DataContracts.ETLExportConfiguration ToETLExportConfigurationContract(DataRows.ETLExportConfiguration row)
        {
            return AutoMapper.Mapper.Map<DataContracts.ETLExportConfiguration>(row);
        }

        public static DataContracts.ETLImportConfiguration ToETLImportConfigurationContract(DataRows.ETLImportConfiguration row)
        {
            return AutoMapper.Mapper.Map<DataContracts.ETLImportConfiguration>(row);
        }

        public static DataContracts.ETLExportDefinition ToETLExportDefinitionContract(DataRows.ETLExportDefinition row)
        {
            return AutoMapper.Mapper.Map<DataContracts.ETLExportDefinition>(row);
        }

        public static List<DataContracts.ETLExportDefinition> ToETLExportDefinitionContract(List<DataRows.ETLExportDefinition> row)
        {
            return AutoMapper.Mapper.Map<List<DataContracts.ETLExportDefinition>>(row);
        }

        public static List<DataContracts.ETLExportTablesDefinition> ToETLExportTableDefinitionContract(List<DataRows.ETLExportDefinition> row)
        {
            return AutoMapper.Mapper.Map<List<DataContracts.ETLExportTablesDefinition>>(row);
        }

        public static List<DataContracts.ETLExportHistory> ToETLExportHistoryContract(List<DataRows.ETLExportDefinition> row)
        {
            return AutoMapper.Mapper.Map<List<DataContracts.ETLExportHistory>>(row);
        }

        public static List<DataContracts.ETLExportEmail> ToETLExportEmailContract(List<DataRows.ETLExportEmail> row)
        {
            return AutoMapper.Mapper.Map<List<DataContracts.ETLExportEmail>>(row);
        }


        public static DataContracts.ETLImportDefinition ToETLImportDefinitionContract(DataRows.ETLImportDefinition row)
        {
            return AutoMapper.Mapper.Map<DataContracts.ETLImportDefinition>(row);
        }

        public static List<DataContracts.ETLImportDefinition> ToETLImportDefinitionContract(List<DataRows.ETLImportDefinition> row)
        {
            return AutoMapper.Mapper.Map<List<DataContracts.ETLImportDefinition>>(row);
        }

        public static List<DataContracts.ETLImportTablesDefinition> ToETLImportTableDefinitionContract(List<DataRows.ETLImportDefinition> row)
        {
            return AutoMapper.Mapper.Map<List<DataContracts.ETLImportTablesDefinition>>(row);
        }

        public static List<DataContracts.ETLImportHistory> ToETLImportHistoryContract(List<DataRows.ETLImportDefinition> row)
        {
            return AutoMapper.Mapper.Map<List<DataContracts.ETLImportHistory>>(row);
        }

        public static List<DataContracts.ETLImportEmail> ToETLImportEmailContract(List<DataRows.ETLImportEmail> row)
        {
            return AutoMapper.Mapper.Map<List<DataContracts.ETLImportEmail>>(row);
        }

        public static DataContracts.DualAppShortcutSetting ToDualAppShortcutSettingContract(DataRows.DualAppShortcutSetting row)
        {
            return AutoMapper.Mapper.Map<DataContracts.DualAppShortcutSetting>(row);
        }

        public static List<DataContracts.vw_FundsAndProperties> ToFundsAndPropertiesContract(List<DataRows.vw_FundsAndProperties> row)
        {
            return AutoMapper.Mapper.Map<List<DataContracts.vw_FundsAndProperties>>(row);
        }
    }
}
