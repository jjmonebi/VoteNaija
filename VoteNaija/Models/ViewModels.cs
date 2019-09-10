using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VoteNaija.Models
{
    public class ResultModel
    {
        public int Page { get; set; }
        public string voteGroup { get; set; }
        public List<Content> Contents { get; set; }
        public int groupId { get; set; }
    }

    public class PageModel 
    {
        public Content content { get; set; }
        public List<VoteSection> voteSections { get; set; }
        public int groupId { get; set; }
        public int UserId { get; set; }
        public int totalVotes { get; set; }
        public int RemDays { get; set; }
    }
    public class VoteModel 
    {
        public List<VoteSection> voteSections { get; set; }
        public int totalVotes { get; set; }
        public int groupId { get; set; }
        public List<int> UserIds { get; set; }
        public int contentId { get; set; }
        public int voteId { get; set; }
        public int RemDays { get; set; }
    }

    public class ManageModel 
    {
        [Display(Name = "Group:")]
        public string groupName { get; set; }
        [Required(ErrorMessage = "You must have a title")]
        [Display(Name = "Title:")]
        public string Title { get; set; }
        [Required(ErrorMessage = "You must have an author")]
        [Display(Name = "Author:")]
        public string Author { get; set; }
        [Required(ErrorMessage = "You must have a summary")]
        [Display(Name = "Summary:")]
        public string Summary { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "You must have a content")]
        [Display(Name = "Content:")]
        public string Content { get; set; }
        [Required(ErrorMessage = "You must upload an image")]
        [Display(Name = "Image:")]
        public HttpPostedFileBase File { get; set; }
        [Required(ErrorMessage = "You must have a sub group")]
        [Display(Name = "Sub Group:")]
        public string subGroupName { get; set; }
        [Required(ErrorMessage = "You must pick the number of days for the poll to run")]
        public int Days { get; set; }


        public string UserName { get; set; }
        public string Role { get; set; }
    }
    public class VotingModel 
    {
        [Required(ErrorMessage = "You must upload an image")]
        [Display(Name = "Image:")]
        public HttpPostedFileBase File { get; set; }
        [Required(ErrorMessage = "You provide a vote title")]
        [Display(Name = "Vote Title:")]
        public string voteTitle { get; set; }
    }
}