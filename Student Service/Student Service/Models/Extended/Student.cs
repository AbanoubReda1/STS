using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Student_Service.Models
{
    [MetadataType(typeof(StudentMetadata))]
    public partial class Student
    {
        public string ConfiremPassword { get; set; }
    }
    public class StudentMetadata

    {
        [Display(Name ="First Name")]
        [Required(AllowEmptyStrings =false,ErrorMessage = "First Name required")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name required")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Display(Name = "E-Mail Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-Mail Address required")]
        [DataType(DataType.EmailAddress)]
        public string E_Mail { get; set; }

        [Display(Name = "Department")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Department required")]
    
        public string Department { get; set; }



        [Display(Name = "Password ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage ="Password 8 or more characters")]
        public string Password { get; set; }


        [Display(Name = "Confirm Password ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not match ")]
        public string ConfiremPassword { get; set; }
    }
}