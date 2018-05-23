using System;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Caching;

using MGRE.ETL.Common;

namespace MGRE.ETL.Web.Service
{
    /// <summary>
    /// Summary description for AuthorisedWebService
    /// </summary>
    public class AuthorisedService
    {
        private string userName;
        private PruPIM.Common.User.DataSets.UserDataSet userDs;

        public AuthorisedService()
        {
            MGREServerLog.AttachToLoggingHandler();

            MGRELog.WriteMethodStart("AuthorisedService()");

            try
            {
                userName = System.ServiceModel.ServiceSecurityContext.Current.WindowsIdentity.Name;

                MGRELog.Write("UserName:" + userName);

                TraceLevel traceLevel = AppSettingsWrapper.tracelLevelSetting.Value;
                int appId = AppSettingsWrapper.applicationIdSetting.Value;

                string userDsKey = "";
                object cacheUser;

                System.Data.DataSet ds;

                string[] segments = userName.Split('\\');
                if (segments.Length > 1)
                    userName = segments[1];

                userDsKey = "UserDs" + appId.ToString() + userName;

                MGRELog.Write("userDsKey:" + userDsKey);

                cacheUser = MemoryCache.Default.Get(userDsKey);

                if (cacheUser != null)
                {
                    MGRELog.Write("User in cache");
                    userDs = (PruPIM.Common.User.DataSets.UserDataSet)cacheUser;
                }
                else
                {
                    MGRELog.Write("Querying user from ic");
                    PruPIM.Common.User.Business.Rules.UserBusinessRule user = new PruPIM.Common.User.Business.Rules.UserBusinessRule(TraceLevel.Error);

                    ds = user.GetUser(userName, appId);

                    MGRELog.WriteDataSet(ds);

                    userDs = new PruPIM.Common.User.DataSets.UserDataSet();
                    userDs.Merge(ds);

                    CacheItemPolicy cachePolicy = new CacheItemPolicy();
                    cachePolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                    cachePolicy.Priority = System.Runtime.Caching.CacheItemPriority.Default;

                    MemoryCache.Default.Add(userDsKey, userDs, cachePolicy);
                }

                if (userDs.Users.Count == 0)
                {
                    throw new System.Security.SecurityException("No valid user authenticated. Impersonation may not be setup on the web server.");
                }

                if (userDs.AllowedScreens.Count == 0)
                {
                    throw new System.Security.SecurityException("No access to EWatch");
                }
            }
            catch (Exception ex)
            {
                MGRELog.Write(ex);
                throw;
            }

        }

        protected Guid UserGuid
        {
            get { return userDs.Users[0].InternalContactGUID; }
        }

        protected PruPIM.Common.User.DataSets.UserDataSet UserDs
        {
            get { return userDs; }
        }

        protected string UserName
        {
            get { return userName; }
        }
    }
}