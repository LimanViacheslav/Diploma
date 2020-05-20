using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class ClothDTO : CommonFieldsDTO
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public bool ForMen { get; set; }

        public string Composition { get; set; }

        public virtual ICollection<StringDataDTO> Sizes { get; set; }

        public virtual ICollection<ColorDTO> Colors { get; set; }

        public virtual ICollection<PropertyDTO> Properties { get; set; }
    }
}
