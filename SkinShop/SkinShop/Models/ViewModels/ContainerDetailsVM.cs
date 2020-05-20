using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class ContainerDetailsVM
    {
        public ContainerDM Container { get; set; }

        public ProductDM Product { get; set; }

        public bool IsContainerAlreadyInBasket { get; set; }

        public bool IsContainerAlreadyInFavorites { get; set; }

        public List<ProductDM> OtherContainers { get; set; }
    }
}