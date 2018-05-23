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
    public class ETLImportHistory
    {
        #region Primitive Properties

        [DataMember]
        public System.Guid ETLImportHistoryGUID
        {
            get;
            set;
        }

        [DataMember]
        public System.Guid ETLImportGUID
        {
            get;
            set;
        }

        [DataMember]
        public System.DateTime ETLImportDate
        {
            get;
            set;
        }

        [DataMember]
        public string ETLImportSubmittedBy
        {
            get;
            set;
        }

        [DataMember]
        public System.DateTime ETLImportStartDate
        {
            get;
            set;
        }

        [DataMember]
        public System.DateTime ETLImportEndDate
        {
            get;
            set;
        }

        [DataMember]
        public int Status
        {
            get;
            set;
        }

        #endregion
    }
}
