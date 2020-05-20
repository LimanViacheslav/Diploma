using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.Account
{
    public class ClientProfileDM
    {
        public string Id { get; set; }

        public virtual BasketDM Basket { get; set; }

        public virtual FavoritesDM Favorites { get; set; }

        public double Money { get; set; }

        public virtual ICollection<ContainerToCountOpensDM> Containers { get; set; }

        public virtual ICollection<ContainerToCountOpensDM> Products { get; set; }

        public virtual ICollection<ContainerToCountOpensDM> ContainerToCountOpens { get; set; }
    }
}