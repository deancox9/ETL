//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;


namespace MGRE.ETL.Contracts.DataContracts
{
    /*
    internal partial class ETLImportEmail
    {
        #region Primitive Properties
    
        [DataMember]
        public System.Guid ETLEmailGUID
        {
            get;
            set;
        }
    
        [DataMember]
        public System.Guid ETLImportGUID
        {
            get { return _eTLImportGUID; }
            set
            {
                if (_eTLImportGUID != value)
                {
                    if (ETLImportDefinition != null && ETLImportDefinition.ETLImportGUID != value)
                    {
                        ETLImportDefinition = null;
                    }
                    _eTLImportGUID = value;
                }
            }
        }
        private System.Guid _eTLImportGUID;
    
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

        #endregion
        #region Navigation Properties
    
        public virtual ETLImportDefinition ETLImportDefinition
        {
            get { return _eTLImportDefinition; }
            set
            {
                if (!ReferenceEquals(_eTLImportDefinition, value))
                {
                    var previousValue = _eTLImportDefinition;
                    _eTLImportDefinition = value;
                    FixupETLImportDefinition(previousValue);
                }
            }
        }
        private ETLImportDefinition _eTLImportDefinition;

        #endregion
        #region Association Fixup
    
        private void FixupETLImportDefinition(ETLImportDefinition previousValue)
        {
            if (previousValue != null && previousValue.ETLImportEmails.Contains(this))
            {
                previousValue.ETLImportEmails.Remove(this);
            }
    
            if (ETLImportDefinition != null)
            {
                if (!ETLImportDefinition.ETLImportEmails.Contains(this))
                {
                    ETLImportDefinition.ETLImportEmails.Add(this);
                }
                if (ETLImportGUID != ETLImportDefinition.ETLImportGUID)
                {
                    ETLImportGUID = ETLImportDefinition.ETLImportGUID;
                }
            }
        }

        #endregion
    
    }
    */
}
