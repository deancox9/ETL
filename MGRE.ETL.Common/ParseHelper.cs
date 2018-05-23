using System;
using System.Collections.Generic;
using System.Text;

namespace MGRE.ETL.Common
{
    #region .Net Class Documentation
    /// <summary>
    /// Class provides parsing functions for common types.
    /// </summary>
    /// <remarks> </remarks> 
    /// <history>
    ///	  
    /// </history>
    #endregion
    public class ParseHelper
    {
        
        public static int CheckInt(object o)
        {
            if (o == null)
            {
                return 0;
            }
            else
            {
                return CheckInt(o.ToString());
            }
        }

        public static int CheckInt(string s)
        {
            int i;

            if (s == "")
            {
                i = 0;
            }
            else
            {
                if (!Int32.TryParse(s, out i))
                {
                    i = 0;
                }
            }

            return i;
        }

        public static short CheckShort(object o)
        {
            if (o == null)
            {
                return 0;
            }
            else
            {
                return CheckShort(o.ToString());
            }
        }

        public static short CheckShort(string s)
        {
            short i;

            if (s == "")
            {
                i = 0;
            }
            else
            {
                if (!short.TryParse(s, out i))
                {//short not part of abf conversion so no need to be lienient
                    throw new ApplicationException("short value [" + s + "] failed parse");
                }
            }

            return i;
        }

        public static decimal? CheckDecimalNull(object o)
        {
            if (o == null)
            {
                return null;
            }
            else if (o.ToString() == "")
            {
                return null;
            }
            else
            {
                return CheckDecimal(o.ToString());
            }
        }

        public static decimal CheckDecimal(object o)
        {
            if (o == null)
            {
                return 0M;
            }
            else
            {
                return CheckDecimal(o.ToString());
            }
        }

        public static decimal CheckDecimal(string s)
        {
            decimal d;

            s = RemoveNumericSigns(s); //BC: added so that formatted currency values would be converted properly

            if (s == "")
            {
                d = 0M;
            }
            else
            {
                if (!Decimal.TryParse(s, out d))
                {
                    d = 0M;
                }
            }

            return d;
        }

        public static double CheckFloat(object o)
        {
            if (o == null)
            {
                return 0;
            }
            else
            {
                return CheckFloat(o.ToString());
            }
        }

        public static double CheckFloat(string s)
        {
            double d;
            s = RemoveNumericSigns(s); //BC: added so that formatted currency values would be converted properly

            if (s == "")
            {
                d = 0;
            }
            else
            {
                if (!double.TryParse(s, out d))
                {
                    d = 0;
                }
            }

            return d;
        }

        public static DateTime CheckDate(object o)
        {
            if (o == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                return CheckDate(o.ToString());
            }
        }

        public static DateTime CheckDate(string s)
        {
            DateTime d;

            if (s == "")
            {
                d = DateTime.MinValue;
            }
            else
            {
                if (!DateTime.TryParse(s, out d))
                {
                    d = DateTime.MinValue;
                }
            }

            return d;
        }

        // strip out display characters
        private static string RemoveNumericSigns(string numericValue)
        {
            return numericValue.Replace(",", "").Replace("£", "").Replace("%", "");
        }

        public static bool CheckBool(object o)
        {
            if (o == null)
            {
                return false;
            }
            else
            {
                return CheckBool(o.ToString());
            }
        }

        public static bool CheckBool(string s)
        {
            bool b;

            if (s == "")
            {
                b = false;
            }
            else
            {

                if (!bool.TryParse(s, out b))
                {
                    if (CheckInt(s) > 0)
                    {
                        b = true;
                    }
                    else
                    {
                        b = false;
                    }
                }
            }

            return b;
        }

    }
}
