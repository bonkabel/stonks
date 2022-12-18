using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stonks.Models;

namespace stonks.Pages
{
    public class _ForumPostPreviewModel : PageModel
    {

        private Guid postId;

        /// <summary>
        /// The unique id of the Post
        /// </summary>
        public Guid PostId
        {
            get { return postId; }
            set { postId = value; }
        }

        private Guid userId;

        /// <summary>
        /// The user that made the post
        /// </summary>
        public Guid UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string title;

        /// <summary>
        /// The title of the Post
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string username;

        /// <summary>
        /// The username that posted the Post
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private int numReplies;

        /// <summary>
        /// The number of replies to the Post
        /// </summary>
        public int NumReplies
        {
            get { return numReplies; }
            set { numReplies = value; }
        }

        private Tag[] tags;

        /// <summary>
        /// The tags of the Post
        /// </summary>
        public Tag[] Tags
        {
            get { return tags; }
            set { tags = value; }
        }

        private DateTime datePosted;

        /// <summary>
        /// The date the Post was posted
        /// </summary>
        public DateTime DatePosted
        {
            get { return datePosted; }
            set { datePosted = value; }
        }


        /// <summary>
        /// Empty constructor.
        /// </summary>
        public _ForumPostPreviewModel()
        {

        }

        /// <summary>
        /// Full constructor.
        /// </summary>
        /// <param name="postId">The id of the post</param>
        /// <param name="title">The title of the post</param>
        /// <param name="username">The username who posted the post</param>
        /// <param name="numReplies">The number of replies to the post</param>
        /// <param name="tags">The tags of the post</param>
        /// <param name="datePosted">The date the post was posted</param>
        public _ForumPostPreviewModel(Guid postId, string title, string username, int numReplies, Tag[] tags, DateTime datePosted, Guid userId)
        {
            PostId = postId;
            UserId = userId;
            Title = title;
            Username = username;
            NumReplies = numReplies;
            DatePosted = datePosted;
            Tags = tags;
        }

        public void OnGet()
        {

        }
    }
}
