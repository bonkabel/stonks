using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stonks.Data;
using stonks.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace stonks.Pages
{
    public class SendMessageModel : PageModel //THIS THING :CONTROLLER
    {

        private readonly ApplicationDbContext db;

		

		private MessageInputModel input;

        [BindProperty]
        public MessageInputModel Input
        {
            get { return input; }
            set { input = value; }
        }



        public class MessageInputModel
        {
			[Required]
			[Remote(action: "ValidateRecipient", controller: "Validations")]
			public string Recipient { get; set; }

			[Required(ErrorMessage = "Please enter a title between 5 - 200 characters")]
            [StringLength(200, MinimumLength = 5)]
            
            public string Title { get; set; }

			[Required(ErrorMessage = "The text has a maximum of 10000 characters")]
			[StringLength(10000)]
			public string Text { get; set; }
		}

        

        public SendMessageModel(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            Guid? recipientId = null;

            if (this.User.FindFirst(ClaimTypes.NameIdentifier) != null && ValidateInput(out recipientId))
            {
                Message message = new Message(Guid.NewGuid(), new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier).Value), (Guid)recipientId, DateTime.UtcNow, Input.Title, Input.Text);
                db.Messages.Add(message);
                await db.SaveChangesAsync();
				return Redirect("/Messages/MESSAGES");

			}
            else
            {
                return null;
            }


            
        }

        private bool ValidateInput(out Guid? recipient)
        {
            bool valid = false;
            User user = db.Users.FirstOrDefault(u => u.UserName == Input.Recipient);
            recipient = null;
            


			if (user != null)
            {
                recipient = new Guid(user.Id);
                valid = true;
            }

            return valid;
        }
    }
}
