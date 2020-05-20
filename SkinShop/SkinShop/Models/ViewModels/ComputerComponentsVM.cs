using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class ComputerComponentsVM
    {
        public IEnumerable<ProductDM> ComputerComponents { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}