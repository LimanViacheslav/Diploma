using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public class Cloth: CommonFields
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public bool ForMen { get; set; }

        public string Composition { get; set; }

        public virtual ICollection<StringData> Sizes { get; set; }

        public virtual ICollection<Color> Colors { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
