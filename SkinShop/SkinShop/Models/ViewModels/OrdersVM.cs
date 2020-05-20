using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class OrdersVM
    {
        public List<OrderDM> Orders { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }
    }
}