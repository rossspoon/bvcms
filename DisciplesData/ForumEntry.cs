using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web.Security;
using System.Linq;
using UtilityExtensions;

namespace DiscData
{
    public partial class ForumEntry
    {
        public bool IsOwner
        {
            get { return this.CreatedBy == DbUtil.Db.CurrentUser.UserId; }
        }
        public bool IsRead(User user)
        {
            return DbUtil.Db.ForumUserReads
                .Count(r => r.ForumEntryId == Id && r.UserId == user.UserId) > 0;
        }
        public void ShowAsRead(User user)
        {
            if (!IsRead(user))
            {
                var r = new ForumUserRead();
                r.UserId = user.UserId;
                r.CreatedOn = Util.Now;
                this.ForumUserReads.Add(r);
            }
        }
        public static ForumEntry LoadById(int id)
        {
            return DbUtil.Db.ForumEntries.SingleOrDefault(fe => fe.Id == id);
        }
        public ForumEntry NewReply(string title, string entry)
        {
            return DbUtil.Db.ForumNewEntry(ForumId, Id, title, entry,
                Util.Now, HttpContext.Current.User.Identity.Name).Single();
        }
        public ICollection<MailAddress> GetNotificationList()
        {
            var mp = Membership.Providers["AdminMembershipProvider"];
            var list = new Dictionary<string, MailAddress>();
            foreach (var mu in Group.GetUsersInRole("Administrator"))
            {
                if (mu.NotifyAll ?? true)
                    list.Add(mu.Username, new MailAddress(mu.EmailAddress,
                        mu.FirstName + " " + mu.LastName));
            }
            foreach (User mu in Group.GetUsersInGroup(Forum.GroupId.Value))
                if (!list.ContainsKey(mu.Username))
                {
                    if (mu.NotifyAll ?? true)
                        list.Add(mu.Username, new MailAddress(mu.EmailAddress,
                            mu.FirstName + " " + mu.LastName));
                }
            foreach (var fn in ForumNotifications)
                if (!list.ContainsKey(fn.UserId))
                {
                    var u = DbUtil.Db.GetUser(fn.UserId);
                    list.Add(fn.UserId, new MailAddress(u.EmailAddress,
                        u.FirstName + " " + u.LastName));
                }
            if (Util.IsLocalNetworkRequest)
            {
                list.Clear();
                list.Add("davcar", new MailAddress("davcar@pobox.com", "David Carroll"));
            }
            return list.Values;
        }

    }
    public partial class ForumEntryController
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<ForumEntry> GetTopLevelEntriesForForum(int ForumId)
        {
            return from f in DbUtil.Db.ForumEntries
                   where f.ForumId == ForumId && !f.ReplyToId.HasValue
                   orderby f.CreatedOn descending
                   select f;
        }
    }
}