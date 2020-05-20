using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class ContainerOpensVM
    {
        public ProductDM Container { get; set; }

        public ProductDM ContainerLeft { get; set; }

        public ProductDM ContainerRight { get; set; }

        public string Message { get; set; }
    }
}