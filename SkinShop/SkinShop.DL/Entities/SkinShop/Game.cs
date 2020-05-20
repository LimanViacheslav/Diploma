using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public class Game : CommonFields
    {
        public string Name { get; set; }

        public string SystemRequirements  { get; set; }

        public bool IsThingGame { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
