using SkinShop.BLL.Identity.IdentityDTO;
using SkinShop.BLL.Identity.Infrastructure;
using SkinShop.BLL.SkinShop.Services;
using SkinShop.BLL.SkinShop.SkinShopDTO;
using SkinShop.Mappers;
using SkinShop.Models;
using SkinShop.Models.Account;
using SkinShop.Models.SkinShop;
using SkinShop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Web.SessionState;
using System.Linq;
using SkinShop.BLL.SkinShop.Interfaces;
using SkinShop.DL.Interfaces.SkinShop;
using SkinShop.DL.Repositories.SkinShop;
using SkinShop.DL.Entities.SkinShop;

namespace SkinShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        IAdminService _adminService = new AdminService(new UnitOfWork("DefaultConnection"));
        IAccountService _accountService = new AccountService(new UnitOfWork("DefaultConnection"));
        MappersForDM _mappers = new MappersForDM();

        public AdminController() { }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpGet]
        public ActionResult UpdateSkin(int id = 0)
        {
            ProductDM product = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(_adminService.GetProduct(id));
            SkinDM result = _mappers.ToSkinDM.Map<SkinDTO, SkinDM>(_adminService.GetSkin(product.FromTableId));
            SkinCreateVM skin = new SkinCreateVM()
            {
                Description = product.Description,
                Game = result.Game.Name,
                Name = product.Name,
                Price = product.Price,
                Sale = product.Sale,
                SkinRarity = result.SkinRarity.RarityName,
                SkinType = result.Properties.FirstOrDefault().Name
            };
            if(product.Images != null && product.Images.FirstOrDefault() != null)
            {
                skin.ImagesInDatebase = new List<ImageDM>();
                foreach(var i in product.Images)
                {
                    skin.ImagesInDatebase.Add(i);
                    skin.Alt = i.Text;
                }
            }
            List<GameDM> games = _mappers.ToGameDM.Map<ICollection<GameDTO>, List<GameDM>>(_adminService.GetGames());
            var gameItems = games.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<SkinRaretyDM> skinRar = _mappers.ToSkinRarityDM.Map<ICollection<SkinRarityDTO>, List<SkinRaretyDM>>(_adminService.GetSkinRarities());
            var skinRarityItems = skinRar.Select(x => new SelectListItem() { Text = x.RarityName, Value = x.RarityName }).ToList();
            skin.Games = gameItems;
            skin.SkinRarities = skinRarityItems;
            return View("CreateSkin", skin);
        }

        [HttpGet]
        public ActionResult CreateSkin()
        {
            List<GameDM> games = _mappers.ToGameDM.Map<ICollection<GameDTO>, List<GameDM>>(_adminService.GetGames());
            var gameItems = games.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<SkinRaretyDM> skinRar = _mappers.ToSkinRarityDM.Map<ICollection<SkinRarityDTO>, List<SkinRaretyDM>>(_adminService.GetSkinRarities());
            var skinRarityItems = skinRar.Select(x => new SelectListItem() { Text = x.RarityName, Value = x.RarityName }).ToList();
            SkinCreateVM skin = new SkinCreateVM();
            skin.Games = gameItems;
            skin.SkinRarities = skinRarityItems;
            return View(skin);
        }

        [HttpPost]
        public ActionResult CreateSkin(SkinCreateVM item)
        {
            if (ModelState.IsValid)
            {
                ProductDTO product = new ProductDTO() { Name = item.Name, Price = item.Price, Sale = item.Sale };
                SkinDTO skin = new SkinDTO();
                if (item.Images != null)
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.Images)
                    {
                        ImageDTO image = new ImageDTO();
                        image.Text = item.Alt;
                        using (var reader = new BinaryReader(i.InputStream))
                            image.Photo = reader.ReadBytes(i.ContentLength);
                        product.Images.Add(image);
                    }
                }
                else
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.ImagesInDatebase)
                    {
                        product.Images.Add(_mappers.ToImageDTO.Map<ImageDM, ImageDTO>(i));
                    }
                }
                if (item.Game != "")
                    skin.Game = new GameDTO() { Name = item.Game };
                if (item.SkinRarity != "")
                {
                    SkinRarityDTO skinRarity = new SkinRarityDTO() { RarityName = item.SkinRarity };
                    skin.SkinRarity = skinRarity;
                }
                skin.Properties = new List<PropertyDTO>();
                if (item.SkinType != "")
                {
                    PropertyDTO skinType = new PropertyDTO() { Name = "Тип скина", Value = item.SkinType };
                    skin.Properties = new List<PropertyDTO>();
                    skin.Properties.Add(skinType);
                }

                for (int i = 0; i < item.PropertyNames.Count; i++)
                {
                    skin.Properties.Add(new PropertyDTO() { Name = item.PropertyNames[i], Value = item.PropertyValues[i] });
                }

                if (item.Description != "")
                    product.Description = item.Description;
                OperationDetails result = _adminService.CreateSkin(product, skin, product.Name);
                ViewBag.Result = result.Message;
                ViewBag.Status = result.Succedeed;
                List<GameDM> games = _mappers.ToGameDM.Map<ICollection<GameDTO>, List<GameDM>>(_adminService.GetGames());
                var gameItems = games.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
                List<SkinRaretyDM> skinRar = _mappers.ToSkinRarityDM.Map<ICollection<SkinRarityDTO>, List<SkinRaretyDM>>(_adminService.GetSkinRarities());
                var skinRarityItems = skinRar.Select(x => new SelectListItem() { Text = x.RarityName, Value = x.RarityName }).ToList();
                item.Games = gameItems;
                item.SkinRarities = skinRarityItems;
                return View(item);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Message(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpGet]
        public ActionResult UpdateGame(int id = 0)
        {
            ProductDM product = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(_adminService.GetProduct(id));
            GameDM result = _mappers.ToGameDM.Map<GameDTO, GameDM>(_adminService.GetGame(product.FromTableId));
            GameCreateVM game = new GameCreateVM()
            {
                IsThingGame = result.IsThingGame,
                Name = product.Name,
                SystemRequirements = result.SystemRequirements,
                Description = product.Description,
                Sale = product.Sale,
                Price = product.Price
            };
            game.GameURL = result.Properties.First(g => g.Name == "Ссылка на игру").Value;
            game.Type = result.Properties.First(g => g.Name == "Тип игры").Value;
            game.Genre = result.Properties.First(g => g.Name == "Жанр игры").Value;
            if (product.Images != null && product.Images.FirstOrDefault() != null)
            {
                game.ImagesInDatebase = new List<ImageDM>();
                foreach (var i in product.Images)
                {
                    game.ImagesInDatebase.Add(i);
                    game.Alt = i.Text;
                }
            }
            return View("CreateGame", game);
        }

        [HttpGet]
        public ActionResult CreateGame()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGame(GameCreateVM item)
        {
            if (ModelState.IsValid)
            {
                ProductDTO product = new ProductDTO() { Name = item.Name, Description = item.Description, Price = item.Price, Sale = item.Sale };
                GameDTO game = new GameDTO();
                if (item.Images != null)
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.Images)
                    {
                        ImageDTO image = new ImageDTO();
                        image.Text = item.Alt;
                        using (var reader = new BinaryReader(i.InputStream))
                            image.Photo = reader.ReadBytes(i.ContentLength);
                        product.Images.Add(image);
                    }
                }
                else
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.ImagesInDatebase)
                    {
                        product.Images.Add(_mappers.ToImageDTO.Map<ImageDM, ImageDTO>(i));
                    }
                }
                game.IsThingGame = item.IsThingGame;
                game.SystemRequirements = item.SystemRequirements;
                game.Properties = new List<PropertyDTO>();
                game.Properties.Add(new PropertyDTO() { Name = "Тип игры", Value = item.Type });
                game.Properties.Add(new PropertyDTO() { Name = "Ссылка на игру", Value = item.GameURL });
                game.Properties.Add(new PropertyDTO() { Name = "Жанр игры", Value = item.Genre });

                for(int i = 0; i < item.PropertyNames.Count; i++)
                {
                    game.Properties.Add(new PropertyDTO() { Name = item.PropertyNames[i], Value = item.PropertyValues[i] });
                }

                OperationDetails result = _adminService.CreateGame(product, game, product.Name);
                ViewBag.Result = result.Message;
                ViewBag.Status = result.Succedeed;
                return View();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEmployee(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Address = model.Address,
                    Name = model.Name,
                    Role = "manager",
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
                OperationDetails result = await _adminService.CreateEmployee(userDto);
                if (result.Succedeed)
                {
                    ViewBag.Result = result.Message;
                    return RedirectToAction("Users", "Admin");
                }
                else
                    ModelState.AddModelError(result.Property, result.Message);
            }
            return RedirectToAction("Register", "Account");
        }

        [HttpGet]
        public ActionResult UpdateCloth(int id = 0)
        {
            ProductDM product = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(_adminService.GetProduct(id));
            ClothDM result = _mappers.ToClothDM.Map<ClothDTO, ClothDM>(_adminService.GetCloth(product.FromTableId));
            CreateClothVM cloth = new CreateClothVM()
            {
                Name = product.Name,
                Description = product.Description,
                Sale = product.Sale,
                Price = product.Price,
                Composition = result.Composition,
                Type = result.Type
            };
            if (result.ForMen)
                cloth.ForMen = "Для мужчин";
            else
                cloth.ForMen = "Для женщин";
            cloth.Properties = result.Properties;
            cloth.SelectedColors = new List<string>();
            foreach (var c in result.Colors)
            {
                cloth.SelectedColors.Add(c.Name);
            }
            cloth.Sizes = new List<string>();
            foreach(var s in result.Sizes)
            {
                cloth.Sizes.Add(s.Data);
            }
            if (product.Images != null && product.Images.FirstOrDefault() != null)
            {
                cloth.ImagesInDatebase = new List<ImageDM>();
                foreach (var i in product.Images)
                {
                    cloth.ImagesInDatebase.Add(i);
                    cloth.Alt = i.Text;
                }
            }
            List<ColorDM> colors = _mappers.ToColorDM.Map<ICollection<ColorDTO>, List<ColorDM>>(_adminService.GetColors());
            var colorItems = colors.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            cloth.Colors = colorItems;
            return View("CreateCloth", cloth);
        }

        [HttpGet]
        public ActionResult CreateCloth()
        {
            List<ColorDM> colors = _mappers.ToColorDM.Map<ICollection<ColorDTO>, List<ColorDM>>(_adminService.GetColors());
            var colorItems = colors.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            CreateClothVM cloth = new CreateClothVM();
            cloth.Colors = colorItems;
            return View(cloth);
        }

        [HttpPost]
        public ActionResult CreateCloth(CreateClothVM item)
        {
            if (ModelState.IsValid)
            {
                ProductDTO product = new ProductDTO() { Name = item.Name, Description = item.Description, Price = item.Price, Sale = item.Sale };
                ClothDTO cloth = new ClothDTO();
                if (item.Images != null)
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.Images)
                    {
                        ImageDTO image = new ImageDTO();
                        image.Text = item.Alt;
                        using (var reader = new BinaryReader(i.InputStream))
                            image.Photo = reader.ReadBytes(i.ContentLength);
                        product.Images.Add(image);
                    }
                }
                else
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.ImagesInDatebase)
                    {
                        product.Images.Add(_mappers.ToImageDTO.Map<ImageDM, ImageDTO>(i));
                    }
                }
                cloth.Composition = item.Composition;
                if (item.ForMen == "Для мужчин")
                    cloth.ForMen = true;
                else
                    cloth.ForMen = false;
                cloth.Name = item.Name;
                cloth.Type = item.Type;
                cloth.Properties = new List<PropertyDTO>();
                for (int i = 0; i < item.PropertyNames.Count; i++)
                {
                    cloth.Properties.Add(new PropertyDTO() { Name = item.PropertyNames[i], Value = item.PropertyValues[i] });
                }
                cloth.Colors = new List<ColorDTO>();
                foreach(var i in item.SelectedColors)
                {
                    cloth.Colors.Add(new ColorDTO() { Name = i });
                }
                cloth.Sizes = new List<StringDataDTO>();
                foreach (var i in item.Sizes)
                {
                    cloth.Sizes.Add(new StringDataDTO() { Data = i });
                }
                OperationDetails result = _adminService.CreateCloth(product, cloth, product.Name);
                ViewBag.Result = result.Message;
                ViewBag.Status = result.Succedeed;
                List<ColorDM> colors = _mappers.ToColorDM.Map<ICollection<ColorDTO>, List<ColorDM>>(_adminService.GetColors());
                var colorItems = colors.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
                item.Colors = colorItems;
                return View(item);
            }
            return View();
        }

        [HttpGet]
        public ActionResult UpdateColor(int id = 0)
        {
            ColorDTO color = _adminService.GetColor(id);
            CreateColorVM item = new CreateColorVM() { Name = color.Name, ColorValue = color.ColorValue };
            return View("CreateColor", color);
        }

        [HttpGet]
        public ActionResult CreateColor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateColor(CreateColorVM item)
        {
            if (ModelState.IsValid)
            {
                ColorDTO color = new ColorDTO() { Name = item.Name, ColorValue = item.ColorValue};
                OperationDetails result = _adminService.CreateColor(color);
                ViewBag.Result = result.Message;
                ViewBag.Status = result.Succedeed;
                return View(item);
            }
            return View();
        }

        [HttpGet]
        public ActionResult UpdateComputerComponent(int id = 0)
        {
            ProductDM product = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(_adminService.GetProduct(id));
            ComputerComponentDM result = _mappers.ToComputerComponentDM.Map<ComputerComponentDTO, ComputerComponentDM>(_adminService.GetComputerComponent(product.FromTableId));
            CreateComputerComponentVM compComp = new CreateComputerComponentVM()
            {
                Name = product.Name,
                Description = product.Description,
                Sale = product.Sale,
                Price = product.Price
            };
            switch (result.Type)
            {
                case ComponentType.VideoCard:
                    compComp.Type = "Видеокарта";
                    break;
                case ComponentType.MotherBoard:
                    compComp.Type = "Материнская плата";
                    break;
                case ComponentType.PowerSupply:
                    compComp.Type = "Блок питания";
                    break;
                case ComponentType.Processor:
                    compComp.Type = "Процессор";
                    break;
                case ComponentType.RAM:
                    compComp.Type = "Оперативная память";
                    break;
                case ComponentType.ROM:
                    compComp.Type = "Постоянная память";
                    break;
                case ComponentType.SystemBlock:
                    compComp.Type = "Системный блок";
                    break;
            }
            if (product.Images != null && product.Images.FirstOrDefault() != null)
            {
                compComp.ImagesInDatebase = new List<ImageDM>();
                foreach (var i in product.Images)
                {
                    compComp.ImagesInDatebase.Add(i);
                    compComp.Alt = i.Text;
                }
            }
            return View("CreateComputerComponent", compComp);
        }

        [HttpGet]
        public ActionResult CreateComputerComponent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateComputerComponent(CreateComputerComponentVM item)
        {
            if (ModelState.IsValid)
            {
                ProductDTO product = new ProductDTO() { Name = item.Name, Description = item.Description, Price = item.Price, Sale = item.Sale };
                ComputerComponentDTO compComp = new ComputerComponentDTO();
                if (item.Images != null)
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.Images)
                    {
                        ImageDTO image = new ImageDTO();
                        image.Text = item.Alt;
                        using (var reader = new BinaryReader(i.InputStream))
                            image.Photo = reader.ReadBytes(i.ContentLength);
                        product.Images.Add(image);
                    }
                }
                else
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.ImagesInDatebase)
                    {
                        product.Images.Add(_mappers.ToImageDTO.Map<ImageDM, ImageDTO>(i));
                    }
                }
                switch(item.Type)
                {
                    case "Видеокарта":
                        compComp.Type = ComponentType.VideoCard;
                        break;
                    case "Материнская плата":
                        compComp.Type = ComponentType.MotherBoard;
                        break;
                    case "Блок питания":
                        compComp.Type = ComponentType.PowerSupply;
                        break;
                    case "Процессор":
                        compComp.Type = ComponentType.Processor;
                        break;
                    case "Оперативная память":
                        compComp.Type = ComponentType.RAM;
                        break;
                    case "Постоянная память":
                        compComp.Type = ComponentType.ROM;
                        break;
                    case "Системный блок":
                        compComp.Type = ComponentType.SystemBlock;
                        break;
                }
                compComp.Properties = new List<PropertyDTO>();
                for (int i = 0; i < item.PropertyNames.Count; i++)
                {
                    compComp.Properties.Add(new PropertyDTO() { Name = item.PropertyNames[i], Value = item.PropertyValues[i] });
                }
                OperationDetails result = _adminService.CreateComputerComponent(product, compComp, product.Name);
                ViewBag.Result = result.Message;
                ViewBag.Status = result.Succedeed;
                return View();
            }
            return View();
        }

        [HttpGet]
        public ActionResult UpdateComputer(int id = 0)
        {
            ProductDM product = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(_adminService.GetProduct(id));
            ComputerDM result = _mappers.ToComputerDM.Map<ComputerDTO, ComputerDM>(_adminService.GetComputer(product.FromTableId));
            CreateComputerVM comp = new CreateComputerVM()
            {
                Name = product.Name,
                Description = product.Description,
                Sale = product.Sale,
                Price = product.Price,
                SelectedMotherBoard = result.MotherBoard.Name,
                SelectedPowerSupply = result.PowerSupply.Name,
                SelectedProcessor = result.Processor.Name,
                SelectedSystemBlock = result.SystemBlock.Name
            };
            comp.SelectedRAM = new List<string>();
            foreach(var i in result.RAM)
            {
                comp.SelectedRAM.Add(i.Name);
            }
            comp.SelectedROM = new List<string>();
            foreach (var i in result.ROM)
            {
                comp.SelectedROM.Add(i.Name);
            }
            comp.SelectedVideoCard = new List<string>();
            foreach (var i in result.VideoCard)
            {
                comp.SelectedVideoCard.Add(i.Name);
            }
            if (product.Images != null && product.Images.FirstOrDefault() != null)
            {
                comp.ImagesInDatebase = new List<ImageDM>();
                foreach (var i in product.Images)
                {
                    comp.ImagesInDatebase.Add(i);
                    comp.Alt = i.Text;
                }
            }
            List<ComputerComponentDM> compMotherBoard = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.MotherBoard));
            var compMotherBoardItems = compMotherBoard.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<ComputerComponentDM> compPowerSupply = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.PowerSupply));
            var compPowerSupplyItems = compPowerSupply.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<ComputerComponentDM> compProcessor = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.Processor));
            var compProcessorItems = compProcessor.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<ComputerComponentDM> compRAM = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.RAM));
            var compRAMItems = compRAM.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<ComputerComponentDM> compROM = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.ROM));
            var compROMItems = compROM.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<ComputerComponentDM> compSystemBlock = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.SystemBlock));
            var compSystemBlockItems = compSystemBlock.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<ComputerComponentDM> compVideoCard = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.VideoCard));
            var compVideoCardItems = compVideoCard.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            comp.MotherBoards = compMotherBoardItems;
            comp.PowerSupplies = compPowerSupplyItems;
            comp.Processors = compProcessorItems;
            comp.RAMs = compRAMItems;
            comp.ROMs = compROMItems;
            comp.SystemBlocks = compSystemBlockItems;
            comp.VideoCards = compVideoCardItems;
            return View("CreateComputer", comp);
        }

        [HttpGet]
        public ActionResult CreateComputer()
        {
            List<ComputerComponentDM> compMotherBoard = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.MotherBoard));
            var compMotherBoardItems = compMotherBoard.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<ComputerComponentDM> compPowerSupply = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.PowerSupply));
            var compPowerSupplyItems = compPowerSupply.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<ComputerComponentDM> compProcessor = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.Processor));
            var compProcessorItems = compProcessor.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<ComputerComponentDM> compRAM = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.RAM));
            var compRAMItems = compRAM.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<ComputerComponentDM> compROM = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.ROM));
            var compROMItems = compROM.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<ComputerComponentDM> compSystemBlock = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.SystemBlock));
            var compSystemBlockItems = compSystemBlock.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            List<ComputerComponentDM> compVideoCard = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.VideoCard));
            var compVideoCardItems = compVideoCard.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            CreateComputerVM comp = new CreateComputerVM();
            comp.MotherBoards = compMotherBoardItems;
            comp.PowerSupplies = compPowerSupplyItems;
            comp.Processors = compProcessorItems;
            comp.RAMs = compRAMItems;
            comp.ROMs = compROMItems;
            comp.SystemBlocks = compSystemBlockItems;
            comp.VideoCards = compVideoCardItems;
            return View(comp);
        }

        [HttpPost]
        public ActionResult CreateComputer(CreateComputerVM item)
        {
            if (ModelState.IsValid)
            {
                ProductDTO product = new ProductDTO() { Name = item.Name, Description = item.Description, Price = item.Price, Sale = item.Sale };
                ComputerDTO computer = new ComputerDTO();
                if (item.Images != null)
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.Images)
                    {
                        ImageDTO image = new ImageDTO();
                        image.Text = item.Alt;
                        using (var reader = new BinaryReader(i.InputStream))
                            image.Photo = reader.ReadBytes(i.ContentLength);
                        product.Images.Add(image);
                    }
                }
                else
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.ImagesInDatebase)
                    {
                        product.Images.Add(_mappers.ToImageDTO.Map<ImageDM, ImageDTO>(i));
                    }
                }
                computer.MotherBoard = new ComputerComponentDTO() { Name = item.SelectedMotherBoard };
                computer.PowerSupply = new ComputerComponentDTO() { Name = item.SelectedPowerSupply };
                computer.Processor = new ComputerComponentDTO() { Name = item.SelectedProcessor };
                computer.SystemBlock = new ComputerComponentDTO() { Name = item.SelectedSystemBlock };
                computer.RAM = new List<ComputerComponentDTO>();
                foreach (var i in item.SelectedRAM)
                {
                    computer.RAM.Add(new ComputerComponentDTO(){Name = i});
                }
                computer.ROM = new List<ComputerComponentDTO>();
                foreach (var i in item.SelectedROM)
                {
                    computer.ROM.Add(new ComputerComponentDTO() { Name = i });
                }
                computer.VideoCard = new List<ComputerComponentDTO>();
                foreach (var i in item.SelectedVideoCard)
                {
                    computer.VideoCard.Add(new ComputerComponentDTO() { Name = i });
                }
                computer.Properties = new List<PropertyDTO>();
                for (int i = 0; i < item.PropertyNames.Count; i++)
                {
                    computer.Properties.Add(new PropertyDTO() { Name = item.PropertyNames[i], Value = item.PropertyValues[i] });
                }
                OperationDetails result = _adminService.CreateComputer(product, computer, product.Name);
                ViewBag.Result = result.Message;
                ViewBag.Status = result.Succedeed;
                List<ComputerComponentDM> compMotherBoard = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.MotherBoard));
                var compMotherBoardItems = compMotherBoard.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
                List<ComputerComponentDM> compPowerSupply = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.PowerSupply));
                var compPowerSupplyItems = compPowerSupply.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
                List<ComputerComponentDM> compProcessor = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.Processor));
                var compProcessorItems = compProcessor.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
                List<ComputerComponentDM> compRAM = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.RAM));
                var compRAMItems = compRAM.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
                List<ComputerComponentDM> compROM = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.ROM));
                var compROMItems = compROM.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
                List<ComputerComponentDM> compSystemBlock = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.SystemBlock));
                var compSystemBlockItems = compSystemBlock.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
                List<ComputerComponentDM> compVideoCard = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.GetComputerComponentsByType(ComponentType.VideoCard));
                var compVideoCardItems = compVideoCard.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
                item.MotherBoards = compMotherBoardItems;
                item.PowerSupplies = compPowerSupplyItems;
                item.Processors = compProcessorItems;
                item.RAMs = compRAMItems;
                item.ROMs = compROMItems;
                item.SystemBlocks = compSystemBlockItems;
                item.VideoCards = compVideoCardItems;
                return View(item);
            }
            return View();
        }

        [HttpGet]
        public ActionResult CreateContainer(string table)
        {
            List<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetAllProducts());
            var prods = products.Select(x => new SelectListItem()
            { Text = x.Name + " | " + (x.Sale!=0?(x.Price - (x.Price * x.Sale)/100) : x.Price) + " | " + x.Table.ToString()
            , Value = x.Name
            }).ToList();
            CreateContainerVM cont = new CreateContainerVM();
            cont.Products = prods;
            return View(cont);
        }

        [HttpPost]
        public ActionResult CreateContainer(CreateContainerVM item)
        {
            if (ModelState.IsValid)
            {
                ProductDTO product = new ProductDTO() { Name = item.Name, Description = item.Description, Price = item.Price, Sale = item.Sale };
                ContainerDTO container = new ContainerDTO();
                if (item.Images != null)
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.Images)
                    {
                        ImageDTO image = new ImageDTO();
                        image.Text = item.Alt;
                        using (var reader = new BinaryReader(i.InputStream))
                            image.Photo = reader.ReadBytes(i.ContentLength);
                        product.Images.Add(image);
                    }
                }
                else
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.ImagesInDatebase)
                    {
                        product.Images.Add(_mappers.ToImageDTO.Map<ImageDM, ImageDTO>(i));
                    }
                }
                container.Products = new List<ProductDTO>();
                foreach(var i in item.SelectedProducts)
                {
                    container.Products.Add(new ProductDTO() { Name = i });
                }
                container.ChanseForLegendary = item.ChanseForLegendary;
                container.ChanseForRare = item.ChanseForRare;
                container.MaxRare = item.MaxRare;
                container.MinRare = item.MinRare;
                container.Name = item.Name;
                container.Type = item.Type;
                container.TypeOfHard = item.TypeOfHard;
                container.Properties = new List<PropertyDTO>();
                for (int i = 0; i < item.PropertyNames.Count; i++)
                {
                    container.Properties.Add(new PropertyDTO() { Name = item.PropertyNames[i], Value = item.PropertyValues[i] });
                }
                OperationDetails result = _adminService.CreateContainer(product, container, product.Name);
                ViewBag.Result = result.Message;
                ViewBag.Status = result.Succedeed;
                List<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetAllProducts());
                var prods = products.Select(x => new SelectListItem()
                {
                    Text = x.Name + " | " + (x.Sale != 0 ? (x.Price - (x.Price * x.Sale) / 100) : x.Price) + " | " + x.Table.ToString()
                ,
                    Value = x.Name
                }).ToList();
                item.Products = prods;
                return View(item);
            }
            return View(item);
        }

        [HttpGet]
        public ActionResult CreateOther()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateOther(CreateOtherVM item)
        {
            if (ModelState.IsValid)
            {
                ProductDTO product = new ProductDTO() { Name = item.Name, Description = item.Description, Price = item.Price, Sale = item.Sale };
                OtherDTO other = new OtherDTO();
                if (item.Images != null)
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.Images)
                    {
                        ImageDTO image = new ImageDTO();
                        image.Text = item.Alt;
                        using (var reader = new BinaryReader(i.InputStream))
                            image.Photo = reader.ReadBytes(i.ContentLength);
                        product.Images.Add(image);
                    }
                }
                else
                {
                    product.Images = new List<ImageDTO>();
                    foreach (var i in item.ImagesInDatebase)
                    {
                        product.Images.Add(_mappers.ToImageDTO.Map<ImageDM, ImageDTO>(i));
                    }
                }
                other.Name = item.Name;
                other.Type = item.Type;
                other.Properties = new List<PropertyDTO>();
                for (int i = 0; i < item.PropertyNames.Count; i++)
                {
                    other.Properties.Add(new PropertyDTO() { Name = item.PropertyNames[i], Value = item.PropertyValues[i] });
                }
                OperationDetails result = _adminService.CreateOther(product, other, product.Name);
                ViewBag.Result = result.Message;
                ViewBag.Status = result.Succedeed;
                return View(item);
            }
            return View(item);
        }

        [HttpGet]
        public ActionResult CreateSkinRarety()
        {
            CreateSkinRarety rarety = new CreateSkinRarety();
            List<ColorDTO> colors = _adminService.GetColors().ToList();
            rarety.Colors = colors.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            return View(rarety);
        }

        [HttpPost]
        public ActionResult CreateSkinRarety(CreateSkinRarety item)
        {
            if (ModelState.IsValid)
            {
                SkinRarityDTO rarety = new SkinRarityDTO();
                rarety.RarityName = item.RaretyName;
                List<ColorDTO> colors = new List<ColorDTO>();
                foreach(var i in item.SelectedColors)
                {
                    colors.Add(new ColorDTO() { Name = i });
                }
                rarety.Colors = colors;
                OperationDetails result = _adminService.CreateSkinRarity(rarety, rarety.RarityName);
                ViewBag.Result = result.Message;
                ViewBag.Status = result.Succedeed;
                List<ColorDTO> colorsInDB = _adminService.GetColors().ToList();
                item.Colors = colorsInDB.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
                return View(item);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Users()
        {
            List<UserDM> users = _mappers.ToUserDM.Map<IEnumerable<UserDTO>, List<UserDM>>(_adminService.GetUsers());
            users = (from u in users
                    where u.UserName != User.Identity.Name
                    select u).ToList();
            return View(users);
        }

        public ActionResult UserFilters(string userName = "", string[] roles = null)
        {
            List<UserDM> result = _mappers.ToUserDM.Map<IEnumerable<UserDTO>, List<UserDM>>(_adminService.GetUsers());
            result = (from r in result
                      where r.UserName != User.Identity.Name
                      select r).ToList();
            if(userName != "")
            {
                result = (from t in result
                         where t.Name.Contains(userName) || t.Email.Contains(userName)
                         select t).ToList();
            }
            
            if(roles != null)
            {
                List<UserDM> _users = new List<UserDM>();
                foreach(var i in roles)
                {
                    List<UserDM> localUsers = (from t in result
                                              where t.Role == i
                                              select t).ToList();
                    _users.AddRange(localUsers);
                }
                result = _users;
            }

            return PartialView(result);
        }

        public ActionResult Ban(string id)
        {
            if (id == "")
                return RedirectToAction("PageNotFound", "Home");
            _adminService.Ban(id);
            return RedirectToAction("Users");
        }

        public ActionResult Unban(string id)
        {
            if (id == "")
                return RedirectToAction("PageNotFound", "Home");
            _adminService.Unban(id);
            return RedirectToAction("Users");
        }

        public ActionResult DeleteSkin(int? id)
        {
            if(id != null)
            {
                ProductDTO product = _adminService.GetProduct(Convert.ToInt32(id));
                OperationDetails result = _adminService.SoftDelete(Tables.Product, Convert.ToInt32(id));
                result = _adminService.SoftDelete(Tables.Skin, product.FromTableId);
                if (result.Succedeed)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("PageNotFound", "Home");
        }

        public ActionResult DeleteGame(int? id)
        {
            if (id != null)
            {
                ProductDTO product = _adminService.GetProduct(Convert.ToInt32(id));
                OperationDetails result = _adminService.SoftDelete(Tables.Product, Convert.ToInt32(id));
                result = _adminService.SoftDelete(Tables.Game, product.FromTableId);
                if (result.Succedeed)
                {
                    return RedirectToAction("Games", "Home");
                }
            }
            return RedirectToAction("PageNotFound", "Home");
        }

        public ActionResult DeleteCloth(int? id)
        {
            if (id != null)
            {
                ProductDTO product = _adminService.GetProduct(Convert.ToInt32(id));
                OperationDetails result = _adminService.SoftDelete(Tables.Product, Convert.ToInt32(id));
                result = _adminService.SoftDelete(Tables.Cloth, product.FromTableId);
                if (result.Succedeed)
                {
                    return RedirectToAction("Clothes", "Home");
                }
            }
            return RedirectToAction("PageNotFound", "Home");
        }

        public ActionResult DeleteComputerComponent(int? id)
        {
            if (id != null)
            {
                ProductDTO product = _adminService.GetProduct(Convert.ToInt32(id));
                OperationDetails result = _adminService.SoftDelete(Tables.Product, Convert.ToInt32(id));
                result = _adminService.SoftDelete(Tables.ComputerComponent, product.FromTableId);
                if (result.Succedeed)
                {
                    return RedirectToAction("ComputerComponents", "Home");
                }
            }
            return RedirectToAction("PageNotFound", "Home");
        }

        public ActionResult DeleteComputer(int? id)
        {
            if (id != null)
            {
                ProductDTO product = _adminService.GetProduct(Convert.ToInt32(id));
                OperationDetails result = _adminService.SoftDelete(Tables.Product, Convert.ToInt32(id));
                result = _adminService.SoftDelete(Tables.Computer, product.FromTableId);
                if (result.Succedeed)
                {
                    return RedirectToAction("Computers", "Home");
                }
            }
            return RedirectToAction("PageNotFound", "Home");
        }

        public ActionResult DeleteContainer(int? id)
        {
            if (id != null)
            {
                ProductDTO product = _adminService.GetProduct(Convert.ToInt32(id));
                OperationDetails result = _adminService.SoftDelete(Tables.Product, Convert.ToInt32(id));
                result = _adminService.SoftDelete(Tables.Container, product.FromTableId);
                if (result.Succedeed)
                {
                    return RedirectToAction("Containers", "Home");
                }
            }
            return RedirectToAction("PageNotFound", "Home");
        }

        public ActionResult DeleteOther(int? id)
        {
            if (id != null)
            {
                ProductDTO product = _adminService.GetProduct(Convert.ToInt32(id));
                OperationDetails result = _adminService.SoftDelete(Tables.Product, Convert.ToInt32(id));
                result = _adminService.SoftDelete(Tables.Other, product.FromTableId);
                if (result.Succedeed)
                {
                    return RedirectToAction("Others", "Home");
                }
            }
            return RedirectToAction("PageNotFound", "Home");
        }
    }
}