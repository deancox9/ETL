using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using MGRE.ETL.Common;

namespace MGRE.ETL.Contracts
{
    [DataContract]
    public class vw_FundsAndProperties
    {
        #region Primitive Properties

        [DataMember]
        public string ID
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public bool Migrated
        {
            get;
            set;
        }

        #endregion

    }
}
