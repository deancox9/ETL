using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using MGRE.ETL.Common;

namespace MGRE.ETL.Contracts
{
    #region .Net Class Documentation
    /// <summary>
    /// DataContract to represent a report snapshot in reporting services. 
    /// </summary>
    /// <remarks>DataMembers often mirror an equivalent EntityFramework db object (i.e. table). See DataContracts folder for auto generated DataContracts</remarks> 
    /// <history>
    ///	  <revision by="deanc" ref="24063" date="2013-07-30">created</revision>
    /// </history>
    #endregion
    [DataContract]
    public class ETLExportDefinition : IEntityValidate
    {

        [DataMember]
        public System.Guid ETLExportGUID
        {
            get;
            set;
        }

        [DataMember]
        public string ApplicationDatabase
        {
            get;
            set;
        }

        [DataMember]
        public string ExportName
        {
            get;
            set;
        }

        [DataMember]
        public string ExportSubDirectory
        {
            get;
            set;
        }

        [DataMember]
        public string Reason
        {
            get;
            set;
        }

        [DataMember]
        public bool Active
        {
            get;
            set;
        }

        [DataMember]
        public int ExportType
        {
            get;
            set;
        }

        [DataMember]
        public string StatusProcedureName
        {
            get;
            set;
        }

        [DataMember]
        public List<ETLExportTablesDefinition> TableDefinitions
        {
            get;
            set;
        }

        [DataMember]
        public List<ETLExportHistory> History
        {
            get;
            set;
        }

        [DataMember]
        public List<ETLExportEmail> Emails
        {
            get;
            set;
        }

        public ValidationResult EntityValidate()
        {
            ValidationResult res = new ValidationResult();

            if (ExportName.Length == 0)
            {
                res.AddError("No export name set.");
            }

            return res;
        }
    }
}
