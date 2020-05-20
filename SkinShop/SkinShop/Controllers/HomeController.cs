using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SkinShop.BLL.SkinShop.SkinShopDTO;
using SkinShop.BLL.SkinShop.Services;
using SkinShop.Mappers;
using SkinShop.Models.SkinShop;
using System;
using SkinShop.Models.ViewModels;
using SkinShop.Models.Account;
using SkinShop.BLL.Identity.IdentityDTO;
using SkinShop.Filters;
using SkinShop.BLL.SkinShop.Interfaces;
using SkinShop.DL.Interfaces.SkinShop;
using SkinShop.DL.Entities.SkinShop;
using SkinShop.DL.Repositories.SkinShop;

namespace SkinShop.Controllers
{
    //[BanAttribute]
    public class HomeController : Controller
    {
        IAdminService _adminService = new AdminService(new UnitOfWork("DefaultConnection"));
        IAccountService _accountService = new AccountService(new UnitOfWork("DefaultConnection"));
        IHomeService _homeService = new HomeService(new UnitOfWork("DefaultConnection"));
        IClientService _clientService = new ClientService(new UnitOfWork("DefaultConnection"));
        MappersForDM _mappers = new MappersForDM();

        public ActionResult Main()
        {
            List<GameDM> games = _mappers.ToGameDM.Map<ICollection<GameDTO>, List<GameDM>>(_adminService.GetGames());
            return View();
        }

