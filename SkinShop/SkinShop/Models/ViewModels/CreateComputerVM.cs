using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkinShop.Models.ViewModels
{
    public class CreateComputerVM
    {
        [Display(Name = "Название")]
        [Required]
        [StringLength(60, MinimumLength = 0, ErrorMessage = "Недопустимая длина")]
        public string Name { get; set; }

        [Display(Name = "Процессор")]
        [Required]
        public string SelectedProcessor { get; set; }

        [Display(Name = "Оперативная память")]
        [Required]
        public ICollection<string> SelectedRAM { get; set; }

        [Display(Name = "Постоянная память")]
        [Required]
        public ICollection<string> SelectedROM { get; set; }

        [Display(Name = "Видеокарта")]
        [Required]
        public ICollection<string> SelectedVideoCard { get; set; }

        [Display(Name = "Материнская плата")]
        [Required]
        public string SelectedMotherBoard { get; set; }

        [Display(Name = "Блок питания")]
        [Required]
        public string SelectedPowerSupply { get; set; }

        [Display(Name = "Корпус")]
        [Required]
        public string SelectedSystemBlock { get; set; }

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

        [Display(Name = "Дополнительные свойства")]
        public virtual ICollection<PropertyDM> Properties { get; set; }

        public ICollection<SelectListItem> Processors { get; set; }

        public ICollection<SelectListItem> RAMs { get; set; }

        public ICollection<SelectListItem> ROMs { get; set; }

        public ICollection<SelectListItem> VideoCards { get; set; }

        public ICollection<SelectListItem> MotherBoards { get; set; }

        public ICollection<SelectListItem> PowerSupplies { get; set; }

        public ICollection<SelectListItem> SystemBlocks { get; set; }
    }
}