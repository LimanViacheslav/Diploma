using SkinShop.BLL.Identity.IdentityDTO;
using SkinShop.BLL.SkinShop.Interfaces;
using SkinShop.DL.Interfaces.SkinShop;
using SkinShop.BLL.Identity.Infrastructure;
using SkinShop.BLL.SkinShop.Services;
using SkinShop.BLL.SkinShop.SkinShopDTO;
using SkinShop.Filters;
using SkinShop.Mappers;
using SkinShop.Models.Account;
using SkinShop.Models.SkinShop;
using SkinShop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkinShop.DL.Repositories.SkinShop;
using SkinShop.BLL.SkinShop.BusinessModels;

namespace SkinShop.Controllers
{
    [BanAttribute]
    [Authorize]
    public class ClientController : Controller
    {
        IClientService _clientService = new ClientService(new UnitOfWork("DefaultConnection"));
        IAdminService _adminService = new AdminService(new UnitOfWork("DefaultConnection"));
        IEmployeeService _employeeService = new EmployeeService(new UnitOfWork("DefaultConnection"));
        MappersForDM _mappers = new MappersForDM();

        public ClientController() { }

        public ActionResult Favorites()
        {
            string clientName = System.Web.HttpContext.Current.User.Identity.Name;
            FavoritesDM favorites = _mappers.ToFavoritesDM.Map<FavoritesDTO, FavoritesDM>(_clientService.GetFavorites(clientName));
            return View(favorites);
        }

        public ActionResult AddToBasket(int? skinid)
        {
            if (skinid == null)
                return RedirectToAction("PageNotFound");
            _clientService.AddToBasket(Convert.ToInt32(skinid), System.Web.HttpContext.Current.User.Identity.Name);
            UpCountOrders(Convert.ToInt32(skinid));
            return RedirectToAction("Basket");
        }

        public ActionResult AddToFavorite(int? skinid)
        {
            if (skinid == null)
                return RedirectToAction("PageNotFound");
            _clientService.AddToFavorites(Convert.ToInt32(skinid), System.Web.HttpContext.Current.User.Identity.Name);
            UpCountFavorites(Convert.ToInt32(skinid));
            return RedirectToAction("Favorites");
        }

        public ActionResult DeleteFromBasket(int? skinid)
        {
            if (skinid == null)
                return RedirectToAction("PageNotFound");
            _clientService.DeleteFromBasket(Convert.ToInt32(skinid), System.Web.HttpContext.Current.User.Identity.Name);
            return RedirectToAction("Basket");
        }

        public ActionResult DeleteFromFavorite(int? skinid)
        {
            if (skinid == null)
                return RedirectToAction("PageNotFound");
            _clientService.DeleteFromFavorites(Convert.ToInt32(skinid), System.Web.HttpContext.Current.User.Identity.Name);
            return RedirectToAction("Favorites");
        }

        [HttpGet]
        public ActionResult Orders()
        {
            OrdersVM reslt = new OrdersVM();
            IEnumerable<OrderDM> orders;
            if(User.IsInRole("manager"))
            {
                orders = _mappers.ToOrderDM.Map<IEnumerable<OrderDTO>, IEnumerable<OrderDM>>(_employeeService.GetOrders());
            }
            else
            {
                orders = _mappers.ToOrderDM.Map<IEnumerable<OrderDTO>, IEnumerable<OrderDM>>(_clientService.GetOrders(User.Identity.Name));
            }
            if (orders != null && orders.FirstOrDefault() != null)
            {
                reslt.Orders = orders.ToList();
                reslt.MinPrice = Convert.ToInt32((from t in orders
                                                  orderby t.Price ascending
                                                  select t.Price).First());
                reslt.MaxPrice = Convert.ToInt32((from t in orders
                                                  orderby t.Price descending
                                                  select t.Price).First()) + 1;
            }
            return View(reslt);
        }
        
