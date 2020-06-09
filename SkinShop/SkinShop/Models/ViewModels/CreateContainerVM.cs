using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkinShop.Models.ViewModels
{
    public class CreateContainerVM
    {
        [Display(Name = "Название")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Товары")]
        public virtual ICollection<SelectListItem> Products { get; set; }

        public virtual ICollection<string> SelectedProducts { get; set; }

        [Display(Name = "Тип кейса")]
        [Required]
        public string Type { get; set; }

        [Display(Name = "Тип сложности")]
        [Required]
        public string TypeOfHard { get; set; }

        [Display(Name = "Шанс для редкого")]
        [Required]
        public int ChanseForRare { get; set; }

        [Display(Name = "Денежный порог до редкого")]
        [Required]
        public double MinRare { get; set; }

        [Display(Name = "Денежный порог до легендарного")]
        [Required]
        public double MaxRare { get; set; }

        [Display(Name = "Шанс для легендарного")]
        [Required]
        public int ChanseForLegendary { get; set; }

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

        public List<string> PropertyNames { get; set; }

        public List<string> PropertyValues { get; set; }
    }
}