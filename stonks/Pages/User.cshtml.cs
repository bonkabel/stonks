using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stonks.Data;
using stonks.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Pages
{
    public class UserModel : PageModel
    {

        private readonly ApplicationDbContext db;

        private User thisUser;

        /// <summary>
        /// The number of forum posts
        /// </summary>
        public int numForumPosts;

        /// <summary>
        /// The users profile
        /// </summary>
        public User ThisUser
        {
            get { return thisUser; }
            set { thisUser = value; }
        }

        public UserModel(ApplicationDbContext db)
        {
            this.db = db;
			
        }


        public void OnGet(Guid? id)
        {
            if (id != null)
            {
				ThisUser = db.Users.FirstOrDefault(u => u.Id == id.ToString());
                if (ThisUser != null)
                {
					Post[] posts = db.Posts.Where(p => p.UserId == new Guid(ThisUser.Id)).ToArray();
					PostReply[] postReplies = db.PostReplies.Where(pr => pr.UserId == new Guid(ThisUser.Id)).ToArray();
					numForumPosts = posts.Length + postReplies.Length;
				}
			}
		}

        public async Task<IActionResult> OnPostReportUser(string userId)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == userId);
            ReportedUser reportedUser = db.ReportedUsers.FirstOrDefault(ru => ru.UserId == new Guid(user.Id));

            if (user != null && reportedUser == null)
            {
                db.ReportedUsers.Add(new ReportedUser(Guid.NewGuid(), new Guid(user.Id)));
                await db.SaveChangesAsync();
            }


            return null;
        }
    }
}
