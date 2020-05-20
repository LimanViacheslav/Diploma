using SkinShop.BLL.SkinShop.SkinShopDTO;
using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.Interfaces
{
    public interface IHomeService
    {
        List<ProductDTO> ProductFilters(bool withSale, string searchText, int minPrice, int maxPrice, Goods table);
    }
}
