using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Care.Models
{
    public class UserRegistrationModel
    {
        [Required]
        [DisplayName("Full name")]
        public string FullName { get; set; }
        [Required]
        [DisplayName("Email address")]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
