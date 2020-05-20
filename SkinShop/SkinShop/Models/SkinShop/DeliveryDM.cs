using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class DeliveryDM
    {
        public int Id { get; set; }

        public string PayType { get; set; }

        public string DeliveryType { get; set; }

        public string DeliveryAddress { get; set; }

        public string Wishes { get; set; }
    }
}