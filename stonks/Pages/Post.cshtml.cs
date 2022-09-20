using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stonks.Classes;
using stonks.Data;
using stonks.Models;

namespace stonks.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class PostModel : PageModel
    {
        /// <summary>
        /// The database context
        /// </summary>
        private readonly ApplicationDbContext db;

        private Post post;

        /// <summary>
        /// The Post itself
        /// </summary>
        public Post Post
        {
            get { return post; }
            set { post = value; }
        }

        private PostReply[] replies;

        /// <summary>
        /// The replies to the post
        /// </summary>
        public PostReply[] Replies
        {
            get { return replies; }
            set { replies = value; }
        }

        private Tag[] tags;

        /// <summary>
        /// The tags of the post
        /// </summary>
        public Tag[] Tags
        {
            get { return tags; }
            set { tags = value; }
        }

        private User postUser;

        /// <summary>
        /// The user that made the post
        /// </summary>
        public User PostUser
        {
            get { return postUser; }
            set { postUser = value; }
        }

        private Dictionary<PostReply, User> replyUsers;

        public Dictionary<PostReply, User> ReplyUsers
        {
            get { return replyUsers; }
            set { replyUsers = value; }
        }

        private PostReplyInputModel input;

        /// <summary>
        /// The users PostReply input
        /// </summary>
        [BindProperty]
        public PostReplyInputModel Input
        {
            get { return input; }
            set { input = value; }
        }


        /// <summary>
        /// The PostReply input model.  This is for verifying the user input
        /// </summary>
        public class PostReplyInputModel
        {
            [Required(ErrorMessage = "Please enter text")]
            [StringLength(10000, MinimumLength = 1)]
            public string Text { get; set; }
        }

        /// <summary>
        /// Constructor for setting db
        /// </summary>
        /// <param name="db">The database object</param>
        public PostModel(ApplicationDbContext db)
        {
            this.db = db;
        }


        /// <summary>
        /// On get, do the setup required for the page.
        /// </summary>
        /// <param name="id">The id of the Post</param>
        public void OnGet(Guid id)
        {
            if (id != Guid.Empty)
            {
                Setup(id);
            }
        }

        /// <summary>
        /// Submits the PostReply to the database
        /// </summary>
        /// <param name="id">The id of the Post</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            Setup(id);

            if (CheckFormData())
            {
                db.PostReplies.Add(new PostReply(Guid.NewGuid(), id, new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value), Input.Text, DateTime.Now));
            }

            await db.SaveChangesAsync();

            return RedirectToAction("");
        }

        /// <summary>
        /// Confirms the Input is valid
        /// </summary>
        /// <returns>Whether or no the Input is valid</returns>
        private bool CheckFormData()
        {
            bool valid = true;

            if (Input.Text == null || Input.Text.Length <= 0)
            {
                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Sets up the fields.
        /// </summary>
        /// <param name="id">The id of the Post</param>
        private void Setup(Guid id)
        {
            Post = db.Posts.First(p => p.PostId == id);

            Replies = db.PostReplies.Where(pr => pr.PostId == Post.PostId).OrderBy(pr => pr.Date).ToArray();

            Tags = db.Tags.Where(t => db.PostTags.Any(pt => pt.TagId == t.TagId && pt.PostId == Post.PostId)).ToArray();

            PostUser = db.Users.First(u => u.Id == Post.UserId.ToString());

            ReplyUsers = new Dictionary<PostReply, User>();

            // Needed to create the ReplyUsers dictionary.
            // This is done in a convoluted way as linq has issues with doing it simply for some reason.
            User[] allUsers = db.Users.ToArray();
            foreach (User user in allUsers)
            {
                PostReply[] postReplies = Replies.Where(r => r.UserId.ToString() == user.Id).ToArray();

                if (postReplies != null && postReplies.Length > 0)
                {
                    foreach (PostReply postReply in postReplies)
                    {
                        replyUsers.Add(postReply, user);
                    }
                 
                }
                
            }
        }
    }
}
