using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class ContainerDM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ProductDM> Products { get; set; }

        public string Type { get; set; }

        public string TypeOfHard { get; set; }

        public double MinRare { get; set; }

        public double MaxRare { get; set; }

        public int ChanseForRare { get; set; }

        public int ChanseForLegendary { get; set; }

        public int CountOpens { get; set; }

        public virtual ICollection<PropertyDM> Properties { get; set; }
    }
}