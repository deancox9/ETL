using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace MGRE.ETL.Common
{
    #region .Net Class Documentation
    /// <summary>
    /// Class to wrap the parsing of application settings in app/web.config
    /// </summary>
    /// <typeparam name="T">type of setting (int, string etc)</typeparam>
    /// <remarks> </remarks> 
    /// <history>
    ///	  <revision by="barryc" ref="24063" date="2012-08-10">copied from ewatch</revision>
    /// </history>
    #endregion
    public class AppSetting<T>
    {
        /// <summary>
        ///     Instatiate object but do not yet parse config. Config parsed on first access of Value
        /// </summary>
        /// <param name="settingName">Name of setting in .config file</param>
        /// <param name="defaultValue">Value to assign if config setting missing or parse fails</param>
        public AppSetting(string settingName, T defaultValue)
        {
            this.settingName = settingName;
            this.defaultValue = defaultValue;
            this.defaultProvided = true;
        }

        public AppSetting(string settingName)
        {
            this.settingName = settingName;
        }

        protected bool defaultProvided = false;
        protected bool loaded = false;//set to true when value is parsed on first read of value
        protected string stringValue = "";//string representation of value
        protected string settingName = "";//name of setting
        protected T defaultValue;
        protected T settingValue;

        /// <summary>
        ///     Get the typed value of the setting
        /// </summary>
        public virtual T Value
        {
            get
            {
                if (!loaded)
                {
                    try
                    {
                        stringValue = System.Configuration.ConfigurationManager.AppSettings[settingName];
                        settingValue = (T)Convert.ChangeType(stringValue, typeof(T));

                        MGRELog.Write("Setting [" + settingName + "] value = " + stringValue);

                    }
                    catch
                    {
                        if (defaultProvided)
                        {
                            settingValue = defaultValue;
                            MGRELog.WriteWarning("Could not find setting [" + settingName + "] in configuration settings");
                        }
                        else
                        {
                            throw new MGREException("Could not find setting [" + settingName + "] in configuration settings");
                        }
                    }

                    loaded = true;
                }

                return settingValue;
            }

        }
    }
}