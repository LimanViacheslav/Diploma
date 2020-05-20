using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public enum ComponentType {
        Processor = 1,
        RAM,
        ROM,
        VideoCard,
        MotherBoard,
        PowerSupply,
        SystemBlock
    }

    public class ComputerComponent: CommonFields
    {
        public string Name { get; set; }

        public ComponentType Type { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
