using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.Identity
{
    public class ClientProfile
    {
        public string Id { get; set; }

        public int? BasketId { get; set; }
        public virtual Basket Basket { get; set; }

        public int? FavoritesId { get; set; }
        public virtual Favorites Favorites { get; set; }

        public double Money { get; set; }

        public virtual ICollection<ContainerToCountOpens> Containers { get; set; }

        public virtual ICollection<ContainerToCountOpens> Products { get; set; }

        public virtual ICollection<ContainerToCountOpens> ContainerToCountOpens { get; set; }
    }
}
