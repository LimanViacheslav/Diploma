using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public class Other: CommonFields
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
