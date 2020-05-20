using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class SkinRarityDTO: CommonFieldsDTO
    {
        public string RarityName { get; set; }

        public virtual ICollection<ColorDTO> Colors { get; set; }
    }
}
