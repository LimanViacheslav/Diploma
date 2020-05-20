using SkinShop.BLL.SkinShop.SkinShopDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.BusinessModels
{
    public class ContainerOpens
    {
        public ProductDTO Container { get; set; }

        public ProductDTO ContainerLeft { get; set; }

        public ProductDTO ContainerRight { get; set; }
    }
}
