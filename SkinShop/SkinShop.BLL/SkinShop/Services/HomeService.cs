using SkinShop.BLL.SkinShop.Interfaces;
using SkinShop.BLL.SkinShop.Mappers;
using SkinShop.BLL.SkinShop.SkinShopDTO;
using SkinShop.DL.Entities.SkinShop;
using SkinShop.DL.Interfaces.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.Services
{
    public class HomeService: IHomeService
    {
        IUnitOfWorK Database { get; set; }
        MappersForDTO _mappers = new MappersForDTO();

        public HomeService(IUnitOfWorK uof)
        {
            Database = uof;
        }

        public List<ProductDTO> ProductFilters(bool withSale, string searchText, int minPrice, int maxPrice, Goods table)
        {
            List<Product> products = new List<Product>();
            if(withSale)
            {
                if(searchText == "")
                {
                    products = Database.Products.Find(p => p.Sale != 0 && 
                    (p.Price - (p.Price * p.Sale)) > minPrice && (p.Price - (p.Price * p.Sale)) < maxPrice && p.Table == table).ToList();
                }
                else
                {
                    products = Database.Products.Find(p => p.Sale != 0 && 
                    (p.Price - (p.Price * p.Sale)) > minPrice && (p.Price - (p.Price * p.Sale)) < maxPrice &&
                    (p.Name.Contains(searchText) || p.Description.Contains(searchText)) && p.Table == table).ToList();
                }
            }
            else
            {
                if (searchText == "")
                {
                    products = Database.Products.Find(p => (p.Price !=0?(p.Price - (p.Price * p.Sale)) :(p.Price)) > minPrice 
                    && (p.Price!=0?(p.Price - (p.Price * p.Sale)) :(p.Price)) < maxPrice && p.Table == table).ToList();
                }
                else
                {
                    products = Database.Products.Find(p => (p.Price != 0 ? (p.Price - (p.Price * p.Sale)) : (p.Price)) > minPrice
                    && (p.Price != 0 ? (p.Price - (p.Price * p.Sale)) : (p.Price)) < maxPrice &&
                    (p.Name.Contains(searchText) || p.Description.Contains(searchText)) && p.Table == table).ToList();
                }
            }
            return _mappers.ToProductDTO.Map<List<Product>, List<ProductDTO>>(products);
        }
    }
}
