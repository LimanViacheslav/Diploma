using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.Account
{
    public class ContainerToCountOpensDM
    {
        public int Id { get; set; }

        public virtual ProductDM Container { get; set; }

        public int CountOpens { get; set; }
    }
}