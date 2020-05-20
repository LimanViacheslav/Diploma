using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class SkinDTO : CommonFieldsDTO
    {
        public string Name { get; set; }

        public virtual SkinRarityDTO SkinRarity { get; set; }

        public virtual GameDTO Game { get; set; }

        public virtual ICollection<PropertyDTO> Properties { get; set; }
    }
}
