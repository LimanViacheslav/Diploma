using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public class Container: CommonFields
    {
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public string Type { get; set; }

        public string TypeOfHard { get; set; }

        public double MinRare { get; set; }

        public double MaxRare { get; set; }

        public int ChanseForRare { get; set; }

        public int ChanseForLegendary { get; set; }

        public int CountOpens { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
