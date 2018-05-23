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
    public class ETLImportDefinition : IEntityValidate
    {

        [DataMember]
        public System.Guid ETLImportGUID
        {
            get;
            set;
        }

        [DataMember]
        public string ImportName
        {
            get;
            set;
        }

        [DataMember]
        public string ImportSubDirectory
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
        public int ImportType
        {
            get;
            set;
        }

        [DataMember]
        public List<ETLImportTablesDefinition> TableDefinitions
        {
            get;
            set;
        }

        [DataMember]
        public List<ETLImportHistory> History
        {
            get;
            set;
        }

        [DataMember]
        public List<ETLImportEmail> Emails
        {
            get;
            set;
        }

        public ValidationResult EntityValidate()
        {
            ValidationResult res = new ValidationResult();

            if (ImportName.Length == 0)
            {
                res.AddError("No import name set.");
            }

            return res;
        }
    }
}
