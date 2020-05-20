using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class OrderCountDM
    {
        public int Id { get; set; }

        public virtual ProductDM Product { get; set; }

        public string Size { get; set; }

        public ColorDM Color { get; set; }

        public int Count { get; set; }
    }
}