using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public class Color: CommonFields
    {
        public string Name { get; set; }

        public string ColorValue { get; set; }

        public virtual ICollection<Cloth> Clothes { get; set; }

        public virtual ICollection<SkinRarity> Rereties { get; set; }
    }
}
