using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class ProductDTO : CommonFieldsDTO
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public int Sale { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ImageDTO> Images { get; set; }

        public int CountFavorites { get; set; }

        public int CountOrders { get; set; }

        public int CountSeen { get; set; }

        public int FromTableId { get; set; }

        public DateTime DateOfAdded { get; set; }
        
        public Goods Table { get; set; }
    }
}
