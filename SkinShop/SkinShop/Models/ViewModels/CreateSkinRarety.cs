using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkinShop.Models.ViewModels
{
    public class CreateSkinRarety
    {
        [Display(Name = "Название")]
        [Required]
        [StringLength(80, MinimumLength = 0, ErrorMessage = "Недопустимая длина")]
        public string RaretyName { get; set; }

        [Display(Name = "Цвета редкости")]
        [Required]
        public List<string> SelectedColors { get; set; }

        public List<SelectListItem> Colors { get; set; }
    }
}