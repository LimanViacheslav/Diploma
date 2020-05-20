using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class GamesVM
    {
        public IEnumerable<ProductDM> Games { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public List<string> Genres { get; set; }

        public List<string> Types { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}