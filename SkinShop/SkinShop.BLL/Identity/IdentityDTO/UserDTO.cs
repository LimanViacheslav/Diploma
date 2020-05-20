using SkinShop.BLL.SkinShop.SkinShopDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.Identity.IdentityDTO
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        public bool IsBanned { get; set; }

        public virtual ClientProfileDTO Client { get; set; }

        public virtual ImageDTO Image { get; set; }
    }
}
