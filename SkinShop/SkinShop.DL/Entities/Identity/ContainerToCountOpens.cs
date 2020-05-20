using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.Identity
{
    public class ContainerToCountOpens: CommonFields
    {
        public virtual Product Container { get; set; }

        public int CountOpens { get; set; }
    }
}
