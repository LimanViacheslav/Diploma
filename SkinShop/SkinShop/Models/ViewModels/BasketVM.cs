using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class BasketVM
    {
        public BasketDM Basket { get; set; }

        public List<ClothDM> Clothes { get; set; }
    }
}