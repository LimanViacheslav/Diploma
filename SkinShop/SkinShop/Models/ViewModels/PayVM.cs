using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.ViewModels
{
    public class PayVM
    {
        public int OrderId { get; set; }

        public double Sum { get; set; }

        public bool IsReplenishing { get; set; }

        public double ReplenishSum { get; set; }
    }
}