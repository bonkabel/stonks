using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using stonks.Data;
using stonks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace stonks.Pages
{
	[IgnoreAntiforgeryToken(Order = 1001)]
	public class MessagesModel : PageModel
    {

		public enum MessagesState { MESSAGES, SENT, NEWS }

		private MessagesState? state;

		public MessagesState? State
		{
			get { return state; }
			set { state = value; }
		}


		/// <summary>
		/// The database context
		/// </summary>
		private readonly ApplicationDbContext db;

		private _MessagePreviewModel[] models;

		/// <summary>
		/// The MessagePrivewModels to display
		/// </summary>
		public _MessagePreviewModel[] Models
		{
			get { return models; }
			set { models = value; }
		}

		public MessagesModel(ApplicationDbContext db)
		{
			this.db = db;
		}


		public void OnGet(MessagesState? state)
        {
			Setup(state);
        }

		private void Setup(MessagesState? state)
		{
			State = state;
			if (this.User.FindFirst(ClaimTypes.NameIdentifier) != null)
			{
				Guid userId = new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
				Message[] messages = null;
				string newsEmail = "news@stonks.com";


				if (State == null || State == MessagesState.MESSAGES)
				{
					// All of the messages where this user is the recipient
					messages = db.Messages.Where(m => m.RecipientId == userId).ToArray();
				}
				else if (State == MessagesState.SENT)
				{
					messages = db.Messages.Where(m => m.SendorId == userId).ToArray();
				}
				else if (State == MessagesState.NEWS)
				{
					Guid newsId = new Guid(db.Users.First(u => u.UserName == newsEmail).Id);
					messages = db.Messages.Where(m => m.SendorId == newsId && m.RecipientId == userId).ToArray();
				}


				// initializing to the length of messages
				Models = new _MessagePreviewModel[messages.Length];
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

					Models[i] = model;
				}
			}
			
		}

        public async Task<IActionResult> OnPostReportMessage(string messageId)
        {
            Message message = db.Messages.FirstOrDefault(m => m.MessageId == new Guid(messageId));
            ReportedMessage reportedMessage = db.ReportedMessages.FirstOrDefault(rm => rm.MessageId == message.MessageId);

            if (message != null && reportedMessage == null)
            {
                db.ReportedMessages.Add(new ReportedMessage(Guid.NewGuid(), message.MessageId));
                await db.SaveChangesAsync();
            }


            return null;
        }
    }
}
