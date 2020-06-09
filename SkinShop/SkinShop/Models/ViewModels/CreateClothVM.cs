using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkinShop.Models.ViewModels
{
    public class CreateClothVM
    {
        [Display(Name = "Название")]
        [Required]
        [StringLength(80, MinimumLength = 0, ErrorMessage = "Недопустимая длина")]
        public string Name { get; set; }

        [Display(Name = "Тип одежды")]
        [Required]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Недопустимая длина")]
        public string Type { get; set; }

        [Display(Name = "Пол")]
        [Required]
        public string ForMen { get; set; }

        [Display(Name = "Состав")]
        [Required]
        [StringLength(200, MinimumLength = 0, ErrorMessage = "Недопустимая длина")]
        public string Composition { get; set; }

        [Display(Name = "Размеры")]
        [Required]
        public virtual ICollection<string> Sizes { get; set; }

        [Display(Name = "Цвета")]
        [Required]
        public virtual ICollection<string> SelectedColors { get; set; }

        [Display(Name = "Дополнительные свойства")]
        public virtual ICollection<PropertyDM> Properties { get; set; }

        [Display(Name = "Цена")]
        [Required]
        [Range(0.01, 10000000, ErrorMessage = "Недопустимая длина")]
        public double Price { get; set; }

        [Display(Name = "Скидка(%)")]
        [Required]
        [Range(0, 99, ErrorMessage = "Недопустимая длина")]
        public int Sale { get; set; }

        [Display(Name = "Описание")]
        [StringLength(300, MinimumLength = 0, ErrorMessage = "Недопустимая длина")]
        public string Description { get; set; }

        [Display(Name = "Изображение")]
        public List<HttpPostedFileBase> Images { get; set; }

        public List<ImageDM> ImagesInDatebase { get; set; }

        [Display(Name = "Подпись к изображению")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Недопустимая длина")]
        public string Alt { get; set; }

        public IEnumerable<SelectListItem> Colors{ get; set; }

        public List<string> PropertyNames { get; set; }

        public List<string> PropertyValues { get; set; }
    }
}