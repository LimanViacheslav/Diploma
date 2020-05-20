using Microsoft.AspNet.Identity;
using SkinShop.BLL.Identity.IdentityDTO;
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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.Services
{
    public class AccountService: IAccountService
    {
        IUnitOfWorK Database { get; set; }
        MappersForDTO _mappers = new MappersForDTO();

        public AccountService(IUnitOfWorK uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            User user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new User { Email = userDto.Email, UserName = userDto.Email, PhoneNumber = userDto.PhoneNumber, Name = userDto.Name, Adres = userDto.Address };
                ClientProfile clientProfile = new ClientProfile { Id = user.Id };
                clientProfile.Basket = new Basket();
                clientProfile.Favorites = new Favorites();
                Database.ClientManager.Create(clientProfile);
                user.Client = clientProfile;
                if(userDto.Image != null)
                {
                    user.Image = _mappers.ToImage.Map<ImageDTO, Image>(userDto.Image);
                }
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                Database.Save();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            User user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public async Task<OperationDetails> CreateEmployee(UserDTO userDTO)
        {
            User user = await Database.UserManager.FindByEmailAsync(userDTO.Email);
            if (user == null)
            {
                user = LocalCreate(userDTO);
                var result = await Database.UserManager.CreateAsync(user, userDTO.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                await Database.UserManager.AddToRoleAsync(user.Id, userDTO.Role);
                Database.Save();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Менеджер с таким логином уже существует", "Email");
            }
        }

        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        public User LocalCreate(UserDTO userDTO)
        {
            User user = new User
            {
                Email = userDTO.Email,
                UserName = userDTO.Email,
                PhoneNumber = userDTO.PhoneNumber,
                Name = userDTO.Name,
            };
            if (userDTO.Image != null)
            {
                Image image = Database.Images.Find(x => x.Photo == userDTO.Image.Photo).FirstOrDefault();
                if (image == null)
                    image = new Image() { Photo = userDTO.Image.Photo, Text = userDTO.Image.Text };
                user.Image = image;
            }
            return user;
        }

        public UserDTO GetUserByName(string name)
        {
            User user = Database.ClientManager.FindUser(u => u.Email == name);
            return _mappers.ToUserDTO.Map<User, UserDTO>(user);
        }
    }
}
