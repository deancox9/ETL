using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;

namespace MGRE.ETL.Common
{
    #region .Net Class Documentation
    /// <summary>
    /// Set of static AppSetting classes used in application. 
    /// </summary>
    /// <remarks> </remarks> 
    /// <history>
    ///	  <revision by="barryc" ref="24063" date="2012-08-10">copied from ewatch</revision>
    /// </history>
    #endregion
    public static class AppSettingsWrapper
    {

        public static AppSetting<int> tracelQueueSizeSetting = new AppSetting<int>("Logging.TraceQueueSize", 100);
        public static AppSetting<int> screenShotQueueSizeSetting = new AppSetting<int>("Logging.ScreenShotQueueSize", 10);
        public static AppSetting<string> logDirectorySetting = new AppSetting<string>("Logging.QueueDirectory", "");
        public static TraceLevelSetting tracelLevelSetting = new TraceLevelSetting("Logging.TraceLevel", TraceLevel.Warning);
        public static AppSetting<int> applicationIdSetting = new AppSetting<int>("AppId");
        public static AppSetting<string> environmentSetting = new AppSetting<string>("Environment");
        public static AppSetting<string> reportsPathSetting = new AppSetting<string>("ReportsPath");
        public static AppSetting<string> linkedReportsPathSetting = new AppSetting<string>("LinkedReportsPath");
        public static AppSetting<string> emailPDFPathSetting = new AppSetting<string>("EmailPDFPath");

    }

    /// <summary>
    ///     Implementation of AppSetting that can parse the TracelLevele enum
    /// </summary>
    public class TraceLevelSetting : AppSetting<System.Diagnostics.TraceLevel>
    {
        public TraceLevelSetting(string settingName, System.Diagnostics.TraceLevel defaultValue)
            : base(settingName, defaultValue)
        {

        }

        public override System.Diagnostics.TraceLevel Value
        {
            get
            {
                if (!loaded)
                {
                    try
                    {
                        stringValue = System.Configuration.ConfigurationManager.AppSettings[settingName];
                        settingValue = (TraceLevel)Enum.Parse(typeof(TraceLevel), stringValue);

                        MGRELog.Write("Setting [" + settingName + "] value = " + stringValue);
                    }
                    catch
                    {
                        settingValue = defaultValue;
                        MGRELog.WriteWarning("Could not find setting [" + settingName + "] in configuration settings");
                    }

                    loaded = true;
                }

                return settingValue;
            }

        }
    }
}
