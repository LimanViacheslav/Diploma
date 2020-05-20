using SkinShop.BLL.Identity.IdentityDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class FavoritesDTO: CommonFieldsDTO
    {
        public virtual ICollection<ProductDTO> Products { get; set; }
    }
}
