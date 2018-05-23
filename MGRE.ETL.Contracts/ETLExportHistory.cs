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
    public class ETLExportHistory
    {
        #region Primitive Properties

        [DataMember]
        public System.Guid ETLExportHistoryGUID
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
        public System.DateTime ETLExportDate
        {
            get;
            set;
        }

        [DataMember]
        public string ETLExportSubmittedBy
        {
            get;
            set;
        }

        [DataMember]
        public System.DateTime ETLExportStartDate
        {
            get;
            set;
        }

        [DataMember]
        public System.DateTime ETLExportEndDate
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
