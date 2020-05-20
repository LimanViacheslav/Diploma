using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public class StringData: CommonFields
    {
        public string Data { get; set; }

        public virtual ICollection<Cloth> Clothes { get; set; }
    }
}
