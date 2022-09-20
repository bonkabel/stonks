using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stonks.Data;
using stonks.Models;

namespace stonks.Pages
{
    public class CreatePostModel : PageModel
    {
        ApplicationDbContext db;

        private PostInputModel input;

        /// <summary>
        /// 
        /// </summary>
        [BindProperty]
        public PostInputModel Input
        {
            get { return input; }
            set { input = value; }
        }

        /// <summary>
        /// The PostReply input model.  This is for verifying the user input
        /// </summary>
        public class PostInputModel
        {
            [Required(ErrorMessage = "Please enter a title between 5 - 200 characters")]
            [StringLength(200, MinimumLength = 5)]
            public string Title { get; set; }

            [Required(ErrorMessage = "The tags field accepts at most 200 characters")]
            [StringLength(200)]
            public string Tags { get; set; }

            [Required(ErrorMessage = "The text has a maximum of 10000 characters")]
            [StringLength(10000)]
            public string Text { get; set; }

        }

        public CreatePostModel(ApplicationDbContext db)
        {
            this.db = db;
        }


        public void OnGet()
        {
        }

        /// <summary>
        /// Submits the PostReply to the database
        /// </summary>
        /// <param name="id">The id of the Post</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (this.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {

                Post post = new Post(new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value), Input.Title, Input.Text);
                db.Posts.Add(post);

                string[] stringTags = ParseTags();

                // Go through each tag and add it to the Tags and PostTags database
                foreach (string stringTag in stringTags)
                {

                    Tag tagObject = db.Tags.FirstOrDefault(t => t.Text == stringTag);

                    if (tagObject == null) {
                        tagObject = new Tag(Guid.NewGuid(), stringTag);
                        db.Tags.Add(tagObject);
                    }

                    db.PostTags.Add(new PostTag(tagObject.TagId, post.PostId));
                }

                await db.SaveChangesAsync();
            }
            return null;
        }


        private string[] ParseTags()
        {
            string[] allTags = null;
            List<string> parsedTags = null;

            if (Input.Tags != null && Input.Tags.Length > 0)
            {
                allTags = Input.Tags.Split(",");
                parsedTags = new List<string>();

                for (int i = 0; i < allTags.Length; i++)
                {
                    Console.WriteLine(allTags[i]);
                    allTags[i] = allTags[i].Trim();
                    Console.WriteLine(allTags[i]);
                    if (allTags[i] != "" && !parsedTags.Contains(allTags[i]))
                    {
                        parsedTags.Add(allTags[i]);
                    }
                }
            }

            return parsedTags.ToArray();
        }
    }
}
