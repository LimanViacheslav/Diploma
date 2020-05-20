using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class CreateColorVM
    {
        [Display(Name = "Название")]
        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Недопустимая длина")]
        public string Name { get; set; }

        [Display(Name = "Код цвета")]
        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Недопустимая длина")]
        public string ColorValue { get; set; }
    }
}