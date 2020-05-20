using SkinShop.BLL.SkinShop.SkinShopDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.Identity.IdentityDTO
{
    public class ClientProfileDTO
    {
        public string Id { get; set; }

        public virtual BasketDTO Basket { get; set; }

        public virtual FavoritesDTO Favorites { get; set; }

        public double Money { get; set; }

        public virtual ICollection<ContainerToCountOpensDTO> Containers { get; set; }

        public virtual ICollection<ContainerToCountOpensDTO> Products { get; set; }

        public virtual ICollection<ContainerToCountOpensDTO> ContainerToCountOpens { get; set; }
    }
}
