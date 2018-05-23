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
    public class  DualAppShortcutSetting : IEntityValidate
    {
        #region Primitive Properties
    
        [DataMember]
        public int AppID
        {
            get;
            set;
        }
    
        [DataMember]
        public string WindowTitle
        {
            get;
            set;
        }
    
        [DataMember]
        public string WindowBackgroundColour
        {
            get;
            set;
        }
    
        [DataMember]
        public string WindowBorderColor
        {
            get;
            set;
        }
    
        [DataMember]
        public string PrimeButtonBackgroundColor
        {
            get;
            set;
        }
    
        [DataMember]
        public string HeaderTextColor
        {
            get;
            set;
        }
    
        [DataMember]
        public string PrimeTextColor
        {
            get;
            set;
        }
    
        [DataMember]
        public string YardiTextColor
        {
            get;
            set;
        }
    
        [DataMember]
        public string HeaderText
        {
            get;
            set;
        }
    
        [DataMember]
        public string PrimeButtonText
        {
            get;
            set;
        }
    
        [DataMember]
        public string YardiButtonText
        {
            get;
            set;
        }
    
        [DataMember]
        public string YardiButtonBackgroundColor
        {
            get;
            set;
        }
    
        [DataMember]
        public bool YardiButtonEnabled
        {
            get;
            set;
        }
    
        [DataMember]
        public bool PrimeButtonEnabled
        {
            get;
            set;
        }

        #endregion

        public ValidationResult EntityValidate()
        {
            ValidationResult res = new ValidationResult();

            if (AppID == 0)
            {
                res.AddError("No Application ID set.");
            }

            return res;
        }
    }

}
