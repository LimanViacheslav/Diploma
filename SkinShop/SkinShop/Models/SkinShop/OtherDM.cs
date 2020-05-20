using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class OtherDM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public virtual ICollection<PropertyDM> Properties { get; set; }
    }
}