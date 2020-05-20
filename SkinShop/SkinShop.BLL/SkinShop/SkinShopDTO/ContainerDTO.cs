using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class ContainerDTO : CommonFieldsDTO
    {
        public string Name { get; set; }

        public virtual ICollection<ProductDTO> Products { get; set; }

        public string Type { get; set; }

        public string TypeOfHard { get; set; }

        public double MinRare { get; set; }

        public double MaxRare { get; set; }

        public int ChanseForRare { get; set; }

        public int ChanseForLegendary { get; set; }

        public int CountOpens { get; set; }

        public virtual ICollection<PropertyDTO> Properties { get; set; }
    }
}
