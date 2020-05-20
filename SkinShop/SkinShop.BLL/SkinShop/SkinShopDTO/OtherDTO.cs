using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class OtherDTO : CommonFieldsDTO
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public virtual ICollection<PropertyDTO> Properties { get; set; }
    }
}
