using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public class SkinRarity : CommonFields
    {
        public string RarityName { get; set; }

        public virtual ICollection<Color> Colors { get; set; }
    }
}
