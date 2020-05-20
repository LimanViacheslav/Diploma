using SkinShop.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class FavoritesDM
    {
        public int Id { get; set; }

        public virtual ICollection<ProductDM> Products { get; set; }
    }
}