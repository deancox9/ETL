using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGRE.ETL.Common
{
    #region .Net Class Documentation
    /// <summary>
    /// class to hold constants
    /// </summary>
    /// <remarks> </remarks> 
    /// <history>
    ///	  
    /// </history>
    #endregion
    public static class Consts
    {

    }
}

namespace MGRE.ETL.Common.Enums
{
    public enum ExportType
    {
        NotSet = 0,
        ETL = 1,
        Email = 2,
        EmailETL = 3
    }

    public enum ETLStatus
    {
        Pending = 0,
        Success = 1,
        Failed = 2
    }
}
