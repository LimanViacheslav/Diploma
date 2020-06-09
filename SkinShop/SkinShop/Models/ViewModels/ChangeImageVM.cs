using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class ChangeImageVM
    {
        [Display(Name = "Изображение")]
        public HttpPostedFileBase Image { get; set; }

        [Display(Name = "Подпись к изображению")]
        [StringLength(60, MinimumLength = 0, ErrorMessage = "Недопустимая длина")]
        public string Alt { get; set; }
    }
}