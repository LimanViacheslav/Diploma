using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.SkinShop
{
    public class Computer: CommonFields
    {
        public string Name { get; set; }

        public virtual ComputerComponent Processor { get; set; }

        public virtual ICollection<ComputerComponent> RAM { get; set; }

        public virtual ICollection<ComputerComponent> ROM { get; set; }

        public virtual ICollection<ComputerComponent> VideoCard { get; set; }

        public virtual ComputerComponent MotherBoard { get; set; }

        public virtual ComputerComponent PowerSupply { get; set; }

        public virtual ComputerComponent SystemBlock { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
