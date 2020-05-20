using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Models.Account
{
    public class UserDM
    {
        public string Id { get; set; }
        
        public string Email { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        public bool IsBanned { get; set; }

        public virtual ClientProfileDM Client { get; set; }

        public virtual ImageDM Image { get; set; }
    }
}