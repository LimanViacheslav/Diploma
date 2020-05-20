using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class ClothesVM
    {
        public IEnumerable<ProductDM> Clothes { get; set; }

        public List<string> Types { get; set; }

        public List<string> Colors { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}