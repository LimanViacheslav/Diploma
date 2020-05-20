using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class ComputerDM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ComputerComponentDM Processor { get; set; }

        public ICollection<ComputerComponentDM> RAM { get; set; }

        public ICollection<ComputerComponentDM> ROM { get; set; }

        public ICollection<ComputerComponentDM> VideoCard { get; set; }

        public ComputerComponentDM MotherBoard { get; set; }

        public ComputerComponentDM PowerSupply { get; set; }

        public ComputerComponentDM SystemBlock { get; set; }

        public virtual ICollection<PropertyDM> Properties { get; set; }
    }
}