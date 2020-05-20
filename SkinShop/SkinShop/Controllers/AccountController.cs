using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SkinShop.BLL.Identity.IdentityDTO;
using SkinShop.BLL.Identity.Infrastructure;
using SkinShop.BLL.SkinShop.Interfaces;
using SkinShop.BLL.SkinShop.Services;
using SkinShop.BLL.SkinShop.SkinShopDTO;
using SkinShop.DL.Interfaces.SkinShop;
using SkinShop.DL.Repositories.SkinShop;
using SkinShop.Filters;
using SkinShop.Mappers;
using SkinShop.Models;
using SkinShop.Models.Account;
using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SkinShop.Controllers
{
    [BanAttribute]
    public class AccountController : Controller
    {
        IAccountService _service = new AccountService(new UnitOfWork("DefaultConnection"));
        MappersForDM _mappers = new MappersForDM();

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController() { }

        public ActionResult UserInfo(string name="")
        {
            if (name == "")
                name = User.Identity.Name;
            UserDM user = _mappers.ToUserDM.Map<UserDTO, UserDM>(_service.GetUserByName(name));
            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if(User.Identity.Name == "")
            {
                await SetInitialDataAsync();
                if (ModelState.IsValid)
                {
                    UserDTO currentUser = _service.GetUserByName(model.Email);
                    if(currentUser == null || !(currentUser.IsBanned))
                    {
                        UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                        ClaimsIdentity claim = await _service.Authenticate(userDto);
                        if (claim == null)
                        {
                            ModelState.AddModelError("", "Неверный логин или пароль.");
                        }
                        else
                        {
                            AuthenticationManager.SignOut();
                            AuthenticationManager.SignIn(new AuthenticationProperties
                            {
                                IsPersistent = true
                            }, claim);
                            return RedirectToAction("Main", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Вы были забанены, Вам запрещён доступ");
                    }
                }
            }
            else
            {
                UserDTO user = _service.GetUserByName(User.Identity.Name);
                if(user.IsBanned)
                {
                    ModelState.AddModelError("", "Вы были забанены, Вам запрещён доступ");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Address = model.Address,
                    Name = model.Name,
                    Role = "user",
                    PhoneNumber = model.PhoneNumber
                };
                if (model.Images != null)
                {
                    foreach (var i in model.Images)
                    {
                        ImageDTO image = new ImageDTO();
                        image.Text = model.Alt;
                        using (var reader = new BinaryReader(i.InputStream))
                            image.Photo = reader.ReadBytes(i.ContentLength);
                        userDto.Image = image;
                    }
                }
                else
                {
                    foreach (var i in model.ImagesInDatebase)
                    {
                        userDto.Image = _mappers.ToImageDTO.Map<ImageDM, ImageDTO>(i);
                    }
                }
                OperationDetails operationDetails = await _service.Create(userDto);
                if (operationDetails.Succedeed)
                    return RedirectToAction("Login", new LoginModel() { Email = model.Email, Password = model.Password});
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        private async Task SetInitialDataAsync()
        {
            await _service.SetInitialData(new UserDTO
            {
                Email = "someemail@gmail.com",
                UserName = "someemail@gmail.com",
                Password = "password",
                Name = "Liman Viacheslav Vitalievich",
                Address = "ул. Спортивная, д.30, кв.75",
                Role = "admin",
            }, new List<string> { "user", "admin", "manager" });
        }
    }
}