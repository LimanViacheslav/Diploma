using SkinShop.BLL.Identity.IdentityDTO;
using SkinShop.BLL.SkinShop.Interfaces;
using SkinShop.BLL.SkinShop.Services;
using SkinShop.DL.Interfaces.SkinShop;
using SkinShop.DL.Repositories.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkinShop.Filters
{
    public class BanUserAttribute: AuthorizeAttribute
    {
        IAccountService _service = new AccountService(new UnitOfWork("DefaultConnection"));

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User;
            UserDTO userDTO = _service.GetUserByName(user.Identity.Name);
            if (userDTO != null && userDTO.IsBanned)
            {
                return httpContext.Request.IsAuthenticated;
            }
            else
            {
                return true;
            }
        }
    }
}