using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkinShop.Models
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Изображение")]
        public List<HttpPostedFileBase> Images { get; set; }

        public List<ImageDM> ImagesInDatebase { get; set; }

        [Display(Name = "Подпись к изображению")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Недопустимая длина")]
        public string Alt { get; set; }
    }
}