using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stonks.Data;
using stonks.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace stonks.Pages
{
	[IgnoreAntiforgeryToken(Order = 1001)]
	public class MessageModel : PageModel
    {

		private readonly ApplicationDbContext db;

		private Message message;

		public Message Message
		{
			get { return message; }
			set { message = value; }
		}

		private MessageReply[] replies;

		/// <summary>
		/// The replies to the message
		/// </summary>
		[BindProperty]
		public MessageReply[] Replies
		{
			get { return replies; }
			set { replies = value; }
		}

		private User sendor;

		/// <summary>
		/// The user that sent the message
		/// </summary>
		public User Sendor
		{
			get { return sendor; }
			set { sendor = value; }
		}

		private Dictionary<MessageReply, User> replyUsers;

		/// <summary>
		/// The replies and users that made them
		/// </summary>
		public Dictionary<MessageReply, User> ReplyUsers
		{
			get { return replyUsers; }
			set { replyUsers = value; }
		}

		private MessageReplyInputModel input;

		/// <summary>
		/// The users MessageReply input
		/// </summary>
		[BindProperty]
		public MessageReplyInputModel Input
		{
			get { return input; }
			set { input = value; }
		}

		private User currentUser;

		public User CurrentUser
		{
			get { return currentUser; }
			set { currentUser = value; }
		}



		/// <summary>
		/// The MessageReply input model.  THis is for verifying the user input
		/// </summary>
		public class MessageReplyInputModel
		{
			[Required(ErrorMessage = "Please enter text")]
			[StringLength(10000, MinimumLength = 1)]
			public string Text { get; set; }
		}

		public MessageModel(ApplicationDbContext db)
		{
			this.db = db;
		}



		public void OnGet(Guid id)
		{ 
			if (id != Guid.Empty && this.User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				Setup(id);
			}
        }

		public async Task<IActionResult> OnPostAsync(Guid id)
		{
			if (id != Guid.Empty && this.User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				Setup(id);

				if (CheckFormData())
				{
					db.MessageReplies.Add(new MessageReply(Guid.NewGuid(), id, new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value), Input.Text, DateTime.UtcNow));
					await db.SaveChangesAsync();
				}
			}


			return RedirectToAction("");
		}

		private bool CheckFormData()
		{
			bool valid = true;
			if (Input.Text == null || Input.Text.Length <= 0)
			{
				valid = false;
			}
			return valid;
		}

		private void Setup(Guid id)
		{
			CurrentUser = db.Users.First(u => u.Id == this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

			Message = db.Messages.First(m => m.MessageId == id);

			Replies = db.MessageReplies.Where(mr => mr.MessageId == Message.MessageId).OrderBy(pr => pr.Date).ToArray();

			Sendor = db.Users.First(u => u.Id == Message.SendorId.ToString());

			ReplyUsers = new Dictionary<MessageReply, User>();

			// Needed to create the ReplyUsers dictionary.
			// This is done in a convoluted way as linq has issues with doing it simply for some reason.
			User[] allUsers = db.Users.ToArray();
			foreach (User user in allUsers)
			{
				MessageReply[] messageReply = Replies.Where(r => r.UserId.ToString() == user.Id).ToArray();

				if (messageReply != null && messageReply.Length > 0)
				{
					foreach (MessageReply postReply in messageReply)
					{
						replyUsers.Add(postReply, user);
					}

				}

			}
		}

        public async Task<IActionResult> OnPostReportMessage(string messageId)
        {
            Message iMessage = db.Messages.FirstOrDefault(m => m.MessageId == new Guid(messageId));
            ReportedMessage reportedMessage = db.ReportedMessages.FirstOrDefault(rm => rm.MessageId == iMessage.MessageId);

            if (iMessage != null && reportedMessage == null)
            {
                db.ReportedMessages.Add(new ReportedMessage(Guid.NewGuid(), iMessage.MessageId));
                await db.SaveChangesAsync();
            }


            return null;
        }

        public async Task<IActionResult> OnPostReportMessageReply(string messageReplyId)
        {
            MessageReply messageReply = db.MessageReplies.FirstOrDefault(mr => mr.ReplyId == new Guid(messageReplyId));
            ReportedMessageReply reportedMessageReply = db.ReportedMessageReplies.FirstOrDefault(rmr => rmr.ReplyId == messageReply.ReplyId);

            if (messageReply != null && reportedMessageReply == null)
            {
                db.ReportedMessageReplies.Add(new ReportedMessageReply(Guid.NewGuid(), messageReply.ReplyId));
                await db.SaveChangesAsync();
            }


            return null;
        }
    }
}
