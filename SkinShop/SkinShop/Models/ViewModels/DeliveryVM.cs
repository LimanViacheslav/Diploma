using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class DeliveryVM
    {
        public int Id { get; set; }

        [Display(Name = "Способ оплаты")]
        [Required]
        public string PayType { get; set; }

        [Display(Name = "Почта")]
        public string Post { get; set; }

        [Display(Name = "Отделение")]
        public string PostDepartment { get; set; }

        [Display(Name = "Способ доставки")]
        [Required]
        public string DeliveryType { get; set; }

        [Display(Name = "Адрес доставки")]
        [Required]
        public string DeliveryAddress { get; set; }

        [Display(Name = "Пожелания")]
        [StringLength(300, ErrorMessage = "Недопустимая длина")]
        public string Wishes { get; set; }

        public int OrderId { get; set; }
    }
}