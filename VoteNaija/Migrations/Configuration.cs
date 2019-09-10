namespace VoteNaija.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<VoteNaija.Models.VoteNaijaDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(VoteNaija.Models.VoteNaijaDBContext context)
        {
            //if (!context.Database.Exists())
            //    context.Database.Create();
            //if (!WebSecurity.Initialized)
            //    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            //var roles = (SimpleRoleProvider)Roles.Provider;
            //var membership = (SimpleMembershipProvider)Membership.Provider;

            //if (!roles.RoleExists("Admin"))
            //{
            //    roles.CreateRole("Admin");
            //}
        }
    }
}
