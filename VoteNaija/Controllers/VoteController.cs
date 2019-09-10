using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VoteNaija.Models;
using WebMatrix.WebData;

namespace VoteNaija.Controllers
{
    public class VoteController : Controller
    {
        public ActionResult Contact()
        {
            ViewBag.Title = "Contact Us";
            return View();
        }
        public ActionResult Policy()
        {
            ViewBag.Title = "Privacy Policy";
            return View();
        }
        public ActionResult TermsAndConditions()
        {
            ViewBag.Title = "Terms and Conditions";
            return View();
        }
        public ActionResult Home()
        {
            var content = ArticleManagement.GetRandomVoteContent();
            if (content != null)
            {
                ViewBag.Title = content.Title;
                ViewBag.contentId = content.ContentID;
                ViewBag.groupId = content.GroupID;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Result(int page, string voteGroup, int groupId)
        {
            List<string> list = new List<string>() { "Article", "News", "Vote" };
            if (list.Contains(voteGroup))
            {
                return RedirectToAction("GroupResultPost", new {page = page, voteGroup = voteGroup });
            }
            else
            {
                return RedirectToAction("SubGroupResultPost", new { page = page, voteGroup = voteGroup, groupId = groupId });
            }
        }

        [HttpGet]
        public ActionResult GroupResult(string voteGroup)
        {
            ViewBag.Title = voteGroup;
            ResultModel model = new ResultModel() { Contents = ArticleManagement.ListGroupTitles(0, voteGroup), Page=0, voteGroup = voteGroup };
            return View("Result",model);
        }
        [HttpGet]
        public ActionResult GroupResultPost(int page, string voteGroup)
        {
            ViewBag.Message = "We dont have more Entries";
            ViewBag.Title = voteGroup;
            var contents = ArticleManagement.ListGroupTitles(page, voteGroup);
            if (contents.Count == 0) 
            {
                return View("EndOfArticle");
            }
            var model = new ResultModel() { Contents = contents, Page = page, voteGroup = voteGroup };
            return View("Result", model);
        }

        [HttpGet]
        public ActionResult SubGroupResult(string voteGroup, int groupId)
        {
            ViewBag.Title = voteGroup;
            ResultModel model = new ResultModel() { Contents = ArticleManagement.ListSubGroupTitles(0, groupId), Page = 0, groupId = groupId, voteGroup = voteGroup };
            return View("Result", model);
        }
        [HttpGet]
        public ActionResult SubGroupResultPost(int page, string voteGroup, int groupId)
        {
            ViewBag.Message = "We dont have more Entries";
            ViewBag.Title = voteGroup;
            var contents = ArticleManagement.ListSubGroupTitles(page, groupId);
            if (contents.Count == 0)
            {
                return View("EndOfArticle");
            }
            var model = new ResultModel() { Contents = contents, Page = page, voteGroup = voteGroup,groupId = groupId };
            return View("Result", model);
        }

        [HttpGet]
        public ActionResult Page(int contentId, int groupId)
        {
            var model = ArticleManagement.FindArticle(contentId, Math.Abs(WebSecurity.CurrentUserId), groupId);
            ViewBag.Message = ArticleManagement.GetVoteGroup(groupId);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult VotingPage(int contentId, string voteRadioButton, int groupId) 
        {
            ArticleManagement.AddVote(Convert.ToInt32(voteRadioButton), WebSecurity.CurrentUserId, contentId);
            return RedirectToAction("Page", new { contentId = contentId, groupId = groupId });
        }
        [HttpPost]
        public ActionResult PostArticles(ManageModel model) 
        {
            ArticleManagement.PostArticles(model);
            return PartialView("_AdminPartial");
        }
        public ActionResult SubGroups(string group)
        {
            return Json(ArticleManagement.GetGroups(group).Select(x => new {value = x.GroupID, text = x.SubGroupName }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTitles(string group)
        {
            return Json(ArticleManagement.ListAllSubGroupTitles(Convert.ToInt32(group)).Select(x => new {value = x.ContentID, text = x.Title }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddGroup(ManageModel model)
        {
            ArticleManagement.AddGroup(model);
            return PartialView("_AdminPartial");
        }
        [HttpPost]
        public ActionResult PostVoteSections(ManageModel model, IEnumerable<VotingModel> voteSections)
        {
            ArticleManagement.PostVoteSections(model, voteSections);
            return PartialView("_AdminPartial");
        }
        public ActionResult DeleteContent(ManageModel model)
        {
            ArticleManagement.DeleteContent(model);
            return PartialView("_AdminPartial");
        }
        public ActionResult DeleteGroup(ManageModel model)
        {
            ArticleManagement.DeleteGroup(model);
            return PartialView("_AdminPartial");
        }
        public ActionResult AddToRole(ManageModel model)
        {
            Roles.AddUserToRole(model.UserName, model.Role);
            return PartialView("_AdminPartial");
        }
        public ActionResult RemoveFromRole(ManageModel model)
        {
            Roles.RemoveUserFromRole(model.UserName, model.Role);
            return PartialView("_AdminPartial");
        }
        public ActionResult DeleteUser(ManageModel model)
        {
            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(model.UserName);
            ((SimpleMembershipProvider)Membership.Provider).DeleteUser(model.UserName, true);
            return PartialView("_AdminPartial");
        }
    }
}