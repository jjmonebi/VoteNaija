namespace VoteNaija.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        GroupID = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false),
                        SubGroupName = c.String(),
                        IsPoll = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GroupID);
            
            CreateTable(
                "dbo.Content",
                c => new
                    {
                        ContentID = c.Int(nullable: false, identity: true),
                        GroupID = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Summary = c.String(),
                        ContentResult = c.String(),
                        Authur = c.String(),
                        Days = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        UserIDs = c.Binary(),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.ContentID)
                .ForeignKey("dbo.Group", t => t.GroupID)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.VoteSection",
                c => new
                    {
                        VoteSectionID = c.Int(nullable: false, identity: true),
                        ContentID = c.Int(nullable: false),
                        Section = c.String(nullable: false),
                        Votes = c.Int(nullable: false),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.VoteSectionID)
                .ForeignKey("dbo.Content", t => t.ContentID)
                .Index(t => t.ContentID);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        ContentID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        CommentResult = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DaysToRun = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Content", t => t.ContentID)
                .ForeignKey("dbo.UserProfile", t => t.UserID)
                .Index(t => t.ContentID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.IsLike",
                c => new
                    {
                        IsLikeID = c.Int(nullable: false, identity: true),
                        ContentID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        IsLiked = c.Boolean(),
                        IsUnliked = c.Boolean(),
                    })
                .PrimaryKey(t => t.IsLikeID)
                .ForeignKey("dbo.Content", t => t.ContentID)
                .ForeignKey("dbo.UserProfile", t => t.UserID)
                .Index(t => t.ContentID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.IsLikeComment",
                c => new
                    {
                        IsLikeID = c.Int(nullable: false, identity: true),
                        CommentID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        IsLiked = c.Boolean(),
                        IsUnliked = c.Boolean(),
                    })
                .PrimaryKey(t => t.IsLikeID)
                .ForeignKey("dbo.Comment", t => t.CommentID)
                .ForeignKey("dbo.UserProfile", t => t.UserID)
                .Index(t => t.CommentID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.webpages_Membership",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        ConfirmationToken = c.String(),
                        IsConfirmed = c.Boolean(),
                        LastPasswordFailureDate = c.DateTime(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        Password = c.String(),
                        PasswordChangedDate = c.DateTime(),
                        PasswordSalt = c.String(),
                        PasswordVerificationToken = c.String(),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_UsersInRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.webpages_OAuthMembership",
                c => new
                    {
                        Provider = c.String(nullable: false, maxLength: 128),
                        ProviderUserId = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Provider, t.ProviderUserId });
            
            CreateTable(
                "dbo.webpages_RolesUserProfile",
                c => new
                    {
                        webpages_Roles_RoleId = c.Int(nullable: false),
                        UserProfile_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.webpages_Roles_RoleId, t.UserProfile_UserId })
                .ForeignKey("dbo.webpages_Roles", t => t.webpages_Roles_RoleId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_UserId, cascadeDelete: true)
                .Index(t => t.webpages_Roles_RoleId)
                .Index(t => t.UserProfile_UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.webpages_RolesUserProfile", new[] { "UserProfile_UserId" });
            DropIndex("dbo.webpages_RolesUserProfile", new[] { "webpages_Roles_RoleId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.IsLikeComment", new[] { "UserID" });
            DropIndex("dbo.IsLikeComment", new[] { "CommentID" });
            DropIndex("dbo.IsLike", new[] { "UserID" });
            DropIndex("dbo.IsLike", new[] { "ContentID" });
            DropIndex("dbo.Comment", new[] { "UserID" });
            DropIndex("dbo.Comment", new[] { "ContentID" });
            DropIndex("dbo.VoteSection", new[] { "ContentID" });
            DropIndex("dbo.Content", new[] { "GroupID" });
            DropForeignKey("dbo.webpages_RolesUserProfile", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.webpages_RolesUserProfile", "webpages_Roles_RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.IsLikeComment", "UserID", "dbo.UserProfile");
            DropForeignKey("dbo.IsLikeComment", "CommentID", "dbo.Comment");
            DropForeignKey("dbo.IsLike", "UserID", "dbo.UserProfile");
            DropForeignKey("dbo.IsLike", "ContentID", "dbo.Content");
            DropForeignKey("dbo.Comment", "UserID", "dbo.UserProfile");
            DropForeignKey("dbo.Comment", "ContentID", "dbo.Content");
            DropForeignKey("dbo.VoteSection", "ContentID", "dbo.Content");
            DropForeignKey("dbo.Content", "GroupID", "dbo.Group");
            DropTable("dbo.webpages_RolesUserProfile");
            DropTable("dbo.webpages_OAuthMembership");
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.IsLikeComment");
            DropTable("dbo.IsLike");
            DropTable("dbo.Comment");
            DropTable("dbo.VoteSection");
            DropTable("dbo.Content");
            DropTable("dbo.Group");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.UserProfile");
        }
    }
}
