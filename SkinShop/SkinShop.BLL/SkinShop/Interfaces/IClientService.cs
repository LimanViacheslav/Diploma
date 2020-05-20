using SkinShop.BLL.Identity.IdentityDTO;
using SkinShop.BLL.Identity.Infrastructure;
using SkinShop.BLL.SkinShop.BusinessModels;
using SkinShop.BLL.SkinShop.Services;
using SkinShop.BLL.SkinShop.SkinShopDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.Interfaces
{
    public interface IClientService
    {
        OperationDetails MakeOrder(List<int> productsId, List<int> counts, string clientName, List<string> colors, List<string> sizes, out int id);
        OperationDetails AddToFavorites(int skinId, string clientName = "");
        OperationDetails DeleteFromFavorites(int skinId, string clientName = "");
        OperationDetails AddToBasket(int skinId, string clientName = "");
        OperationDetails DeleteFromBasket(int skinId, string clientName = "");
        FavoritesDTO GetFavorites(string clientName = "");
        BasketDTO GetBasket(string clientName = "");
        IEnumerable<OrderDTO> GetOrders(string clientName);
        OperationDetails CountUp(int productId, Count countField);
        OperationDetails DeleteOrder(int orderId);
        OperationDetails WriteComment(string text, string userName, int assessment, int productId, int parentId);
        List<CommentDTO> GetCommentsForProduct(int id);
        OperationDetails MakeDelivery(int orderId, DeliveryDTO model);
        OrderDTO GetOrder(int id);
        OperationDetails Pay(int id);
        OperationDetails ReestablishOrder(int orderId);
        OperationDetails Replenish(double sum, string user);
        UserDTO GetUserDTO(string name);
        OperationDetails BuyContainer(int id, int count, string userName);
        ContainerOpens OpenContainer(int id, string userName);
        OperationDetails SaleProduct(int id, int count, string userName);
    }
}
