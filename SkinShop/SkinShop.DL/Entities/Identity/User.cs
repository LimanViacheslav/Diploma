using Microsoft.AspNet.Identity.EntityFramework;
using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Entities.Identity
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public string Adres { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBanned { get; set; }

        public virtual ClientProfile Client { get; set; }

        public virtual Image Image { get; set; }
    }
}
