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
    public class ETLExportTablesDefinition
    {
        #region Primitive Properties

        [DataMember]
        public System.Guid ETLExportGUID
        {
            get;
            set;
        }

        [DataMember]
        public System.Guid ETLExportTableGUID
        {
            get;
            set;
        }

        [DataMember]
        public string TableName
        {
            get;
            set;
        }

        [DataMember]
        public string ETLFileName
        {
            get;
            set;
        }

        [DataMember]
        public string ETLHeaderName
        {
            get;
            set;
        }

        [DataMember]
        public string ProcedureName
        {
            get;
            set;
        }



        #endregion
    }
}
