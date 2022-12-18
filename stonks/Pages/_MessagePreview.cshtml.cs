using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace stonks.Pages
{
    public class _MessagePreviewModel : PageModel
    {

        private Guid messageId;

        /// <summary>
        /// The unique id of the message
        /// </summary>
        public Guid MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        private string title;

        /// <summary>
        /// The title of the message
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string username;

        /// <summary>
        /// The username that sent the message
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private DateTime dateSent;

        /// <summary>
        /// The date the message was sent
        /// </summary>
        public DateTime DateSent
        {
            get { return dateSent; }
            set { dateSent = value; }
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public _MessagePreviewModel()
        {

        }

        public _MessagePreviewModel(Guid messageId,string title, string username, DateTime dateSent)
        {
            MessageId = messageId;
            Title = title;
            Username = username;
            DateSent = dateSent;
        }

        public void OnGet()
        {
        }
    }
}
