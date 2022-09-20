using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stonks.Models
{
    /// <summary>
    /// A User of the web app
    /// </summary>
    public class User : IdentityUser
    {
        [Required]
        /// <summary>
        /// If the user is an admin
        /// </summary>
        public bool IsAdmin { get; set; }

        

    }
}
