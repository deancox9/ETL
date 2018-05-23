using System;
using System.Text;

namespace MGRE.ETL.Contracts
{
    #region .Net Class Documentation
    /// <summary>
    /// interface for DataContracts when self validating
    /// </summary>
    /// <remarks> </remarks> 
    /// <history>
    ///	  <revision by="barryc" ref="24063" date="2012-08-10">created</revision>
    /// </history>
    #endregion
    interface IEntityValidate
    {
        Common.ValidationResult EntityValidate();
    }
}

