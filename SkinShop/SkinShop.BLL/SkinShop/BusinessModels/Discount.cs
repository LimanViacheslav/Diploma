using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.BusinessModels
{
    public class Discount
    {
        public static double GetDiscountedPrice(double price, int discountPercent)
        {
            return price - (price * discountPercent) / 100;
        }
    }
}