        public ActionResult OrderFilters(OrderStatusDM[] status = null, string userName = "", string minPrice = "", string maxPrice = "", string order = "")
        {
            OrdersVM reslt = new OrdersVM();
            IEnumerable<OrderDM> orders;
            if (User.IsInRole("manager"))
            {
                orders = _mappers.ToOrderDM.Map<IEnumerable<OrderDTO>, IEnumerable<OrderDM>>(_employeeService.GetOrders());
            }
            else
            {
                orders = _mappers.ToOrderDM.Map<IEnumerable<OrderDTO>, IEnumerable<OrderDM>>(_clientService.GetOrders(User.Identity.Name));
            }
            if (userName != "")
            {
                orders = from t in orders
                         where t.Client.Name.Contains(userName)
                         select t;
            }

            if (status != null)
            {
                List<OrderDM> _orders = new List<OrderDM>();
                foreach (var i in status)
                {
                    List<OrderDM> localOrder;
                    localOrder = (from t in orders
                             where t.Status == i
                             select t).ToList();
                    _orders.AddRange(localOrder);
                }
                orders = _orders;
            }

            if (minPrice != "" && maxPrice != "")
            {
                int min = int.Parse(minPrice);
                int max = int.Parse(maxPrice);
                orders = from t in orders
                         where t.Price >= min && t.Price <= max
                         select t;
            }
            else if (minPrice != "")
            {
                int min = int.Parse(minPrice);
                orders = from t in orders
                         where t.Price >= min
                         select t;
            }
            else if (maxPrice != "")
            {
                int max = int.Parse(maxPrice);
                orders = from t in orders
                         where t.Price <= max
                         select t;
            }

            if (order != "")
            {
                switch (order)
                {
                    case "По алфавиту А-Я (клиент)":
                        orders = from t in orders
                                 orderby t.Client.Name ascending
                                 select t;
                        break;
                    case "По алфавиту Я-А (клиент)":
                        orders = from t in orders
                                 orderby t.Client.Name descending
                                 select t;
                        break;
                    case "По увеличению стоимости":
                        orders = from t in orders
                                 orderby t.Price ascending
                                 select t;
                        break;
                    case "По уменьшению стоимости":
                        orders = from t in orders
                                 orderby t.Price descending
                                 select t;
                        break;
                    case "По увеличению даты":
                        orders = from t in orders
                                 orderby t.OrderTime ascending
                                 select t;
                        break;
                    case "По убыванию даты":
                        orders = from t in orders
                                 orderby t.OrderTime descending
                                 select t;
                        break;
                }
            }

            reslt.Orders = orders.ToList();
            if (orders != null && orders.FirstOrDefault() != null)
            {
                reslt.MinPrice = Convert.ToInt32((from t in orders
                                                  orderby t.Price ascending
                                                  select t.Price).First());
                reslt.MaxPrice = Convert.ToInt32((from t in orders
                                                  orderby t.Price descending
                                                  select t.Price).First()) + 1;
            }
            return PartialView(reslt);
        }

        [HttpGet]
        public ActionResult Basket()
        {
            string client = System.Web.HttpContext.Current.User.Identity.Name;
            BasketVM result = new BasketVM();
            BasketDM basket = _mappers.ToBasketDM.Map<BasketDTO, BasketDM>(_clientService.GetBasket(client));
            result.Basket = basket;
            result.Clothes = new List<ClothDM>();
            foreach(var i in basket.Products)
            {
                if(i.Table == DL.Entities.SkinShop.Goods.Cloth)
                {
                    result.Clothes.Add(_mappers.ToClothDM.Map<ClothDTO, ClothDM>(_adminService.GetCloth(i.FromTableId)));
                }
            }
            return View(result);
        }

        [HttpPost]
        public ActionResult Basket(BasketDM basket, List<int> counts, List<string> colors = null, List<string> sizes = null)
        {
            List<int> productsId = new List<int>();
            string client = System.Web.HttpContext.Current.User.Identity.Name;
            BasketDM Basket = _mappers.ToBasketDM.Map<BasketDTO, BasketDM>(_clientService.GetBasket(client));
            bool flag = false;
            if (Basket.Id == basket.Id)
            {
                foreach (var i in Basket.Products)
                {
                    if (i.Table != DL.Entities.SkinShop.Goods.Skin)
                        flag = true;
                    productsId.Add(i.Id);
                }
            }
            int id = 0;
            OperationDetails result = _clientService.MakeOrder(productsId, counts, client, colors, sizes, out id);
            if(result.Succedeed)
            {
                foreach(var i in productsId)
                {
                    _clientService.DeleteFromBasket(i, client);
                }
                if (flag)
                    return RedirectToAction("Delivery", "Client", new { orderId = id });
                else
                    return RedirectToAction("Pay", "Client", new { orderId = id});
            }
            ViewBag.Result = result.Message;
            ViewBag.Status = result.Succedeed;
            return View();
        }

        [HttpGet]
        public ActionResult Delivery(int orderId)
        {
            if (orderId == 0)
                return RedirectToAction("PageNotFound", "Client");
            DeliveryVM result = new DeliveryVM();
            result.OrderId = orderId;
            return View(result);
        }

