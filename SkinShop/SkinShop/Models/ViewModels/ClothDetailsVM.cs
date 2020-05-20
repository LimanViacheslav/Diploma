﻿using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class ClothDetailsVM
    {
        public ClothDM Cloth { get; set; }

        public ProductDM Product { get; set; }

        public bool IsSkinAlreadyInBasket { get; set; }

        public bool IsSkinAlreadyInFavorites { get; set; }

        public List<ProductDM> OtherClothes { get; set; }
    }
}