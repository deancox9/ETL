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
    /// DataContract to represent an ETL Export Email
    /// </summary>
    /// <remarks>DataMembers often mirror an equivalent EntityFramework db object (i.e. table). See DataContracts folder for auto generated DataContracts</remarks> 
    /// <history>
    ///	  <revision by="deanc" ref="24063" date="2013-07-30">created</revision>
    /// </history>
    #endregion
    [DataContract]
    public class ETLExportEmail
    {
        [DataMember]
        public System.Guid ETLEmailGUID
        {
            get;
            set;
        }

        [DataMember]
        public System.Guid ETLExportGUID
        {
            get;
            set;
        }

        [DataMember]
        public string Description
        {
            get;
            set;
        }

        [DataMember]
        public string Subject
        {
            get;
            set;
        }

        [DataMember]
        public string Body
        {
            get;
            set;
        }

        [DataMember]
        public string ToAssignees
        {
            get;
            set;
        }

        [DataMember]
        public string CCAssignees
        {
            get;
            set;
        }

        [DataMember]
        public string BCCAssignees
        {
            get;
            set;
        }

        [DataMember]
        public string FromAddress
        {
            get;
            set;
        }

        [DataMember]
        public bool EmailIsDataSpecific
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



    }
}
