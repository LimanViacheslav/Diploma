using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class SkinRaretyDM
    {
        public int Id { get; set; }

        public string RarityName { get; set; }

        public virtual ICollection<ColorDM> Colors { get; set; }
    }
}