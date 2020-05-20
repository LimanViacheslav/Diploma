using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class CreateComputerComponentVM
    {
        [Display(Name = "Название")]
        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Недопустимая длина")]
        public string Name { get; set; }

        [Display(Name = "Тип комплектующего")]
        [Required]
        public string Type { get; set; }

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
        [StringLength(60, MinimumLength = 0, ErrorMessage = "Недопустимая длина")]
        public string Alt { get; set; }

        [Display(Name = "Дополнительные свойства")]
        public virtual ICollection<PropertyDM> Properties { get; set; }
    }
}