using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class ClothDM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool ForMen { get; set; }

        public string Composition { get; set; }

        public virtual ICollection<StringDataDM> Sizes { get; set; }

        public virtual ICollection<ColorDM> Colors { get; set; }

        public virtual ICollection<PropertyDM> Properties { get; set; }
    }
}