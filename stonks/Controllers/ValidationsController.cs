using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stonks.Data;
using stonks.Models;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Controllers
{
    public class ValidationsController : Controller
    {

		private readonly ApplicationDbContext db;

		public ValidationsController(ApplicationDbContext db)
		{
			this.db = db;
		}

        public IActionResult Index()
        {
            return View();
        }

		[AllowAnonymous]
		[AcceptVerbs("Get", "Post")]
		public IActionResult ValidateRecipient([Bind(Prefix = "Input.Recipient")] string recipient)
		{
			if (!db.Users.Any(u => u.UserName == recipient))
			{
				return Json("User " + recipient + "does not exist");
			}
			else
			{
				return Json(true);
			}

			
		}
	}
}
