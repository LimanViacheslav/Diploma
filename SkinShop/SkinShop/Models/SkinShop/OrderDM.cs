using SkinShop.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.SkinShop
{
    public enum OrderStatusDM
    {
        Confirmed = 1,
        NotConfirmed,
        InProcessing,
        Rejected,
        Completed,
        Null
    }

    public class OrderDM
    {
        public int Id { get; set; }

        public string ClientId { get; set; }

        public virtual UserDM Client { get; set; }

        public virtual ICollection<OrderCountDM> OrderCounts { get; set; }

        public string EmployeeId { get; set; }

        public virtual UserDM Employee { get; set; }

        public DateTime OrderTime { get; set; }

        public OrderStatusDM Status { get; set; }

        public string RejectText { get; set; }

        public double Price { get; set; }

        public bool IsDeletedFromUser { get; set; }

        public bool IsDeletedFromEmployee { get; set; }

        public bool IsPayed { get; set; }

        public virtual DeliveryDM Delivery { get; set; }
    }
}