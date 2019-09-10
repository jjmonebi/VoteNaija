using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace VoteNaija.Models
{
    public class VoteNaijaDBContext : DbContext
    {
        public VoteNaijaDBContext()
            : base("MyConnection")
        {
            Database.SetInitializer<VoteNaijaDBContext>(new DropCreateDatabaseIfModelChanges<VoteNaijaDBContext>());
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<VoteSection> VoteSections { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<IsLike> IsLikes { get; set; }

        public DbSet<IsLikeComment> IsLikeComments { get; set; }
        public DbSet<webpages_Membership> webpages_Memberships { get; set; }
        public DbSet<webpages_Roles> webpages_Roless { get; set; }
        public DbSet<webpages_UsersInRoles> webpages_UsersInRoless { get; set; }
        public DbSet<webpages_OAuthMembership> webpages_OAuthMemberships { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove <OneToManyCascadeDeleteConvention>();
        }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        public UserProfile() 
        {
            this.webpages_Roles = new HashSet<webpages_Roles>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        public virtual ICollection<webpages_Roles> webpages_Roles { get; set; }
    }

    [Table("Group")]
    public class Group
    {
        public Group() 
        {

        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GroupID { get; set; }
        [Required]
        public string GroupName { get; set; }
        public string SubGroupName { get; set; }
        [Required]
        public bool IsPoll { get; set; }
    }

    [Table("Content")]
    public class Content 
    {
        public Content() 
        {

        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ContentID { get; set; }
        [ForeignKey("Group")]
        public int GroupID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string ContentResult { get; set; }
        public string Authur { get; set; }
        public int Days { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public byte[] UserIDs { get; set; }

        public string ImageUrl { get; set; }

        public virtual Group Group { get; set; }
    }

    [Table("VoteSection")]
    public class VoteSection 
    {
        public VoteSection() 
        {

        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int VoteSectionID { get; set; }

        [ForeignKey("Content")]
        public int ContentID { get; set; }
        [Required]
        public string Section { get; set; }
        [Required]
        public int Votes { get; set; }

        public string ImageUrl { get; set; }
        public virtual Content Content { get; set; }
    }

    [Table("Comment")]
    public class Comment 
    {
        public Comment() 
        {

        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }

        [ForeignKey("Content")]
        public int ContentID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        [Required]
        public string CommentResult { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public int DaysToRun { get;set;}
        public virtual Content Content { get; set; }
        public virtual UserProfile User { get; set; }
    }

    [Table("IsLike")]
    public class IsLike 
    {
        public IsLike() 
        {

        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int IsLikeID { get; set; }
        
        [ForeignKey("Content")]
        public int ContentID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public bool? IsLiked { get; set; }
        public bool? IsUnliked { get; set; }
        public virtual Content Content { get; set; }
        public virtual UserProfile User { get; set; }
    }

    [Table("IsLikeComment")]
    public class IsLikeComment
    {
        public IsLikeComment()
        {

        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int IsLikeID { get; set; }
        
        [ForeignKey("Comment")]
        public int CommentID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public bool? IsLiked { get; set; }
        public bool? IsUnliked { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual UserProfile User { get; set; }
    }

    [Table("webpages_Membership")]
    public class webpages_Membership
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ConfirmationToken { get; set; }
        public Nullable<bool> IsConfirmed { get; set; }
        public Nullable<System.DateTime> LastPasswordFailureDate { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> PasswordChangedDate { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordVerificationToken { get; set; }
        public Nullable<System.DateTime> PasswordVerificationTokenExpirationDate { get; set; }
        
    }

    [Table("webpages_OAuthMembership")]
    public class webpages_OAuthMembership
    {
        [Key]
        [Column(Order=1)]
        public string Provider { get; set; }
        [Key]
        [Column(Order = 2)]
        public string ProviderUserId { get; set; } 
        public int UserId { get; set; }
    }

    [Table("webpages_Roles")]
    public class webpages_Roles
    {
        public webpages_Roles()
        {
            this.UserProfiles = new HashSet<UserProfile>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }

    [Table("webpages_UsersInRoles")]
    public class webpages_UsersInRoles 
    {
        [Key,ForeignKey("User")]
        [Column(Order=1)]
        public int UserId { get; set; }

        [Key,ForeignKey("Role")]
        [Column(Order=2)]
        public int RoleId { get; set; }

        public virtual UserProfile User { get; set; }
        public virtual webpages_Roles Role { get; set; }
    }
}