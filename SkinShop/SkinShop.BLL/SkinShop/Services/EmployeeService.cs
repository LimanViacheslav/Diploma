using SkinShop.BLL.Identity.Infrastructure;
using SkinShop.BLL.SkinShop.Interfaces;
using SkinShop.BLL.SkinShop.Mappers;
using SkinShop.BLL.SkinShop.SkinShopDTO;
using SkinShop.DL.Entities.Identity;
using SkinShop.DL.Entities.SkinShop;
using SkinShop.DL.Interfaces.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.Services
{
    public class EmployeeService: IEmployeeService
    {
        IUnitOfWorK Database { get; set; }
        MappersForDTO _mappers = new MappersForDTO();

        public EmployeeService(IUnitOfWorK uow)
        {
            Database = uow;
        }

        public OperationDetails ConfirmOrder(int? orderId, string employeeName)
        {
            if (orderId != null)
            {
                Order order = Database.Orders.Get(Convert.ToInt32(orderId));
                if (employeeName != null && order.Employee == null)
                {
                    User employee = GetUser(employeeName);
                    order.Employee = employee;
                    order.EmployeeId = employee.Id;
                }
                else
                {
                    return new OperationDetails(false, "Не удалось найти сотрудника", this.ToString());
                }
                order.Status = OrderStatus.Confirmed;
                Database.Orders.Update(order);
                Database.Save();
                return new OperationDetails(true, "Заказ успешно подтверждён", this.ToString());
            }
            else
            {
                return new OperationDetails(false, "Не удалось найти заказ", this.ToString());
            }
        }

        public OperationDetails Reject(int? orderId, string employeeName, string text)
        {
            if (orderId != null)
            {
                Order order = Database.Orders.Get(Convert.ToInt32(orderId));
                if (employeeName != null && order.Employee == null)
                {
                    User employee = GetUser(employeeName);
                    order.Employee = employee;
                    order.EmployeeId = employee.Id;
                }
                else
                {
                    return new OperationDetails(false, "Не удалось найти сотрудника", this.ToString());
                }
                order.Status = OrderStatus.Rejected;
                order.RejectText = text;
                Database.Orders.Update(order);
                Database.Save();
                return new OperationDetails(true, "Заказ был отклонён", this.ToString());
            }
            else
            {
                return new OperationDetails(false, "Не удалось найти заказ", this.ToString());
            }
        }

        public IEnumerable<OrderDTO> GetOrders()
        {
            return _mappers.ToOrderDTO.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(Database.Orders.Find(o => o.IsDeleted == false));
        }

        public OperationDetails DeleteOrder(int orderId)
        {
            Order order = Database.Orders.Find(x => x.Id == orderId).FirstOrDefault();
            if (order == null)
                return new OperationDetails(false, "Не удалось найти заказ", this.ToString());
            order.IsDeletedFromEmployee = true;
            Database.Orders.Update(order);
            Database.Save();
            return new OperationDetails(true, "Заказ был упешно удалён", this.ToString());
        }

        public OperationDetails ReestablishOrder(int orderId)
        {
            Order order = Database.Orders.Find(x => x.Id == orderId).FirstOrDefault();
            if (order == null)
                return new OperationDetails(false, "Не удалось найти заказ", this.ToString());
            order.IsDeletedFromEmployee = false;
            Database.Orders.Update(order);
            Database.Save();
            return new OperationDetails(true, "Заказ был упешно восстановлен", this.ToString());
        }

        public OperationDetails Pay(int orderId)
        {
            Order order = Database.Orders.Find(x => x.Id == orderId).FirstOrDefault();
            if (order == null)
                return new OperationDetails(false, "Не удалось найти заказ", this.ToString());
            order.IsPayed = true;
            Database.Orders.Update(order);
            Database.Save();
            return new OperationDetails(true, "Заказ был упешно оплачен", this.ToString());
        }

        public User GetUser(string userName)
        {
            User item = Database.ClientManager.FindUser(x => x.Email == userName);
            return item;
        }
    }
}