        [HttpPost]
        public ActionResult Delivery(DeliveryVM model)
        {
            if(ModelState.IsValid)
            {
                DeliveryDTO delivery = new DeliveryDTO();
                delivery.PayType = model.PayType;
                delivery.DeliveryType = model.DeliveryType;
                delivery.Wishes = model.Wishes;
                if(model.Post != null && model.PostDepartment != null)
                {
                    delivery.DeliveryAddress = model.Post + " | " + model.PostDepartment + " | " + model.DeliveryAddress;
                }
                else
                {
                    delivery.DeliveryAddress = model.DeliveryAddress;
                }
                _clientService.MakeDelivery(model.OrderId, delivery);
                if (model.PayType == "Карта")
                {
                    return RedirectToAction("Pay", "Client",  new { orderId = model.OrderId });
                }
                else
                    return RedirectToAction("Orders", "Client");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Pay(int orderId = 0)
        {
            if (orderId == 0)
            {
                PayVM pay = new PayVM();
                pay.IsReplenishing = true;
                return View(pay);
            }
            else
            {
                OrderDTO order = _clientService.GetOrder(orderId);
                PayVM pay = new PayVM();
                pay.IsReplenishing = false;
                pay.Sum = order.Price;
                pay.OrderId = orderId;
                return View(pay);
            }
        }

        [HttpPost]
        public ActionResult Pay(PayVM model, string payed)
        {
            if (payed == "payed")
            {
                OperationDetails result = _clientService.Pay(model.OrderId);
                if (result.Succedeed)
                    return RedirectToAction("SuccesfullyPayed", "Client");
                else
                    return RedirectToAction("PageNotFound", "Home");
            }
            else if(payed == "replenishing")
            {
                OperationDetails result = _clientService.Replenish(model.ReplenishSum, User.Identity.Name);
                if(result.Succedeed)
                    return RedirectToAction("SuccesfullyPayed", "Client", new { text = result.Message});
                else
                    return RedirectToAction("PageNotFound", "Home");
            }
            else
                return View();
        }

        public ActionResult SuccesfullyPayed(string text)
        {
            if (text != "")
                ViewBag.Text = text;
            return View();
        }

        public ActionResult DeleteOrder(int id)
        {
            if (id == 0)
                return RedirectToAction("PageNotFound", "Home");
            OperationDetails result = _clientService.DeleteOrder(id);
            if (result.Succedeed)
                return RedirectToAction("Orders", "Client");
            return RedirectToAction("PageNotFound", "Home");
        }

        public ActionResult ReestablishOrder(int id)
        {
            if (id == 0)
                return RedirectToAction("PageNotFound", "Home");
            OperationDetails result = _clientService.ReestablishOrder(id);
            if (result.Succedeed)
                return RedirectToAction("Orders", "Client");
            return RedirectToAction("PageNotFound", "Home");
        }

        public ActionResult FromFavoritesToBasket(int? skinId)
        {
            if (skinId == null)
                return RedirectToAction("PageNotFound", "Home");
            string userName = User.Identity.Name;
            _clientService.AddToBasket(Convert.ToInt32(skinId), userName);
            _clientService.DeleteFromFavorites(Convert.ToInt32(skinId), userName);
            UpCountOrders(Convert.ToInt32(skinId));
            return RedirectToAction("Basket");
        }
        
        public void UpCountFavorites(int id)
        {
            _clientService.CountUp(id, Count.Favorites);
        }

        public void UpCountOrders(int id)
        {
            _clientService.CountUp(id, Count.Orders);
        }

        public string BuyContainer(int skinid, int count)
        {
            if (skinid != 0 && count != 0)
            {
                 OperationDetails result = _clientService.BuyContainer(skinid, count, User.Identity.Name);
                 return result.Message;
            }
            return "Error";
        }

        public ActionResult WriteComment(string text, int assessment, int productId, string route, int parentId = 0)
        {
            OperationDetails result = _clientService.WriteComment(text, User.Identity.Name, assessment, productId, parentId);
            List<CommentDM> comments = _mappers.ToCommentDM.Map<List<CommentDTO>, List<CommentDM>>(_clientService.GetCommentsForProduct(productId)).OrderByDescending(x => x.CommentDate).ToList();
            return PartialView(comments);
        }

        public ActionResult Open(int id)
        {
            if (id == 0)
                return RedirectToAction("PageNotFound", "Home");
            ContainerDetailsVM model = new ContainerDetailsVM();
            model.Product = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(_adminService.GetProduct(id));
            model.Container = _mappers.ToContainerDM.Map<ContainerDTO, ContainerDM>(_adminService.GetContainer(model.Product.FromTableId));
            model.Container.Products = model.Container.Products.OrderBy(x => x.Sale != 0 ? (x.Price - (x.Price * x.Sale) / 100) : (x.Price)).ToList();
            return View(model);
        }

        public ActionResult OpenContainer(int id)
        {
            if(id == 0)
                return RedirectToAction("PageNotFound", "Home");
            UserDTO user = _clientService.GetUserDTO(User.Identity.Name);
            ContainerToCountOpensDTO cont = user.Client.Containers.Where(x => x.Container.Id == id).FirstOrDefault();
            ContainerOpensVM model = new ContainerOpensVM();
            if (cont.CountOpens > 0)
            {
                UpCountOrders(id);
                ContainerOpens result = _clientService.OpenContainer(id, User.Identity.Name);
                model.Container = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(result.Container);
                model.ContainerLeft = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(result.ContainerLeft);
                model.ContainerRight = _mappers.ToProductDM.Map<ProductDTO, ProductDM>(result.ContainerRight);
                return PartialView(model);
            }
            model.Message = "У вас закончились кейсы";
            return PartialView(model);
        }

        public ActionResult SaleProduct(int id, int count)
        {
            if (id == 0 || count == 0)
                return RedirectToAction("PageNotFound", "Home");
            OperationDetails result = _clientService.SaleProduct(id, count, User.Identity.Name);
            return PartialView(result);
        }
    }
}