using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class SkinDM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual SkinRaretyDM SkinRarity { get; set; }

        public virtual GameDM Game { get; set; }

        public virtual ICollection<PropertyDM> Properties { get; set; }
    }
}