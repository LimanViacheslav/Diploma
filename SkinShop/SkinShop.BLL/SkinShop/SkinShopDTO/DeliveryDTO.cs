using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class DeliveryDTO : CommonFieldsDTO
    {
        public string PayType { get; set; }

        public string DeliveryType { get; set; }

        public string DeliveryAddress { get; set; }

        public string Wishes { get; set; }
    }
}
