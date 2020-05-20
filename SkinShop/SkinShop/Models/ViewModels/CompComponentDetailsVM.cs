using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class CompComponentDetailsVM
    {
        public ComputerComponentDM ComputerComponent { get; set; }

        public ProductDM Product { get; set; }

        public bool IsCompCompAlreadyInBasket { get; set; }

        public bool IsCompCompAlreadyInFavorites { get; set; }

        public List<ProductDM> OtherComputerComponents { get; set; }
    }
}