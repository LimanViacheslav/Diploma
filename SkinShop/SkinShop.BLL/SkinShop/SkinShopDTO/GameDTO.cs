using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class GameDTO : CommonFieldsDTO
    {
        public string Name { get; set; }

        public string SystemRequirements { get; set; }

        public bool IsThingGame { get; set; }

        public virtual ICollection<PropertyDTO> Properties { get; set; }
    }
}
