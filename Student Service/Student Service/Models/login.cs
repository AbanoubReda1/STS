using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Student_Service.Models
{
    public class login
    {
        [Display(Name = "E-Mail Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-Mail Address required")]
        [DataType(DataType.EmailAddress)]
        public string E_Mail { get; set; }

        [Display(Name = "Password ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password 8 or more characters")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }


    }
}