using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using stonks.Data;
using stonks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace stonks.Pages
{
    public class ReportsModel : PageModel
    {

        public enum Type { POST, POSTREPLY, MESSAGE, MESSAGEREPLY, USER };

        public Type? contentType;

        /// <summary>
        /// The possible states that the reports page can be in
        /// Used to switch between what reported content is displayed
        /// </summary>
        public Type? ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        private readonly ApplicationDbContext db;

        private User currentUser;

        /// <summary>
        /// The users that viewing this page
        /// </summary>
        public User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }

        private Object[] reportedContent;

        /// <summary>
        /// 
        /// </summary>
        public Object[] ReportedContent
        {
            get { return reportedContent; }
            set { reportedContent = value; }
        }


		private Dictionary<PostReply, User> postReplyUsers;

		public Dictionary<PostReply, User> PostReplyUsers
		{
			get { return postReplyUsers; }
			set { postReplyUsers = value; }
		}

		private Dictionary<MessageReply, User> messageReplyUsers;

		public Dictionary<MessageReply, User> MessageReplyUsers
		{
			get { return messageReplyUsers; }
			set { messageReplyUsers = value; }
		}


		private PostReply[] postReplies;

        public PostReply[] PostReplies
        {
            get { return postReplies; }
            set { postReplies = value; }
        }

		private MessageReply[] messageReplies;

		public MessageReply[] MessageReplies
		{
			get { return messageReplies; }
			set { messageReplies = value; }
		}

		private User[] users;

		public User[] Users
		{
			get { return users; }
			set { users = value; }
		}




		public ReportsModel(ApplicationDbContext db)
        {
            this.db = db;
        }


        public void OnGet(Type? id)
        {

            Setup(id);
        }

        private void Setup(Type? reportedContent)
        {
            ContentType = reportedContent;
            if (this.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                Guid userId = new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                CurrentUser = db.Users.First(u => u.Id == userId.ToString());

                if (CurrentUser.IsAdmin)
                {
                    if (ContentType == null || ContentType == Type.POST)
                    {
                        SetupPosts();
                    }
                    else if (ContentType == Type.POSTREPLY)
                    {
                        SetupPostReplies();
                    }
                    else if (ContentType == Type.MESSAGE)
                    {
                        SetupMessages();
                    }
					else if (ContentType == Type.MESSAGEREPLY)
					{
						SetupMessageReplies();
					}
					else if (ContentType == Type.USER)
					{
						SetupUsers();
					}
                }
			}
		}

        

        private void SetupMessages()
        {
			Guid[] rpIds = db.ReportedMessages.Select(rp => rp.MessageId).ToArray();
            Message[] messages = db.Messages.Where(m => rpIds.Contains(m.MessageId)).ToArray();
			ReportedContent = new _MessagePreviewModel[messages.Length];

			// Getting the relevant users
			List<User> relevantUsers = new List<User>();
			User[] allUsers = db.Users.ToArray();

			// This is needed as usernames are in a seperate table
			Dictionary<Message, User> userDict = new Dictionary<Message, User>();

			foreach (User user in allUsers)
			{
				if (messages.Any(m => m.SendorId.ToString() == user.Id))
				{
					relevantUsers.Add(user);
				}
			}

			foreach (Message message in messages)
			{
				userDict.Add(message, relevantUsers.First(u => new Guid(u.Id) == message.SendorId));
			}

			for (int i = 0; i < messages.Length; i++)
			{
				// Creating the PartialViewResult
				_MessagePreviewModel model = new _MessagePreviewModel(messages[i].MessageId, messages[i].Title, userDict[messages[i]].UserName, messages[i].Date);

				ReportedContent[i] = (object)model;
			}
		}

		private void SetupMessageReplies()
		{
			Guid[] rpIds = db.ReportedMessageReplies.Select(rmr => rmr.ReplyId).ToArray();
			messageReplies = db.MessageReplies.Where(p => rpIds.Contains(p.ReplyId)).ToArray();
			MessageReplyUsers = new Dictionary<MessageReply, User>();

			// Needed to create the ReplyUsers dictionary.
			// This is done in a convoluted way as linq has issues with doing it simply for some reason.
			User[] allUsers = db.Users.ToArray();
			foreach (User user in allUsers)
			{
				MessageReply[] userMessageReplies = messageReplies.Where(r => r.UserId.ToString() == user.Id).ToArray();

				if (userMessageReplies != null && userMessageReplies.Length > 0)
				{
					foreach (MessageReply postReply in userMessageReplies)
					{
						MessageReplyUsers.Add(postReply, user);
					}

				}

			}
		}

		private void SetupPostReplies()
        {
			Guid[] rpIds = db.ReportedReplyPosts.Select(rp => rp.ReplyPostId).ToArray();
			PostReplies = db.PostReplies.Where(p => rpIds.Contains(p.ReplyId)).ToArray();
            PostReplyUsers = new Dictionary<PostReply, User>();

			// Needed to create the ReplyUsers dictionary.
			// This is done in a convoluted way as linq has issues with doing it simply for some reason.
			User[] allUsers = db.Users.ToArray();
			foreach (User user in allUsers)
			{
				PostReply[] userPostReplies = PostReplies.Where(r => r.UserId.ToString() == user.Id).ToArray();

				if (userPostReplies != null && userPostReplies.Length > 0)
				{
					foreach (PostReply postReply in userPostReplies)
					{
						PostReplyUsers.Add(postReply, user);
					}

				}

			}

		}

        private void SetupUsers()
        {
            Guid[] rpIdsTemp = db.ReportedUsers.Select(ru => ru.UserId).ToArray();

			String[] rpIds = new string[rpIdsTemp.Length];

			for (int i = 0; i < rpIdsTemp.Length; i++)
			{
				rpIds[i] = rpIdsTemp[i].ToString();
			}
            Users = db.Users.Where(u => rpIds.Contains(u.Id)).ToArray();

        }

        private void SetupPosts()
        {
            Guid[] rpIds = db.ReportedPosts.Select(rp => rp.PostId).ToArray();
            Post[] posts = db.Posts.Where(p => rpIds.Contains(p.PostId)).ToArray();
            ReportedContent = new _ForumPostPreviewModel[posts.Length];

			// This is needed as usernames are in a seperate table
			Dictionary<Post, User> userDict = new Dictionary<Post, User>();
			// This is needed as tags are in a seperate table
			Dictionary<Post, Tag[]> tagDict = new Dictionary<Post, Tag[]>();
			// Getting the relevant users
			List<User> relevantUsers = new List<User>();
			User[] allUsers = db.Users.ToArray();

			foreach (User user in allUsers)
			{
				if (posts.Any(p => p.UserId.ToString() == user.Id))
				{
					relevantUsers.Add(user);
				}
			}

			// Adding them to the dictionary
			foreach (Post post in posts)
			{
				userDict.Add(post, relevantUsers.First(u => new Guid(u.Id) == post.UserId));

				// this is absolute nonesense i need to start using the sql syntax thing
				// what this does (in theory) is that it connects the post table to the posttags table to the tags table
				// and gets all the names of the tags there 
				tagDict.Add(post, db.Tags.Where(t => t.TagId == db.PostTags.First(pt => pt.TagId == t.TagId && post.PostId == pt.PostId).TagId).ToArray());

			}

			for (int i = 0; i < posts.Length; i++)
			{
				// Creating the PartialViewResult
				_ForumPostPreviewModel model = new _ForumPostPreviewModel(posts[i].PostId, posts[i].Title, userDict[posts[i]].UserName, posts[i].NumberReplies, tagDict[posts[i]], posts[i].Date, posts[i].UserId);


				ReportedContent[i] = model;
			}

		}

        
    }
}
