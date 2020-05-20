using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public class Skin : CommonFields
    {
        public string Name { get; set; }

        public virtual SkinRarity SkinRarity { get; set; }

        public virtual Game Game { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
