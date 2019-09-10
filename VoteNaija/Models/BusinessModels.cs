using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Web.Security;
using System.Threading;

namespace VoteNaija.Models
{
    public class ArticleManagement
    {
        public static List<Group> GetGroups(string groupName) 
        {
            List<Group> groups = new List<Group>();
            using (VoteNaijaDBContext db = new VoteNaijaDBContext())
            {
                try
                {
                    groups = (from x in db.Groups
                              where x.GroupName == groupName
                              select x).ToList();
                }
                catch { }
            }
            return groups;
        }

        static Random random = new Random();
        public static Content GetRandomVoteContent() 
        {
            Content result = new Content();
            using (VoteNaijaDBContext db = new VoteNaijaDBContext())
            {
                try
                {
                    var data = (from x in db.Contents
                               join z in db.Groups on x.GroupID equals z.GroupID
                               where z.IsPoll == true
                               select x).ToList();

                    result = data[random.Next(data.Count())];
                }
                catch { }
            }
            return result;
        }
        public static List<Content> ListGroupTitles(int page, string votegroup, int multiply = 10) 
        {
            VoteNaijaDBContext db = new VoteNaijaDBContext();
            List<Content> result = new List<Content>();
            try
            {
                var data = (from c in db.Contents
                            join r in db.Groups on c.GroupID equals r.GroupID
                            where r.GroupName == votegroup
                            select c).OrderByDescending(c => c.Date).ToList();
                var total = data.Count();

                for (int i = (page * multiply); i < (page * multiply + multiply); i++)
                {
                    if (i < total)
                    {
                        result.Add(data[i]);
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            catch { }
            return result;
        }
        public static List<Content> ListAllSubGroupTitles(int groupId) 
        {
            VoteNaijaDBContext db = new VoteNaijaDBContext();
            try
            {
                var data = (from c in db.Contents
                            join r in db.Groups on c.GroupID equals r.GroupID
                            where r.GroupID == groupId
                            select c).OrderByDescending(c => c.Date).ToList();
                return data;
            }
            catch { }
            return null;
        }
        public static List<Content> ListSubGroupTitles(int page, int groupId, int multiply = 10)
        {
            List<Content> result = new List<Content>();
            try
            {
                var data = ArticleManagement.ListAllSubGroupTitles(groupId);
                var total = data.Count();
                for (int i = (page * multiply); i < (page * multiply + multiply); i++)
                {
                    if (i < total)
                    {
                        result.Add(data[i]);
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            catch { }
            return result;
        }

        public static PageModel FindArticle(int contentId, int UserId, int groupId)
        {
            VoteNaijaDBContext db = new VoteNaijaDBContext();
            
            var data = (from c in db.Contents
                    where c.ContentID == contentId
                    select c).First();

            var data2 = (from com in db.Comments
                         join c in db.Contents on com.ContentID equals c.ContentID
                         where c.ContentID == contentId
                         select com).ToList();
            
            if (CheckWhetherIsPoll(groupId))
            {
                List<VoteSection> list = new List<VoteSection>();
                int sum = 0;
                ArticleManagement.VoteResult(contentId, out list, out sum);
               return new PageModel { content = data,groupId=groupId, voteSections = list, totalVotes = sum, RemDays = data.Days-(DateTime.Now-data.Date).Days };
            }
            return new PageModel { content = data, groupId=groupId };
        }
        public static void VoteResult(int contentId, out List<VoteSection> list, out int sum) 
        {
            VoteNaijaDBContext db = new VoteNaijaDBContext();
            try
            {
                list = (from v in db.VoteSections
                        join c in db.Contents on v.ContentID equals c.ContentID
                        where c.ContentID == contentId
                        select v).ToList();
                sum = (from v in db.VoteSections
                       join c in db.Contents on v.ContentID equals c.ContentID
                       where c.ContentID == contentId
                       select v.Votes).Sum();
            }
            catch(Exception ex)
            {
                list = new List<VoteSection>();
                sum = 0;
            }
        }

        public static bool CheckWhetherIsPoll(int groupId) 
        {
            bool data = false;
            using (VoteNaijaDBContext db = new VoteNaijaDBContext())
            {
                data = (from g in db.Groups
                        where g.GroupID == groupId
                        select g.IsPoll).FirstOrDefault();
            }
            return data;
        }
        internal static void AddVote(int voteId, int UserId, int contentId)
        {
            VoteNaijaDBContext db = new VoteNaijaDBContext();
            var data = from v in db.VoteSections
                       where v.VoteSectionID == voteId
                       select v;

            var data2 = from v in db.Contents
                        where v.ContentID == contentId
                        select v;

            foreach (var d in data) 
            {
                d.Votes++;
            }
            
            foreach (var d in data2)
            {
                List<int> x = new List<int>();
                if (d.UserIDs != null)
                   x = d.UserIDs.DeserializeList<int>();
                x.Add(UserId);
                d.UserIDs = x.SerializeList();
            }

            db.SaveChanges();
        }

        internal static void PostArticles(ManageModel model)
        {
            VoteNaijaDBContext db = new VoteNaijaDBContext();
            var file = model.File;
            if (file != null) 
            {
                var ImageName = Path.GetFileName(file.FileName);
                string physicalPath = HttpContext.Current.Server.MapPath("~/Images/" + ImageName);
                file.SaveAs(physicalPath);

                var groupId = Convert.ToInt32(model.subGroupName);
                db.Contents.Add(new Content { Authur = model.Author, Date = DateTime.Now, Title = model.Title, ContentResult = model.Content, GroupID = groupId, ImageUrl = ImageName, Summary = model.Summary});

                db.SaveChanges();
            }
            else if (model != null)
            {
                var groupId = Convert.ToInt32(model.subGroupName);
                db.Contents.Add(new Content { Authur = model.Author, Date = DateTime.Now, Title = model.Title, ContentResult = model.Content, GroupID = groupId, Summary = model.Summary});

                db.SaveChanges();
            }
            
            
        }

        internal static void AddGroup(ManageModel model)
        {
            if (model != null) 
            {
                using (VoteNaijaDBContext db = new VoteNaijaDBContext()) 
                {
                    if (model.groupName == "Vote")
                    {
                        db.Groups.Add(new Group { GroupName = model.groupName, SubGroupName = model.subGroupName, IsPoll = true });
                    }
                    else 
                    {
                        db.Groups.Add(new Group { GroupName = model.groupName, SubGroupName = model.subGroupName, IsPoll = false });
                    }
                    db.SaveChanges();
                }
            }
        }

        internal static void PostVoteSections(ManageModel model, IEnumerable<VotingModel> voteSections)
        {
            using (VoteNaijaDBContext db = new VoteNaijaDBContext()) 
            {
                if (model != null)
                {
                    var groupId = Convert.ToInt32(model.subGroupName);

                    Content content1 = new Content { Date = DateTime.Now, Title = model.Title, GroupID = groupId, Days = model.Days};
                    if (voteSections != null)
                    {
                        foreach (var m in voteSections)
                        {
                            if (m.File != null)
                            {
                                var ImageName = Path.GetFileName(m.File.FileName);
                                string physicalPath = HttpContext.Current.Server.MapPath("~/Images/" + ImageName);
                                m.File.SaveAs(physicalPath);
                                db.VoteSections.Add(new VoteSection { Content = content1, Section = m.voteTitle, ImageUrl = ImageName });
                            }
                            else
                            {
                                db.VoteSections.Add(new VoteSection { Content = content1, Section = m.voteTitle });
                            }
                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        internal static void DeleteContent(ManageModel model)
        {
            using (VoteNaijaDBContext db = new VoteNaijaDBContext())
            {
                try
                {
                    var id = Convert.ToInt32(model.Title);
                    var data = from x in db.Contents
                               where x.ContentID == id
                               select x;
                    try
                    {
                        var data1 = from x in db.VoteSections
                                    where x.ContentID == id
                                    select x;
                        foreach (var z in data1)
                            db.VoteSections.Remove(z);
                        db.SaveChanges();
                    }
                    catch { }

                    foreach (var y in data)
                    {
                        db.Contents.Remove(y);
                    }
                    db.SaveChanges();
                }
                catch
                {
                   
                }
            }
        }

        internal static void DeleteGroup(ManageModel model)
        {
            using (VoteNaijaDBContext db = new VoteNaijaDBContext()) 
            {
                try
                {
                    var id = Convert.ToInt32(model.subGroupName);
                    var data = from x in db.Groups
                               where x.GroupID == id
                               select x;
                    try
                    {
                        var data2 = from x in db.Contents
                                    where x.GroupID == id
                                    select x;
                        foreach (var u in data2)
                        {
                            try
                            {
                                var data1 = from x in db.VoteSections
                                            where x.ContentID == u.ContentID
                                            select x;
                                foreach (var z in data1)
                                    db.VoteSections.Remove(z);
                                db.SaveChanges();
                            }
                            catch { }
                            db.Contents.Remove(u);
                        }
                        db.SaveChanges();
                    }
                    catch { }
                    foreach (var y in data)
                    {
                        db.Groups.Remove(y);
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        internal static List<string> GetRoles()
        {
            return Roles.GetAllRoles().ToList();
        }

        internal static List<string> GetUsers()
        {
            VoteNaijaDBContext db = new VoteNaijaDBContext();
            var data = from x in db.UserProfiles
                       select x.UserName;
            return data.ToList();
        }

        internal static string GetVoteGroup(int groupId)
        {
            VoteNaijaDBContext db = new VoteNaijaDBContext();
            var votegroup = (from x in db.Groups
                            where x.GroupID == groupId
                            select x.SubGroupName).First();
            return votegroup;
        }
    }
    public static class Converter 
    {
        public static byte[] SerializeList<T>(this List<T> list) 
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, list);
            ms.Position = 0;
            byte[] serializedList = new byte[ms.Length];
            ms.Read(serializedList, 0, (int)ms.Length);
            ms.Close();
            return serializedList;
        }
        public static List<T> DeserializeList<T>(this byte[] data) 
        {
            try 
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(data, 0, data.Length);
                ms.Position = 0;
                BinaryFormatter bf = new BinaryFormatter();
                List<T> list = bf.Deserialize(ms) as List<T>;
                return list;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }
    }
}