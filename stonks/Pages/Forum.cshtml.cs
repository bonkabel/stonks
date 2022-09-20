using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using stonks.Data;
using stonks.Models;

namespace stonks.Pages
{
    public class ForumModel : PageModel
    {
        /// <summary>
        /// The database context
        /// </summary>
        private readonly ApplicationDbContext db;

        private _ForumPostPreviewModel[] models;

        public _ForumPostPreviewModel[] Models
        {
            get { return models; }
            set { models = value; }
        }

        public ForumModel(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void OnGet()
        {
            Setup();
        }

        

        /// <summary>
        /// Sets up fields
        /// </summary>
        private void Setup()
        {
            // All of the posts that exist
            Post[] posts = db.Posts.ToArray();
            // Initializing to the length of posts
            Models = new _ForumPostPreviewModel[posts.Length];

            // This is needed as usernames are in a seperate table
            Dictionary<Post, User> userDict = new Dictionary<Post, User>();
            // This is needed as tags are in a seperate table
            Dictionary<Post, Tag[]> tagDict = new Dictionary<Post, Tag[]>();
            // Getting the relevant users
            List<User> relevantUsers = new List<User>();
            User[] allUsers = db.Users.ToArray();

            foreach(User user in allUsers)
            {
                if (posts.Any(p => p.UserId.ToString() == user.Id))
                {
                    relevantUsers.Add(user);
                }
            }


            // Adding them to the dictionary
            foreach(Post post in posts)
            {
                userDict.Add(post, relevantUsers.First(u => new Guid(u.Id) == post.UserId));

                // this is absolute nonesense i need to start using the sql syntax thing
                // what this does (in theory) is that it connects the post table to the posttags table to the tags table
                // and gets all the names of the tags that 
                tagDict.Add(post, db.Tags.Where(t => t.TagId == db.PostTags.First(pt => pt.TagId == t.TagId && post.PostId == pt.PostId).TagId).ToArray());

            }

            for (int i = 0; i < posts.Length; i++)
            {
                // Creating the PartialViewResult
                _ForumPostPreviewModel model = new _ForumPostPreviewModel(posts[i].PostId, posts[i].Title, userDict[posts[i]].UserName, posts[i].NumberReplies, tagDict[posts[i]], posts[i].Date);
                

                Models[i] = model;
            }
        }
    }
}
