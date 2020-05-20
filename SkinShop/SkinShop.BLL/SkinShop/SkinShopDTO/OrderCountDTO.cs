using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class OrderCountDTO: CommonFieldsDTO
    {
        public virtual ProductDTO Product { get; set; }

        public string Size { get; set; }

        public ColorDTO Color { get; set; }

        public int Count { get; set; }
    }
}
