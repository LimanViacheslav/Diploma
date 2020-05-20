using SkinShop.BLL.SkinShop.SkinShopDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.Identity.IdentityDTO
{
    public class ContainerToCountOpensDTO: CommonFieldsDTO
    {
        public virtual ProductDTO Container { get; set; }

        public int CountOpens { get; set; }
    }
}
