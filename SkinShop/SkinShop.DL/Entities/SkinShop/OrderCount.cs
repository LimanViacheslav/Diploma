using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public class OrderCount : CommonFields
    {
        public virtual Product Product { get; set; }

        public string Size { get; set; }

        public virtual Color Color { get; set; }

        public int Count { get; set; }
    }
}
