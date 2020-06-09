using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class OtherDetailsVM
    {
        public OtherDM Other { get; set; }

        public ProductDM Product { get; set; }

        public bool IsOtherAlreadyInBasket { get; set; }

        public bool IsOtherAlreadyInFavorites { get; set; }

        public List<ProductDM> OtherOthers { get; set; }
    }
}