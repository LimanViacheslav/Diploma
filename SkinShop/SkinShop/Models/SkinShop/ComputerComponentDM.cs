using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class ComputerComponentDM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ComponentType Type { get; set; }

        public virtual ICollection<PropertyDM> Properties { get; set; }
    }
}