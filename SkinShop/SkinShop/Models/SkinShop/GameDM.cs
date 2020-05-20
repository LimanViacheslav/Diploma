using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public class GameDM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SystemRequirements { get; set; }

        public bool IsThingGame { get; set; }

        public virtual ICollection<PropertyDM> Properties { get; set; }
    }
}