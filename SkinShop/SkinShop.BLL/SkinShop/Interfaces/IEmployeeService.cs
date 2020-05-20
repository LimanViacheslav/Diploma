using SkinShop.BLL.Identity.Infrastructure;
using SkinShop.BLL.SkinShop.SkinShopDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.Interfaces
{
    public interface IEmployeeService
    {
        OperationDetails ConfirmOrder(int? orderId, string employeeName);
        OperationDetails Reject(int? orderId, string employeeName, string text);
        IEnumerable<OrderDTO> GetOrders();
        OperationDetails DeleteOrder(int orderId);
        OperationDetails ReestablishOrder(int orderId);
        OperationDetails Pay(int orderId);
    }
}
