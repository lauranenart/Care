using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Care.Models
{
    public class AdminModel
    {
        [Required]
        public string Password { get; set; }
    }
}
