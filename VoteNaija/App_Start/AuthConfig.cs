using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebMatrix.WebData;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.Web.WebPages.OAuth;
using VoteNaija.Models;
using System.Web.Security;

namespace VoteNaija
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            try
            {
                using (var db = new VoteNaijaDBContext())
                {
                    //if (!db.Database.Exists())
                    //{
                    //    db.Database.Create();
                    //}
                    if (!WebSecurity.Initialized)
                        WebSecurity.InitializeDatabaseConnection("MyConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                    var roles = (SimpleRoleProvider)Roles.Provider;
                    var membership = (SimpleMembershipProvider)Membership.Provider;

                    if (!roles.RoleExists("Admin"))
                    {
                        roles.CreateRole("Admin");
                    }
                }

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var x = ex.InnerException.Message;
                throw new InvalidOperationException(ex.Message);
            }

            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "1516870861897015",
                appSecret: "ebbddab9d7b9d14912f7992e56da701f");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
