using SkinShop.BLL.Identity.IdentityDTO;
using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.SkinShopDTO
{
    public class OrderDTO : CommonFieldsDTO
    {
        public string ClientId { get; set; }

        public virtual UserDTO Client { get; set; }

        public virtual ICollection<OrderCountDTO> OrderCounts { get; set; }

        public string EmployeeId { get; set; }

        public virtual UserDTO Employee { get; set; }

        public DateTime OrderTime { get; set; }

        public OrderStatus Status { get; set; }

        public string RejectText { get; set; }

        public double Price { get; set; }

        public bool IsDeletedFromUser { get; set; }

        public bool IsDeletedFromEmployee { get; set; }

        public bool IsPayed { get; set; }

        public virtual DeliveryDTO Delivery { get; set; }
    }
}