        public ActionResult Games(int page = 1)
        {
            List<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.Game));
            foreach (var i in products)
            {
                i.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(i.Id));
            }
            GamesVM result = new GamesVM();
            result.MinPrice = Convert.ToInt32((from t in products
                                               orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                               select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First());
            result.MaxPrice = Convert.ToInt32((from t in products
                                               orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                               select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First()) + 1;
            result.Genres = (from t in _adminService.GetProperties("Жанр игры")
                            select t.Value).Distinct().ToList();

            result.Types = (from t in _adminService.GetProperties("Тип игры")
                            select t.Value).Distinct().ToList();

            int pageSize = 18;

            result.Games = products.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            result.PageInfo = pageInfo;
            return View(result);
        }

        public ActionResult GameFilters(string searchbyname = "", string order = "", string[] types = null, string[] genres = null, int page = 1, string minPrice = "", string maxPrice = "")
        {
            GamesVM result = new GamesVM();
            int pageSize = 18;
            PageInfo pageInfo;

            IEnumerable<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, ICollection<ProductDM>>(_adminService.GetProducts(Goods.Game));
            foreach (var i in products)
            {
                i.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(i.Id));
            }
            if (searchbyname != "")
            {
                products = from t in products
                           where t.Name.Contains(searchbyname)
                           select t;
            }

            if (minPrice != "" && maxPrice != "")
            {
                int min = int.Parse(minPrice);
                int max = int.Parse(maxPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 >= min && t.Price - (t.Price * t.Sale) / 100 <= max) : (t.Price >= min && t.Price <= max)
                           select t;
            }
            else if (minPrice != "")
            {
                int min = int.Parse(minPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 >= min) : (t.Price >= min)
                           select t;
            }
            else if (maxPrice != "")
            {
                int max = int.Parse(maxPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 <= max) : (t.Price <= max)
                           select t;
            }

            List<GameDM> finalGames = _mappers.ToGameDM.Map<ICollection<GameDTO>, List<GameDM>>(_adminService.ProductsIntoGames(_mappers.ToProductDTO.Map<IEnumerable<ProductDM>, List<ProductDTO>>(products)));

            if (types != null)
            {
                List<GameDM> games = new List<GameDM>();
                foreach (var i in types)
                {
                    List<GameDM> localGame;
                    localGame = (from t in finalGames
                                 where t.Properties.TakeWhile(x => x.Value == i && x.Name == "Тип игры").Count() != 0
                                 select t).ToList();
                    games.AddRange(localGame);
                }
                finalGames = games;
            }


            if (genres != null)
            {
                List<GameDM> games = new List<GameDM>();
                foreach (var i in genres)
                {
                    List<GameDM> localGame;
                    localGame = (from t in finalGames
                                 where t.Properties.TakeWhile(x => x.Value == i && x.Name == "Жанр игры").Count() != 0
                                 select t).ToList();
                    games.AddRange(localGame);
                }
                finalGames = games;
            }

            List<ProductDM> localProducts = new List<ProductDM>();
            foreach (var i in products)
            {
                GameDM game = (from s in finalGames
                               where s.Id == i.FromTableId
                               select s).FirstOrDefault();
                if (game != null)
                    localProducts.Add(i);
            }
            products = localProducts;

            if (order != "")
            {
                switch (order)
                {
                    case "По алфавиту А-Я":
                        products = (from t in products
                                    orderby t.Name ascending
                                    select t).ToList();
                        break;
                    case "По алфавиту Я-А":
                        products = (from t in products
                                    orderby t.Name descending
                                    select t).ToList();
                        break;
                    case "По увеличению стоимости":
                        products = (from t in products
                                    orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                    select t).ToList();
                        break;
                    case "По уменьшению стоимости":
                        products = (from t in products
                                    orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                    select t).ToList();
                        break;
                    case "По популярности":
                        products = (from t in products
                                    orderby t.CountSeen + t.CountFavorites descending
                                    select t).ToList();
                        break;
                    case "По рейтингу":
                        products = (from t in products
                                    orderby Convert.ToDouble(t.Comments.Sum(x => x.Assessment)) / Convert.ToDouble(t.Comments.Count) descending
                                    select t).ToList();
                        break;
                    case "По заказам":
                        products = (from t in products
                                    orderby t.CountOrders descending
                                    select t).ToList();
                        break;
                }
            }

            if (page > pageSize)
            {
                return RedirectToAction("PageNotFound");
            }
            result.Games = products.Skip((page - 1) * pageSize).Take(pageSize);
            pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            result.PageInfo = pageInfo;
            if (products != null && products.FirstOrDefault() != null)
            {
                result.MinPrice = Convert.ToInt32((from t in products
                                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                                   select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First());
                result.MaxPrice = Convert.ToInt32((from t in products
                                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                                   select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First()) + 1;
            }
            return PartialView(result);
        }

        public ActionResult GameDetails(int? id, bool fromskin = false)
        {
            if (id != null)
            {
                UpCountSeen(Convert.ToInt32(id));
                ProductDTO product;
                if (fromskin)
                {
                    product = (from pr in _adminService.GetProducts(Goods.Game)
                               where pr.FromTableId == id
                               select pr).FirstOrDefault();
                }
                else
                {
                    product = _adminService.GetProduct(Convert.ToInt32(id));
                }
                GameDTO result = _adminService.GetGame(product.FromTableId);
                if (result != null)
                {
                    GameDetailsVM game = new GameDetailsVM();
                    game.IsGameAlreadyInBasket = false;
                    game.IsGameAlreadyInFavorites = false;
                    game.Game = _mappers.ToGameDM.Map<GameDTO, GameDM>(result);
                    UserDM client = _mappers.ToUserDM.Map<UserDTO, UserDM>(_accountService.GetUserByName(User.Identity.Name));
                    if (client?.Client != null)
                    {
                        foreach (var i in client.Client.Favorites.Products)
                        {
                            if (i.Id == id)
                            {
                                game.IsGameAlreadyInFavorites = true;
                            }
                        }

                        foreach (var i in client.Client.Basket.Products)
                        {
                            if (i.Id == id)
                            {
                                game.IsGameAlreadyInBasket = true;
                                game.IsGameAlreadyInFavorites = true;
                            }
                        }
                    }
                    game.Product = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(product);
                    game.Product.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(product.Id));
                    List<GameDM> games = _mappers.ToGameDM.Map<ICollection<GameDTO>, IEnumerable<GameDM>>(_adminService.GetGames()).ToList();
                    List<ProductDM> localProducts = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.Game));
                    game.OtherGames = new List<ProductDM>();
                    foreach (var i in games)
                    {
                        ProductDM prod = (from s in localProducts
                                          where s.FromTableId == i.Id && s.Id != id
                                          select s).FirstOrDefault();
                        if (prod != null)
                        {
                            prod.Comments = _mappers.ToCommentDM.Map<List<CommentDTO>, ICollection<CommentDM>>(_clientService.GetCommentsForProduct(prod.Id));
                            game.OtherGames.Add(prod);
                        }
                    }
                    return View(game);
                }
            }
            return RedirectToAction("PageNotFound");
        }

        public ActionResult Index(int page = 1, string gameName = "")
        {
            List<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.Skin));
            foreach(var i in products)
            {
                i.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(i.Id));
            }
            SkinsVM result = new SkinsVM();
            result.MinPrice = Convert.ToInt32((from t in products
                                               orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                               select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First());
            result.MaxPrice = Convert.ToInt32((from t in products
                                               orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                               select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First()) + 1;
            result.Games = (from t in _mappers.ToGameDM.Map<ICollection<GameDTO>, IEnumerable<GameDM>>(_adminService.GetGames())
                           select t.Name).ToList();

            result.Types = (from t in _adminService.GetProperties("Тип скина")
                                  select t.Value).Distinct().ToList();

            result.Rareties = (from t in _mappers.ToSkinRarityDM.Map<ICollection<SkinRarityDTO>, IEnumerable<SkinRaretyDM>>(_adminService.GetSkinRarities())
                            select t.RarityName).ToList();
            int pageSize = 18;

            result.Skins = products.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            result.PageInfo = pageInfo;
            result.CheckedGameName = gameName;
            return View(result);
        }

        public ActionResult SkinFilters(string searchbyname = "", string order = "", string[] games = null, string[] types = null, string[] rareties = null, int page = 1, string minPrice = "", string maxPrice = "")
        {
            SkinsVM result = new SkinsVM();
            int pageSize = 18;
            result.CheckedGameName = "";
            PageInfo pageInfo;

            IEnumerable<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, ICollection<ProductDM>>(_adminService.GetProducts(Goods.Skin));
            foreach (var i in products)
            {
                i.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(i.Id));
            }
            if (searchbyname != "")
            {
                products = from t in products
                           where t.Name.Contains(searchbyname)
                        select t;
            }

            if (minPrice != "" && maxPrice != "")
            {
                int min = int.Parse(minPrice);
                int max = int.Parse(maxPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 >= min && t.Price - (t.Price * t.Sale) / 100 <= max) : (t.Price >= min && t.Price <= max)
                        select t;
            }
            else if (minPrice != "")
            {
                int min = int.Parse(minPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 >= min) : (t.Price >= min)
                        select t;
            }
            else if (maxPrice != "")
            {
                int max = int.Parse(maxPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 <= max) : (t.Price <= max)
                        select t;
            }
            List<SkinDM> skins = _mappers.ToSkinDM.Map<ICollection<SkinDTO>, List<SkinDM>>(_adminService.ProductsIntoSkins(_mappers.ToProductDTO.Map<IEnumerable<ProductDM>, List<ProductDTO>>(products)));

            if (games != null)
            {
                List<SkinDM> _skins = new List<SkinDM>();
                foreach (var i in games)
                {
                    List<SkinDM> localSkin;
                    localSkin = (from t in skins
                                 where t.Game.Name == i
                                 select t).ToList();
                    _skins.AddRange(localSkin);
                }
                skins = _skins;
            }

            if (rareties != null)
            {
                List<SkinDM> _skins = new List<SkinDM>();
                foreach (var i in rareties)
                {
                    List<SkinDM> localSkin;
                    localSkin = (from t in skins
                                 where t.SkinRarity.RarityName == i
                                 select t).ToList();
                    _skins.AddRange(localSkin);
                }
                skins = _skins;
            }

            if(types != null)
            {
                List<SkinDM> _skins = new List<SkinDM>();
                foreach (var i in types)
                {
                    List<SkinDM> localSkin;
                    localSkin = (from t in skins
                                 where t.Properties.TakeWhile(x => x.Value == i && x.Name == "Тип скина").Count() != 0
                                 select t).ToList();
                    _skins.AddRange(localSkin);
                }
                skins = _skins;
            }

            List<ProductDM> localProducts = new List<ProductDM>();
            foreach(var i in products)
            {
                SkinDM skin = (from s in skins
                              where s.Id == i.FromTableId
                              select s).FirstOrDefault();
                if (skin != null)
                    localProducts.Add(i);
            }
            products = localProducts;

            if (order != "")
                {
                    switch (order)
                    {
                        case "По алфавиту А-Я":
                            products = (from t in products
                                       orderby t.Name ascending
                                    select t).ToList();
                            break;
                        case "По алфавиту Я-А":
                        products = (from t in products
                                   orderby t.Name descending
                                    select t).ToList();
                            break;
                        case "По увеличению стоимости":
                        products = (from t in products
                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                    select t).ToList();
                            break;
                        case "По уменьшению стоимости":
                        products = (from t in products
                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                    select t).ToList();
                            break;
                        case "По популярности":
                        products = (from t in products
                                   orderby t.CountSeen+t.CountFavorites descending
                                    select t).ToList();
                            break;
                        case "По рейтингу":
                        products = (from t in products
                                   orderby Convert.ToDouble(t.Comments.Sum(x => x.Assessment))/Convert.ToDouble(t.Comments.Count) descending
                                    select t).ToList();
                            break;
                        case "По заказам":
                        products = (from t in products
                                   orderby t.CountOrders descending
                                    select t).ToList();
                            break;
                    }
                }

            if(page > pageSize)
            {
                return RedirectToAction("PageNotFound");
            }
            result.Skins = products.Skip((page - 1) * pageSize).Take(pageSize);
            pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            result.PageInfo = pageInfo;
            if(products != null && products.FirstOrDefault() != null)
            {
                result.MinPrice = Convert.ToInt32((from t in products
                                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                                   select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First());
                result.MaxPrice = Convert.ToInt32((from t in products
                                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                                   select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First()) + 1;
            }

            return PartialView(result);
        }

        public ActionResult SkinDetails(int? id)
        {
            if(id!=null)
            {
                UpCountSeen(Convert.ToInt32(id));
                ProductDTO product = _adminService.GetProduct(Convert.ToInt32(id));
                SkinDTO result = _adminService.GetSkin(product.FromTableId);
                if(result != null)
                {
                    SkinDetailsView skin = new SkinDetailsView();
                    skin.IsSkinAlreadyInBasket = false;
                    skin.IsSkinAlreadyInFavorites = false;
                    skin.Skin = _mappers.ToSkinDM.Map<SkinDTO, SkinDM>(result);
                    UserDM client =_mappers.ToUserDM.Map<UserDTO, UserDM>(_accountService.GetUserByName(User.Identity.Name));
                    if(client?.Client != null)
                    {
                        foreach(var i in client.Client.Favorites.Products)
                        {
                            if(i.Id == id)
                            {
                                skin.IsSkinAlreadyInFavorites = true;
                            }
                        }

                        foreach (var i in client.Client.Basket.Products)
                        {
                            if (i.Id == id)
                            {
                                skin.IsSkinAlreadyInBasket = true;
                                skin.IsSkinAlreadyInFavorites = true;
                            }
                        }
                    }
                    skin.Product = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(product);
                    skin.Product.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(product.Id));
                    List<SkinDM> skins = (from t in _mappers.ToSkinDM.Map<ICollection<SkinDTO>, IEnumerable<SkinDM>>(_adminService.GetSkins())
                                          where t.Game.Name == skin.Skin.Game.Name && t.Id != product.FromTableId
                                          select t).ToList();
                    List<ProductDM> localProducts = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.Skin));
                    skin.OtherSkins = new List<ProductDM>();
                    foreach (var i in skins)
                    {
                        ProductDM prod = (from s in localProducts
                                       where s.FromTableId == i.Id && s.Id != id
                                       select s).FirstOrDefault();
                        if (prod!=null)
                        {
                            prod.Comments = _mappers.ToCommentDM.Map<List<CommentDTO>, ICollection<CommentDM>>(_clientService.GetCommentsForProduct(prod.Id));
                            skin.OtherSkins.Add(prod);
                        }
                    }
                    return View(skin);
                }
            }
            return RedirectToAction("PageNotFound");
        }

        public ActionResult Clothes(int page = 1)
        {
            List<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.Cloth));
            foreach (var i in products)
            {
                i.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(i.Id));
            }
            ClothesVM result = new ClothesVM();
            result.MinPrice = Convert.ToInt32((from t in products
                                               orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                               select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First());
            result.MaxPrice = Convert.ToInt32((from t in products
                                               orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                               select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First()) + 1;
            result.Types = (from t in _mappers.ToClothDM.Map<ICollection<ClothDTO>, IEnumerable<ClothDM>>(_adminService.GetClothes())
                            select t.Type).Distinct().ToList();

            result.Colors = (from t in _adminService.GetColors()
                            select t.Name).ToList();

            int pageSize = 18;

            result.Clothes = products.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            result.PageInfo = pageInfo;
            return View(result);
        }

        public ActionResult ClothFilters(string searchbyname = "", string order = "", string[] types = null, string[] colors = null, string[] sizes = null, int page = 1, string minPrice = "", string maxPrice = "")
        {
            ClothesVM result = new ClothesVM();
            int pageSize = 18;
            PageInfo pageInfo;

            IEnumerable<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, ICollection<ProductDM>>(_adminService.GetProducts(Goods.Cloth));
            foreach (var i in products)
            {
                i.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(i.Id));
            }
            if (searchbyname != "")
            {
                products = from t in products
                           where t.Name.Contains(searchbyname)
                           select t;
            }

            if (minPrice != "" && maxPrice != "")
            {
                int min = int.Parse(minPrice);
                int max = int.Parse(maxPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 >= min && t.Price - (t.Price * t.Sale) / 100 <= max) : (t.Price >= min && t.Price <= max)
                           select t;
            }
            else if (minPrice != "")
            {
                int min = int.Parse(minPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 >= min) : (t.Price >= min)
                           select t;
            }
            else if (maxPrice != "")
            {
                int max = int.Parse(maxPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 <= max) : (t.Price <= max)
                           select t;
            }
            List<ClothDM> clothes = _mappers.ToClothDM.Map<ICollection<ClothDTO>, List<ClothDM>>(_adminService.ProductsIntoClothes(_mappers.ToProductDTO.Map<IEnumerable<ProductDM>, List<ProductDTO>>(products)));

            if (colors != null)
            {
                List<ClothDM> _cloth = new List<ClothDM>();
                foreach (var i in colors)
                {
                    List<ClothDM> localCloth;
                    localCloth = (from t in clothes
                                 where t.Colors.Where(c => c.Name == i).Count() != 0
                                 select t).ToList();
                    _cloth.AddRange(localCloth);
                }
                clothes = _cloth;
            }

            if (sizes != null)
            {
                List<ClothDM> _cloth = new List<ClothDM>();
                foreach (var i in sizes)
                {
                    List<ClothDM> localCloth;
                    localCloth = (from t in clothes
                                  where t.Sizes.Where(s => s.Data == i).Count() != 0
                                  select t).ToList();
                    _cloth.AddRange(localCloth);
                }
                clothes = _cloth;
            }

            if (types != null)
            {
                List<ClothDM> _cloth = new List<ClothDM>();
                foreach (var i in types)
                {
                    List<ClothDM> localCloth;
                    localCloth = (from t in clothes
                                  where t.Type == i
                                  select t).ToList();
                    _cloth.AddRange(localCloth);
                }
                clothes = _cloth;
            }

            List<ProductDM> localProducts = new List<ProductDM>();
            foreach (var i in products)
            {
                ClothDM cloth = (from s in clothes
                               where s.Id == i.FromTableId
                               select s).FirstOrDefault();
                if (cloth != null)
                    localProducts.Add(i);
            }
            products = localProducts;

            if (order != "")
            {
                switch (order)
                {
                    case "По алфавиту А-Я":
                        products = (from t in products
                                    orderby t.Name ascending
                                    select t).ToList();
                        break;
                    case "По алфавиту Я-А":
                        products = (from t in products
                                    orderby t.Name descending
                                    select t).ToList();
                        break;
                    case "По увеличению стоимости":
                        products = (from t in products
                                    orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                    select t).ToList();
                        break;
                    case "По уменьшению стоимости":
                        products = (from t in products
                                    orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                    select t).ToList();
                        break;
                    case "По популярности":
                        products = (from t in products
                                    orderby t.CountSeen + t.CountFavorites descending
                                    select t).ToList();
                        break;
                    case "По рейтингу":
                        products = (from t in products
                                    orderby Convert.ToDouble(t.Comments.Sum(x => x.Assessment)) / Convert.ToDouble(t.Comments.Count) descending
                                    select t).ToList();
                        break;
                    case "По заказам":
                        products = (from t in products
                                    orderby t.CountOrders descending
                                    select t).ToList();
                        break;
                }
            }

            if (page > pageSize)
            {
                return RedirectToAction("PageNotFound");
            }
            result.Clothes = products.Skip((page - 1) * pageSize).Take(pageSize);
            pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            result.PageInfo = pageInfo;
            if (products != null && products.FirstOrDefault() != null)
            {
                result.MinPrice = Convert.ToInt32((from t in products
                                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                                   select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First());
                result.MaxPrice = Convert.ToInt32((from t in products
                                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                                   select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First()) + 1;
            }

            return PartialView(result);
        }

        public ActionResult ClothDetails(int? id)
        {
            if (id != null)
            {
                UpCountSeen(Convert.ToInt32(id));
                ProductDTO product = _adminService.GetProduct(Convert.ToInt32(id));
                ClothDTO result = _adminService.GetCloth(product.FromTableId);
                if (result != null)
                {
                    ClothDetailsVM cloth = new ClothDetailsVM();
                    cloth.IsSkinAlreadyInBasket = false;
                    cloth.IsSkinAlreadyInFavorites = false;
                    cloth.Cloth = _mappers.ToClothDM.Map<ClothDTO, ClothDM>(result);
                    UserDM client = _mappers.ToUserDM.Map<UserDTO, UserDM>(_accountService.GetUserByName(User.Identity.Name));
                    if (client?.Client != null)
                    {
                        foreach (var i in client.Client.Favorites.Products)
                        {
                            if (i.Id == id)
                            {
                                cloth.IsSkinAlreadyInFavorites = true;
                            }
                        }

                        foreach (var i in client.Client.Basket.Products)
                        {
                            if (i.Id == id)
                            {
                                cloth.IsSkinAlreadyInBasket = true;
                                cloth.IsSkinAlreadyInFavorites = true;
                            }
                        }
                    }
                    cloth.Product = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(product);
                    cloth.Product.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(product.Id));
                    List<ClothDM> clothes = (from t in _mappers.ToClothDM.Map<ICollection<ClothDTO>, IEnumerable<ClothDM>>(_adminService.GetClothes())
                                          where t.ForMen == cloth.Cloth.ForMen && t.Id != product.FromTableId
                                          select t).ToList();
                    List<ProductDM> localProducts = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.Cloth));
                    cloth.OtherClothes = new List<ProductDM>();
                    foreach (var i in clothes)
                    {
                        ProductDM prod = (from s in localProducts
                                          where s.FromTableId == i.Id && s.Id != id
                                          select s).FirstOrDefault();
                        if (prod != null)
                        {
                            prod.Comments = _mappers.ToCommentDM.Map<List<CommentDTO>, ICollection<CommentDM>>(_clientService.GetCommentsForProduct(prod.Id));
                            cloth.OtherClothes.Add(prod);
                        }
                    }
                    return View(cloth);
                }
            }
            return RedirectToAction("PageNotFound");
        }

        public ActionResult ComputerComponents(int page = 1)
        {
            List<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.ComputerComponent));

            foreach (var i in products)
            {
                i.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(i.Id));
            }
            ComputerComponentsVM result = new ComputerComponentsVM();
            result.MinPrice = Convert.ToInt32((from t in products
                                               orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                               select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First());
            result.MaxPrice = Convert.ToInt32((from t in products
                                               orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                               select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First()) + 1;

            int pageSize = 18;

            result.ComputerComponents = products.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            result.PageInfo = pageInfo;
            return View(result);
        }

        public ActionResult ComputerComponentFilters(string searchbyname = "", string order = "", string[] types = null, int page = 1, string minPrice = "", string maxPrice = "")
        {
            ComputerComponentsVM result = new ComputerComponentsVM();
            int pageSize = 18;
            PageInfo pageInfo;

            IEnumerable<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, ICollection<ProductDM>>(_adminService.GetProducts(Goods.ComputerComponent));
            foreach (var i in products)
            {
                i.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(i.Id));
            }
            if (searchbyname != "")
            {
                products = from t in products
                           where t.Name.Contains(searchbyname)
                           select t;
            }

            if (minPrice != "" && maxPrice != "")
            {
                int min = int.Parse(minPrice);
                int max = int.Parse(maxPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 >= min && t.Price - (t.Price * t.Sale) / 100 <= max) : (t.Price >= min && t.Price <= max)
                           select t;
            }
            else if (minPrice != "")
            {
                int min = int.Parse(minPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 >= min) : (t.Price >= min)
                           select t;
            }
            else if (maxPrice != "")
            {
                int max = int.Parse(maxPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 <= max) : (t.Price <= max)
                           select t;
            }
            List<ComputerComponentDM> compComponents = _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, List<ComputerComponentDM>>(_adminService.ProductsIntoComputerComponents(_mappers.ToProductDTO.Map<IEnumerable<ProductDM>, List<ProductDTO>>(products)));

            if (types != null)
            {
                List<ComputerComponentDM> comps = new List<ComputerComponentDM>();
                foreach(var type in types)
                {
                    List<ComputerComponentDM> localComps = new List<ComputerComponentDM>();
                    switch (type)
                    {
                        case "Видеокарты":
                            localComps = (from comp in compComponents
                                              where comp.Type == ComponentType.VideoCard
                                              select comp).ToList();
                            break;
                        case "Материнские платы":
                            localComps = (from comp in compComponents
                                              where comp.Type == ComponentType.MotherBoard
                                              select comp).ToList();
                            break;
                        case "Блоки питания":
                            localComps = (from comp in compComponents
                                              where comp.Type == ComponentType.PowerSupply
                                              select comp).ToList();
                            break;
                        case "Процессоры":
                            localComps = (from comp in compComponents
                                              where comp.Type == ComponentType.Processor
                                              select comp).ToList();
                            break;
                        case "Оперативная память":
                            localComps = (from comp in compComponents
                                              where comp.Type == ComponentType.RAM
                                              select comp).ToList();
                            break;
                        case "Постоянная память":
                            localComps = (from comp in compComponents
                                              where comp.Type == ComponentType.ROM
                                              select comp).ToList();
                            break;
                        case "Сиситемные блоки":
                            localComps = (from comp in compComponents
                                              where comp.Type == ComponentType.SystemBlock
                                              select comp).ToList();
                            break;
                    }
                    comps.AddRange(localComps);
                }
                compComponents = comps;
            }

            List<ProductDM> localProducts = new List<ProductDM>();
            foreach (var i in products)
            {
                ComputerComponentDM comp = (from s in compComponents
                                 where s.Id == i.FromTableId
                                 select s).FirstOrDefault();
                if (comp != null)
                    localProducts.Add(i);
            }
            products = localProducts;

            if (order != "")
            {
                switch (order)
                {
                    case "По алфавиту А-Я":
                        products = (from t in products
                                    orderby t.Name ascending
                                    select t).ToList();
                        break;
                    case "По алфавиту Я-А":
                        products = (from t in products
                                    orderby t.Name descending
                                    select t).ToList();
                        break;
                    case "По увеличению стоимости":
                        products = (from t in products
                                    orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                    select t).ToList();
                        break;
                    case "По уменьшению стоимости":
                        products = (from t in products
                                    orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                    select t).ToList();
                        break;
                    case "По популярности":
                        products = (from t in products
                                    orderby t.CountSeen + t.CountFavorites descending
                                    select t).ToList();
                        break;
                    case "По рейтингу":
                        products = (from t in products
                                    orderby Convert.ToDouble(t.Comments.Sum(x => x.Assessment)) / Convert.ToDouble(t.Comments.Count) descending
                                    select t).ToList();
                        break;
                    case "По заказам":
                        products = (from t in products
                                    orderby t.CountOrders descending
                                    select t).ToList();
                        break;
                }
            }

            if (page > pageSize)
            {
                return RedirectToAction("PageNotFound");
            }
            result.ComputerComponents = products.Skip((page - 1) * pageSize).Take(pageSize);
            pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            result.PageInfo = pageInfo;
            if (products != null && products.FirstOrDefault() != null)
            {
                result.MinPrice = Convert.ToInt32((from t in products
                                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                                   select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First());
                result.MaxPrice = Convert.ToInt32((from t in products
                                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                                   select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First()) + 1;
            }

            return PartialView(result);
        }

        public ActionResult ComputerComponentDetails(int? id)
        {
            if (id != null)
            {
                UpCountSeen(Convert.ToInt32(id));
                ProductDTO product = _adminService.GetProduct(Convert.ToInt32(id));
                ComputerComponentDTO result = _adminService.GetComputerComponent(product.FromTableId);
                if (result != null)
                {
                    CompComponentDetailsVM comp = new CompComponentDetailsVM();
                    comp.IsCompCompAlreadyInBasket = false;
                    comp.IsCompCompAlreadyInFavorites = false;
                    comp.ComputerComponent = _mappers.ToComputerComponentDM.Map<ComputerComponentDTO, ComputerComponentDM>(result);
                    UserDM client = _mappers.ToUserDM.Map<UserDTO, UserDM>(_accountService.GetUserByName(User.Identity.Name));
                    if (client?.Client != null)
                    {
                        foreach (var i in client.Client.Favorites.Products)
                        {
                            if (i.Id == id)
                            {
                                comp.IsCompCompAlreadyInFavorites = true;
                            }
                        }

                        foreach (var i in client.Client.Basket.Products)
                        {
                            if (i.Id == id)
                            {
                                comp.IsCompCompAlreadyInBasket = true;
                                comp.IsCompCompAlreadyInFavorites = true;
                            }
                        }
                    }
                    comp.Product = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(product);
                    comp.Product.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(product.Id));
                    List<ComputerComponentDM> comps = (from t in _mappers.ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, IEnumerable<ComputerComponentDM>>(_adminService.GetCompComponents())
                                             where t.Type == comp.ComputerComponent.Type && t.Id != product.FromTableId
                                             select t).ToList();
                    List<ProductDM> localProducts = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.ComputerComponent));
                    comp.OtherComputerComponents = new List<ProductDM>();
                    foreach (var i in comps)
                    {
                        ProductDM prod = (from s in localProducts
                                          where s.FromTableId == i.Id && s.Id != id
                                          select s).FirstOrDefault();
                        if (prod != null)
                        {
                            prod.Comments = _mappers.ToCommentDM.Map<List<CommentDTO>, ICollection<CommentDM>>(_clientService.GetCommentsForProduct(prod.Id));
                            comp.OtherComputerComponents.Add(prod);
                        }
                    }
                    return View(comp);
                }
            }
            return RedirectToAction("PageNotFound");
        }

        public ActionResult Computers(int page = 1)
        {
            List<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.Computer));
            foreach (var i in products)
            {
                i.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(i.Id));
            }
            ComputersVM result = new ComputersVM();
            result.MinPrice = Convert.ToInt32((from t in products
                                               orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                               select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First());
            result.MaxPrice = Convert.ToInt32((from t in products
                                               orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                               select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First()) + 1;
            result.MotherBoards = (from t in _adminService.GetComputerComponentsByType(ComponentType.MotherBoard)
                            select t.Name).Distinct().ToList();
            result.PowerSupplies = (from t in _adminService.GetComputerComponentsByType(ComponentType.PowerSupply)
                                   select t.Name).Distinct().ToList();
            result.Processors = (from t in _adminService.GetComputerComponentsByType(ComponentType.Processor)
                                   select t.Name).Distinct().ToList();
            result.RAMs = (from t in _adminService.GetComputerComponentsByType(ComponentType.RAM)
                                   select t.Name).Distinct().ToList();
            result.ROMs = (from t in _adminService.GetComputerComponentsByType(ComponentType.ROM)
                                   select t.Name).Distinct().ToList();
            result.SystemBlocks = (from t in _adminService.GetComputerComponentsByType(ComponentType.SystemBlock)
                                   select t.Name).Distinct().ToList();
            result.VideoCards = (from t in _adminService.GetComputerComponentsByType(ComponentType.VideoCard)
                                   select t.Name).Distinct().ToList();

            int pageSize = 18;

            result.Computers = products.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            result.PageInfo = pageInfo;
            return View(result);
        }

        public ActionResult ComputerFilters(string searchbyname = "", string order = "", string[] processors = null, string[] rams = null, 
            string[] roms = null, string[] videoCards = null, string[] motherBoards = null, string[] powerSupplies = null, 
            string[] systemBlocks = null, int page = 1, string minPrice = "", string maxPrice = "")
        {
            ComputersVM result = new ComputersVM();
            int pageSize = 18;
            PageInfo pageInfo;

            IEnumerable<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, ICollection<ProductDM>>(_adminService.GetProducts(Goods.Computer));
            foreach (var i in products)
            {
                i.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(i.Id));
            }
            if (searchbyname != "")
            {
                products = from t in products
                           where t.Name.Contains(searchbyname)
                           select t;
            }

            if (minPrice != "" && maxPrice != "")
            {
                int min = int.Parse(minPrice);
                int max = int.Parse(maxPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 >= min && t.Price - (t.Price * t.Sale) / 100 <= max) : (t.Price >= min && t.Price <= max)
                           select t;
            }
            else if (minPrice != "")
            {
                int min = int.Parse(minPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 >= min) : (t.Price >= min)
                           select t;
            }
            else if (maxPrice != "")
            {
                int max = int.Parse(maxPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 <= max) : (t.Price <= max)
                           select t;
            }
            List<ComputerDM> computers = _mappers.ToComputerDM.Map<ICollection<ComputerDTO>, List<ComputerDM>>(_adminService.ProductsIntoComputers(_mappers.ToProductDTO.Map<IEnumerable<ProductDM>, List<ProductDTO>>(products)));

            if (processors != null)
            {
                List<ComputerDM> comps = new List<ComputerDM>();
                foreach (var i in processors)
                {
                    List<ComputerDM> localComp;
                    localComp = (from t in computers
                                  where t.Processor.Name == i
                                  select t).ToList();
                    comps.AddRange(localComp);
                }
                computers = comps;
            }

            if (rams != null)
            {
                List<ComputerDM> comps = new List<ComputerDM>();
                foreach (var i in rams)
                {
                    List<ComputerDM> localComp;
                    localComp = (from t in computers
                                 where t.RAM.TakeWhile(r => r.Name == i).Count() != 0
                                 select t).ToList();
                    comps.AddRange(localComp);
                }
                computers = comps;
            }

            if (roms != null)
            {
                List<ComputerDM> comps = new List<ComputerDM>();
                foreach (var i in roms)
                {
                    List<ComputerDM> localComp;
                    localComp = (from t in computers
                                 where t.ROM.TakeWhile(r => r.Name == i).Count() != 0
                                 select t).ToList();
                    comps.AddRange(localComp);
                }
                computers = comps;
            }

            if (videoCards != null)
            {
                List<ComputerDM> comps = new List<ComputerDM>();
                foreach (var i in videoCards)
                {
                    List<ComputerDM> localComp;
                    localComp = (from t in computers
                                 where t.VideoCard.TakeWhile(r => r.Name == i).Count() != 0
                                 select t).ToList();
                    comps.AddRange(localComp);
                }
                computers = comps;
            }

            if (motherBoards != null)
            {
                List<ComputerDM> comps = new List<ComputerDM>();
                foreach (var i in motherBoards)
                {
                    List<ComputerDM> localComp;
                    localComp = (from t in computers
                                 where t. MotherBoard.Name == i
                                 select t).ToList();
                    comps.AddRange(localComp);
                }
                computers = comps;
            }

            if (powerSupplies != null)
            {
                List<ComputerDM> comps = new List<ComputerDM>();
                foreach (var i in powerSupplies)
                {
                    List<ComputerDM> localComp;
                    localComp = (from t in computers
                                 where t.PowerSupply.Name == i
                                 select t).ToList();
                    comps.AddRange(localComp);
                }
                computers = comps;
            }

            if (systemBlocks != null)
            {
                List<ComputerDM> comps = new List<ComputerDM>();
                foreach (var i in systemBlocks)
                {
                    List<ComputerDM> localComp;
                    localComp = (from t in computers
                                 where t.SystemBlock.Name == i
                                 select t).ToList();
                    comps.AddRange(localComp);
                }
                computers = comps;
            }

            List<ProductDM> localProducts = new List<ProductDM>();
            foreach (var i in products)
            {
                ComputerDM comp = (from s in computers
                                 where s.Id == i.FromTableId
                                 select s).FirstOrDefault();
                if (comp != null)
                    localProducts.Add(i);
            }
            products = localProducts;

            if (order != "")
            {
                switch (order)
                {
                    case "По алфавиту А-Я":
                        products = (from t in products
                                    orderby t.Name ascending
                                    select t).ToList();
                        break;
                    case "По алфавиту Я-А":
                        products = (from t in products
                                    orderby t.Name descending
                                    select t).ToList();
                        break;
                    case "По увеличению стоимости":
                        products = (from t in products
                                    orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                    select t).ToList();
                        break;
                    case "По уменьшению стоимости":
                        products = (from t in products
                                    orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                    select t).ToList();
                        break;
                    case "По популярности":
                        products = (from t in products
                                    orderby t.CountSeen + t.CountFavorites descending
                                    select t).ToList();
                        break;
                    case "По рейтингу":
                        products = (from t in products
                                    orderby Convert.ToDouble(t.Comments.Sum(x => x.Assessment)) / Convert.ToDouble(t.Comments.Count) descending
                                    select t).ToList();
                        break;
                    case "По заказам":
                        products = (from t in products
                                    orderby t.CountOrders descending
                                    select t).ToList();
                        break;
                }
            }

            if (page > pageSize)
            {
                return RedirectToAction("PageNotFound");
            }
            result.Computers = products.Skip((page - 1) * pageSize).Take(pageSize);
            pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            result.PageInfo = pageInfo;
            if (products != null && products.FirstOrDefault() != null)
            {
                result.MinPrice = Convert.ToInt32((from t in products
                                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                                   select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First());
                result.MaxPrice = Convert.ToInt32((from t in products
                                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                                   select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First()) + 1;
            }

            return PartialView(result);
        }

        public ActionResult ComputerDetails(int? id)
        {
            if (id != null)
            {
                UpCountSeen(Convert.ToInt32(id));
                ProductDTO product = _adminService.GetProduct(Convert.ToInt32(id));
                ComputerDTO result = _adminService.GetComputer(product.FromTableId);
                if (result != null)
                {
                    ComputerDetailsVM comp = new ComputerDetailsVM();
                    comp.IsCompAlreadyInBasket = false;
                    comp.IsCompAlreadyInFavorites = false;
                    comp.Computer = _mappers.ToComputerDM.Map<ComputerDTO, ComputerDM>(result);

                    comp.MotherBoard = (from c in _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.ComputerComponent))
                                         where c.FromTableId == comp.Computer.MotherBoard.Id
                                         select c).FirstOrDefault();
                    comp.PowerBlock = (from c in _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.ComputerComponent))
                                        where c.FromTableId == comp.Computer.PowerSupply.Id
                                        select c).FirstOrDefault();
                    comp.Processor = (from c in _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.ComputerComponent))
                                        where c.FromTableId == comp.Computer.Processor.Id
                                        select c).FirstOrDefault();
                    comp.SystemBlock = (from c in _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.ComputerComponent))
                                        where c.FromTableId == comp.Computer.SystemBlock.Id
                                        select c).FirstOrDefault();

                    comp.Rams = new List<ProductDM>();
                    foreach(var i in comp.Computer.RAM)
                    {
                        ProductDM prod = (from c in _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.ComputerComponent))
                                  where c.FromTableId == i.Id 
                                  select c).FirstOrDefault();
                        comp.Rams.Add(prod);
                    }
                    comp.Roms = new List<ProductDM>();
                    foreach (var i in comp.Computer.ROM)
                    {
                        ProductDM prod = (from c in _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.ComputerComponent))
                                          where c.FromTableId == i.Id
                                          select c).FirstOrDefault();
                        comp.Roms.Add(prod);
                    }
                    comp.VideoCards = new List<ProductDM>();
                    foreach (var i in comp.Computer.VideoCard)
                    {
                        ProductDM prod = (from c in _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.ComputerComponent))
                                          where c.FromTableId == i.Id
                                          select c).FirstOrDefault();
                        comp.VideoCards.Add(prod);
                    }
                    UserDM client = _mappers.ToUserDM.Map<UserDTO, UserDM>(_accountService.GetUserByName(User.Identity.Name));
                    if (client?.Client != null)
                    {
                        foreach (var i in client.Client.Favorites.Products)
                        {
                            if (i.Id == id)
                            {
                                comp.IsCompAlreadyInFavorites = true;
                            }
                        }

                        foreach (var i in client.Client.Basket.Products)
                        {
                            if (i.Id == id)
                            {
                                comp.IsCompAlreadyInBasket = true;
                                comp.IsCompAlreadyInFavorites = true;
                            }
                        }
                    }
                    comp.Product = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(product);
                    comp.Product.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(product.Id));
                    List<ComputerDM> comps = (from t in _mappers.ToComputerDM.Map<ICollection<ComputerDTO>, IEnumerable<ComputerDM>>(_adminService.GetComputers())
                                             where t.Id != product.FromTableId
                                             select t).ToList();
                    List<ProductDM> localProducts = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.Computer));
                    comp.OtherComputers = new List<ProductDM>();
                    foreach (var i in comps)
                    {
                        ProductDM prod = (from s in localProducts
                                          where s.FromTableId == i.Id && s.Id != id
                                          select s).FirstOrDefault();
                        if (prod != null)
                        {
                            prod.Comments = _mappers.ToCommentDM.Map<List<CommentDTO>, ICollection<CommentDM>>(_clientService.GetCommentsForProduct(prod.Id));
                            comp.OtherComputers.Add(prod);
                        }
                    }
                    return View(comp);
                }
            }
            return RedirectToAction("PageNotFound");
        }

        public ActionResult Containers(int page = 1)
        {
            List<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.Container));
            foreach (var i in products)
            {
                i.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(i.Id));
            }
            ContainersVM result = new ContainersVM();
            result.MinPrice = Convert.ToInt32((from t in products
                                               orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                               select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First());
            result.MaxPrice = Convert.ToInt32((from t in products
                                               orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                               select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First()) + 1;
            result.Types = (from t in _mappers.ToContainerDM.Map<ICollection<ContainerDTO>, IEnumerable<ContainerDM>>(_adminService.GetContainers())
                            select t.Type).Distinct().ToList();

            int pageSize = 18;

            result.Containers = products.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            result.PageInfo = pageInfo;
            return View(result);
        }

        public ActionResult ContainerFilters(string searchbyname = "", string order = "", string[] types = null, int page = 1, string minPrice = "", string maxPrice = "")
        {
            ContainersVM result = new ContainersVM();
            int pageSize = 18;
            PageInfo pageInfo;

            IEnumerable<ProductDM> products = _mappers.ToProductDM.Map<ICollection<ProductDTO>, ICollection<ProductDM>>(_adminService.GetProducts(Goods.Container));
            foreach (var i in products)
            {
                i.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(i.Id));
            }
            if (searchbyname != "")
            {
                products = from t in products
                           where t.Name.Contains(searchbyname)
                           select t;
            }

            if (minPrice != "" && maxPrice != "")
            {
                int min = int.Parse(minPrice);
                int max = int.Parse(maxPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 >= min && t.Price - (t.Price * t.Sale) / 100 <= max) : (t.Price >= min && t.Price <= max)
                           select t;
            }
            else if (minPrice != "")
            {
                int min = int.Parse(minPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 >= min) : (t.Price >= min)
                           select t;
            }
            else if (maxPrice != "")
            {
                int max = int.Parse(maxPrice);
                products = from t in products
                           where (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100 <= max) : (t.Price <= max)
                           select t;
            }
            List<ContainerDM> containers = _mappers.ToContainerDM.Map<ICollection<ContainerDTO>, List<ContainerDM>>(_adminService.ProductsIntoContainers(_mappers.ToProductDTO.Map<IEnumerable<ProductDM>, List<ProductDTO>>(products)));

            if (types != null)
            {
                List<ContainerDM> conts = new List<ContainerDM>();
                foreach (var i in types)
                {
                    List<ContainerDM> localConts;
                    localConts = (from t in conts
                                  where t.Type == i
                                  select t).ToList();
                    conts.AddRange(localConts);
                }
                containers = conts;
            }

            List<ProductDM> localProducts = new List<ProductDM>();
            foreach (var i in products)
            {
                ContainerDM cont = (from s in containers
                                 where s.Id == i.FromTableId
                                 select s).FirstOrDefault();
                if (cont != null)
                    localProducts.Add(i);
            }
            products = localProducts;

            if (order != "")
            {
                switch (order)
                {
                    case "По алфавиту А-Я":
                        products = (from t in products
                                    orderby t.Name ascending
                                    select t).ToList();
                        break;
                    case "По алфавиту Я-А":
                        products = (from t in products
                                    orderby t.Name descending
                                    select t).ToList();
                        break;
                    case "По увеличению стоимости":
                        products = (from t in products
                                    orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                    select t).ToList();
                        break;
                    case "По уменьшению стоимости":
                        products = (from t in products
                                    orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                    select t).ToList();
                        break;
                    case "По популярности":
                        products = (from t in products
                                    orderby t.CountSeen + t.CountFavorites descending
                                    select t).ToList();
                        break;
                    case "По рейтингу":
                        products = (from t in products
                                    orderby Convert.ToDouble(t.Comments.Sum(x => x.Assessment)) / Convert.ToDouble(t.Comments.Count) descending
                                    select t).ToList();
                        break;
                    case "По покупкам":
                        products = (from t in products
                                    orderby t.CountOrders descending
                                    select t).ToList();
                        break;
                }
            }

            if (page > pageSize)
            {
                return RedirectToAction("PageNotFound");
            }
            result.Containers = products.Skip((page - 1) * pageSize).Take(pageSize);
            pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.Count() };
            result.PageInfo = pageInfo;
            if (products != null && products.FirstOrDefault() != null)
            {
                result.MinPrice = Convert.ToInt32((from t in products
                                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) ascending
                                                   select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First());
                result.MaxPrice = Convert.ToInt32((from t in products
                                                   orderby (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price) descending
                                                   select (t.Sale != 0) ? (t.Price - (t.Price * t.Sale) / 100) : (t.Price)).First()) + 1;
            }

            return PartialView(result);
        }

        public ActionResult ContainerDetails(int? id)
        {
            if (id != null)
            {
                UpCountSeen(Convert.ToInt32(id));
                ProductDTO product = _adminService.GetProduct(Convert.ToInt32(id));
                ContainerDTO result = _adminService.GetContainer(product.FromTableId);
                if (result != null)
                {
                    ContainerDetailsVM cont = new ContainerDetailsVM();
                    cont.IsContainerAlreadyInBasket = false;
                    cont.IsContainerAlreadyInFavorites = false;
                    cont.Container = _mappers.ToContainerDM.Map<ContainerDTO, ContainerDM>(result);
                    UserDM client = _mappers.ToUserDM.Map<UserDTO, UserDM>(_accountService.GetUserByName(User.Identity.Name));
                    if (client?.Client != null)
                    {
                        foreach (var i in client.Client.Favorites.Products)
                        {
                            if (i.Id == id)
                            {
                                cont.IsContainerAlreadyInFavorites = true;
                            }
                        }

                        foreach (var i in client.Client.Basket.Products)
                        {
                            if (i.Id == id)
                            {
                                cont.IsContainerAlreadyInBasket = true;
                                cont.IsContainerAlreadyInFavorites = true;
                            }
                        }
                    }
                    cont.Product = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(product);
                    cont.Product.Comments = _mappers.ToCommentDM.Map<ICollection<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(product.Id));
                    List<ContainerDM> containers = (from t in _mappers.ToContainerDM.Map<ICollection<ContainerDTO>, IEnumerable<ContainerDM>>(_adminService.GetContainers())
                                             where t.Type == cont.Container.Type && t.Id != product.FromTableId
                                             select t).ToList();
                    List<ProductDM> localProducts = _mappers.ToProductDM.Map<ICollection<ProductDTO>, List<ProductDM>>(_adminService.GetProducts(Goods.Container));
                    cont.OtherContainers = new List<ProductDM>();
                    foreach (var i in containers)
                    {
                        ProductDM prod = (from s in localProducts
                                          where s.FromTableId == i.Id && s.Id != id
                                          select s).FirstOrDefault();
                        if (prod != null)
                        {
                            prod.Comments = _mappers.ToCommentDM.Map<List<CommentDTO>, ICollection<CommentDM>>(_clientService.GetCommentsForProduct(prod.Id));
                            cont.OtherContainers.Add(prod);
                        }
                    }
                    cont.Container.Products = cont.Container.Products.OrderBy(x => x.Sale!=0?(x.Price - (x.Price*x.Sale)/100):(x.Price)).ToList();
                    return View(cont);
                }
            }
            return RedirectToAction("PageNotFound");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

        public void UpCountSeen(int productId)
        {
            _clientService.CountUp(productId, Count.Seen);
        }
    }
}