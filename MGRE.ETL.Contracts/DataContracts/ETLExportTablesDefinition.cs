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
    internal partial class ETLExportTablesDefinition
    {
        #region Primitive Properties
    
        [DataMember]
        public System.Guid ETLExportGUID
        {
            get { return _eTLExportGUID; }
            set
            {
                if (_eTLExportGUID != value)
                {
                    if (ETLExportDefinition != null && ETLExportDefinition.ETLExportGUID != value)
                    {
                        ETLExportDefinition = null;
                    }
                    _eTLExportGUID = value;
                }
            }
        }
        private System.Guid _eTLExportGUID;
    
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
        #region Navigation Properties
    
        public virtual ETLExportDefinition ETLExportDefinition
        {
            get { return _eTLExportDefinition; }
            set
            {
                if (!ReferenceEquals(_eTLExportDefinition, value))
                {
                    var previousValue = _eTLExportDefinition;
                    _eTLExportDefinition = value;
                    FixupETLExportDefinition(previousValue);
                }
            }
        }
        private ETLExportDefinition _eTLExportDefinition;

        #endregion
        #region Association Fixup
    
        private void FixupETLExportDefinition(ETLExportDefinition previousValue)
        {
            if (previousValue != null && previousValue.ETLExportTablesDefinitions.Contains(this))
            {
                previousValue.ETLExportTablesDefinitions.Remove(this);
            }
    
            if (ETLExportDefinition != null)
            {
                if (!ETLExportDefinition.ETLExportTablesDefinitions.Contains(this))
                {
                    ETLExportDefinition.ETLExportTablesDefinitions.Add(this);
                }
                if (ETLExportGUID != ETLExportDefinition.ETLExportGUID)
                {
                    ETLExportGUID = ETLExportDefinition.ETLExportGUID;
                }
            }
        }

        #endregion
    
    }
    */
}