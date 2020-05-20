using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class ComputerComponentDTO : CommonFieldsDTO
    {
        public string Name { get; set; }

        public ComponentType Type { get; set; }

        public virtual ICollection<PropertyDTO> Properties { get; set; }
    }
}
