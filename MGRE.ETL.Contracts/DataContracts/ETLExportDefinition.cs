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
    internal partial class ETLExportDefinition
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
                    if (ETLExportSchedule != null && ETLExportSchedule.ETLExportGUID != value)
                    {
                        ETLExportSchedule = null;
                    }
                    _eTLExportGUID = value;
                }
            }
        }
        private System.Guid _eTLExportGUID;
    
        [DataMember]
        public string ApplicationDatabase
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

        #endregion
        #region Navigation Properties
    
        public virtual ETLExportSchedule ETLExportSchedule
        {
            get { return _eTLExportSchedule; }
            set
            {
                if (!ReferenceEquals(_eTLExportSchedule, value))
                {
                    var previousValue = _eTLExportSchedule;
                    _eTLExportSchedule = value;
                    FixupETLExportSchedule(previousValue);
                }
            }
        }
        private ETLExportSchedule _eTLExportSchedule;
    
        public virtual ICollection<ETLExportHistory> ETLExportHistories
        {
            get
            {
                if (_eTLExportHistories == null)
                {
                    var newCollection = new FixupCollection<ETLExportHistory>();
                    newCollection.CollectionChanged += FixupETLExportHistories;
                    _eTLExportHistories = newCollection;
                }
                return _eTLExportHistories;
            }
            set
            {
                if (!ReferenceEquals(_eTLExportHistories, value))
                {
                    var previousValue = _eTLExportHistories as FixupCollection<ETLExportHistory>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupETLExportHistories;
                    }
                    _eTLExportHistories = value;
                    var newValue = value as FixupCollection<ETLExportHistory>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupETLExportHistories;
                    }
                }
            }
        }
        private ICollection<ETLExportHistory> _eTLExportHistories;
    
        public virtual ICollection<ETLExportTablesDefinition> ETLExportTablesDefinitions
        {
            get
            {
                if (_eTLExportTablesDefinitions == null)
                {
                    var newCollection = new FixupCollection<ETLExportTablesDefinition>();
                    newCollection.CollectionChanged += FixupETLExportTablesDefinitions;
                    _eTLExportTablesDefinitions = newCollection;
                }
                return _eTLExportTablesDefinitions;
            }
            set
            {
                if (!ReferenceEquals(_eTLExportTablesDefinitions, value))
                {
                    var previousValue = _eTLExportTablesDefinitions as FixupCollection<ETLExportTablesDefinition>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupETLExportTablesDefinitions;
                    }
                    _eTLExportTablesDefinitions = value;
                    var newValue = value as FixupCollection<ETLExportTablesDefinition>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupETLExportTablesDefinitions;
                    }
                }
            }
        }
        private ICollection<ETLExportTablesDefinition> _eTLExportTablesDefinitions;
    
        public virtual ICollection<ETLExportEmail> ETLExportEmails
        {
            get
            {
                if (_eTLExportEmails == null)
                {
                    var newCollection = new FixupCollection<ETLExportEmail>();
                    newCollection.CollectionChanged += FixupETLExportEmails;
                    _eTLExportEmails = newCollection;
                }
                return _eTLExportEmails;
            }
            set
            {
                if (!ReferenceEquals(_eTLExportEmails, value))
                {
                    var previousValue = _eTLExportEmails as FixupCollection<ETLExportEmail>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupETLExportEmails;
                    }
                    _eTLExportEmails = value;
                    var newValue = value as FixupCollection<ETLExportEmail>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupETLExportEmails;
                    }
                }
            }
        }
        private ICollection<ETLExportEmail> _eTLExportEmails;

        #endregion
        #region Association Fixup
    
        private void FixupETLExportSchedule(ETLExportSchedule previousValue)
        {
            if (previousValue != null && ReferenceEquals(previousValue.ETLExportDefinition, this))
            {
                previousValue.ETLExportDefinition = null;
            }
    
            if (ETLExportSchedule != null)
            {
                ETLExportSchedule.ETLExportDefinition = this;
                if (ETLExportGUID != ETLExportSchedule.ETLExportGUID)
                {
                    ETLExportGUID = ETLExportSchedule.ETLExportGUID;
                }
            }
        }
    
        private void FixupETLExportHistories(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ETLExportHistory item in e.NewItems)
                {
                    item.ETLExportDefinition = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (ETLExportHistory item in e.OldItems)
                {
                    if (ReferenceEquals(item.ETLExportDefinition, this))
                    {
                        item.ETLExportDefinition = null;
                    }
                }
            }
        }
    
        private void FixupETLExportTablesDefinitions(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ETLExportTablesDefinition item in e.NewItems)
                {
                    item.ETLExportDefinition = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (ETLExportTablesDefinition item in e.OldItems)
                {
                    if (ReferenceEquals(item.ETLExportDefinition, this))
                    {
                        item.ETLExportDefinition = null;
                    }
                }
            }
        }
    
        private void FixupETLExportEmails(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ETLExportEmail item in e.NewItems)
                {
                    item.ETLExportDefinition = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (ETLExportEmail item in e.OldItems)
                {
                    if (ReferenceEquals(item.ETLExportDefinition, this))
                    {
                        item.ETLExportDefinition = null;
                    }
                }
            }
        }

        #endregion
    
    }
    */
}
