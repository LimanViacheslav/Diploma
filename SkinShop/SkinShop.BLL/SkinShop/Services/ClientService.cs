using SkinShop.BLL.Identity.IdentityDTO;
using SkinShop.BLL.Identity.Infrastructure;
using SkinShop.BLL.SkinShop.BusinessModels;
using SkinShop.BLL.SkinShop.Interfaces;
using SkinShop.BLL.SkinShop.Mappers;
using SkinShop.BLL.SkinShop.SkinShopDTO;
using SkinShop.DL.Entities.Identity;
using SkinShop.DL.Entities.SkinShop;
using SkinShop.DL.Interfaces.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.Services
{
    public enum Count
    {
        Favorites = 1,
        Orders,
        Seen
    }

    public class ClientService: IClientService
    {
        public Random random = new Random();
        IUnitOfWorK Database { get; set; }
        MappersForDTO _mappers = new MappersForDTO();

        public ClientService(IUnitOfWorK uow)
        {
            Database = uow;
        }

        public OperationDetails Pay(int id)
        {
            if (id == 0)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            Order order = Database.Orders.Get(id);
            order.IsPayed = true;
            Database.Orders.Update(order);
            Database.Save();
            return new OperationDetails(true, "Платёж был успешно совершён", this.ToString());
        }

        public OperationDetails MakeDelivery(int orderId, DeliveryDTO model)
        {
            if (orderId == 0 || model == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            Delivery delivery = _mappers.ToDelivery.Map<DeliveryDTO, Delivery>(model);
            Order order = Database.Orders.Get(orderId);
            if(order == null || delivery == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            order.Delivery = delivery;
            Database.Orders.Update(order);
            Database.Save();
            return new OperationDetails(true, "Доставка была успешно оформена", this.ToString());
        }

        public OperationDetails MakeOrder(List<int> productsId, List<int> counts, string clientName, List<string> colors, List<string> sizes, out int id)
        {
            Order order = new Order();
            double price = 0;
            id = 0;
            List<Product> products = new List<Product>();
            foreach (var i in productsId)
            {
                products.Add(Database.Products.Get(i));
            }
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Sale > 0)
                {
                    price += Discount.GetDiscountedPrice(products[i].Price, products[i].Sale) * counts[i];
                }
                else
                {
                    price += products[i].Price * counts[i];
                }
            }
            if (clientName != "")
            {
                User _client = GetUser(clientName);
                order.Client = _client;
                order.ClientId = _client.Id;
            }
            order.OrderTime = DateTime.Now;
            order.Price = price;
            order.Status = OrderStatus.NotConfirmed;
            if (products != null && counts != null)
            {
                List<OrderCount> list = new List<OrderCount>() { };
                for (int i = 0, j = 0; i < products.Count; i++)
                {
                    Product product = Database.Products.Get(products[i].Id);
                    if (product.Table == Goods.Cloth)
                    {
                        if (sizes != null && colors != null)
                        {
                            list.Add(new OrderCount()
                            {
                                Product = product,
                                Count = counts[i],
                                Color = Database.Colors.Find(x => x.Name == colors[j]).FirstOrDefault(),
                                Size = sizes[j]
                            });
                        }
                        else
                        {
                            return new OperationDetails(false, "Не указан размер и цвет вещи", this.ToString());
                        }
                    }
                    else
                    {
                        list.Add(new OrderCount() { Product = product, Count = counts[i] });
                    }
                }
                order.OrderCounts = list;
            }
            else
            {
                return new OperationDetails(false, "Не удалось найти товары для заказа", this.ToString());
            }
            Database.Orders.Add(order);
            Database.Save();
            id = Database.Orders.Find(o => o.Client.UserName == clientName && o.OrderTime == order.OrderTime).FirstOrDefault().Id;
            return new OperationDetails(true, "Заказ успешно оформлен", this.ToString());
        }

        public OperationDetails SetDelivery(DeliveryDTO delivery, int orderId)
        {
            if(delivery == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            Order order = Database.Orders.Get(orderId);
            if (order == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            order.Delivery = _mappers.ToDelivery.Map<DeliveryDTO, Delivery>(delivery);
            Database.Orders.Update(order);
            Database.Save();
            return new OperationDetails(true, "Доставка была успешно оформлена", this.ToString());
        }

        public OperationDetails AddToFavorites(int productId, string clientName = "")
        {
            if (clientName != "")
            {
                User client = GetUser(clientName);
                if (client == null || client.Client == null)
                    return new OperationDetails(false, "Не удалось найти клиента", this.ToString());
                IEnumerable<Favorites> result = Database.Favorites.Find(f => f.Id == client.Client.Favorites.Id);
                Favorites favorites;
                Product product = Database.Products.Get(productId);
                if (product == null)
                    return new OperationDetails(false, "Не удалось найти товар", this.ToString());
                if (result != null)
                {
                    favorites = result.FirstOrDefault();
                    favorites.Products.Add(product);
                    Database.Favorites.Update(favorites);
                }
                else
                {
                    favorites = new Favorites();
                    favorites.Products.Add(product);
                    Database.Favorites.Add(favorites);
                }
                Database.Save();
                return new OperationDetails(true, "Tовар успешно добавлен в избранное", this.ToString());
            }
            else
            {
                return new OperationDetails(false, "Не удалось найти клиента", this.ToString());
            }
        }

        public OperationDetails DeleteFromFavorites(int productId, string clientName = "")
        {
            if (clientName != "")
            {
                User client = GetUser(clientName);
                if (client == null || client.Client == null)
                    return new OperationDetails(false, "Не удалось найти клиента", this.ToString());
                IEnumerable<Favorites> result = Database.Favorites.Find(f => f.Id == client.Client.Favorites.Id);
                if (result != null)
                {
                    Product product = Database.Products.Get(productId);
                    if (product == null)
                        return new OperationDetails(false, "Не удалось найти товар", this.ToString());
                    result.FirstOrDefault().Products.Remove(product);
                    Database.Favorites.Update(result.FirstOrDefault());
                    Database.Save();
                    return new OperationDetails(true, "Tовар успешно удалён из избранного", this.ToString());
                }
                else
                {
                    return new OperationDetails(false, "Не удалось найти информацию об избранном пользователя", this.ToString());
                }
            }
            else
            {
                return new OperationDetails(false, "Не удалось найти клиента", this.ToString());
            }
        }

        public FavoritesDTO GetFavorites(string clientName)
        {
            User user = GetUser(clientName);
            if (user.Client?.Favorites == null)
                user.Client.Favorites = new Favorites();
            return _mappers.ToFavoritesDTO.Map<Favorites, FavoritesDTO>(Database.Favorites.Find(x => x.Id == user.Client?.FavoritesId).FirstOrDefault());
        }

        public BasketDTO GetBasket(string clientName)
        {
            User user = GetUser(clientName);
            if (user.Client?.Basket == null)
                user.Client.Basket = new Basket();
            return _mappers.ToBasketDTO.Map<Basket, BasketDTO>(Database.Baskets.Find(x => x.Id == user.Client?.BasketId).FirstOrDefault());
        }

        public IEnumerable<OrderDTO> GetOrders(string clientName)
        {
            User user = GetUser(clientName);
            return _mappers.ToOrderDTO.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(Database.Orders.Find(x => x.ClientId == user.Id && x.IsDeleted == false));
        }

        public OperationDetails DeleteOrder(int orderId)
        {
            Order order = Database.Orders.Find(x => x.Id == orderId).FirstOrDefault();
            if(order == null)
                return new OperationDetails(false, "Не удалось найти заказ", this.ToString());
            order.IsDeletedFromUser = true;
            Database.Orders.Update(order);
            Database.Save();
            return new OperationDetails(true, "Заказ был упешно удалён", this.ToString());
        }

        public OperationDetails ReestablishOrder(int orderId)
        {
            Order order = Database.Orders.Find(x => x.Id == orderId).FirstOrDefault();
            if (order == null)
                return new OperationDetails(false, "Не удалось найти заказ", this.ToString());
            order.IsDeletedFromUser = false;
            Database.Orders.Update(order);
            Database.Save();
            return new OperationDetails(true, "Заказ был упешно восстановлен", this.ToString());
        }

        public OperationDetails AddToBasket(int productId, string clientName = "")
        {
            if (clientName != "")
            {
                User client = GetUser(clientName);
                if (client == null || client.Client == null)
                    return new OperationDetails(false, "Не удалось найти клиента", this.ToString());
                IEnumerable<Basket> result = Database.Baskets.Find(b => b.Id == client.Client.Basket.Id);
                Basket basket;
                Product product = Database.Products.Get(productId);
                if (product == null)
                    return new OperationDetails(false, "Не удалось найти товар", this.ToString());
                if (result != null)
                {
                    basket = result.FirstOrDefault();
                    basket.Products.Add(product);
                    Database.Baskets.Update(basket);
                }
                else
                {
                    basket = new Basket();
                    basket.Products.Add(product);
                    Database.Baskets.Add(basket);
                }
                Database.Save();
                return new OperationDetails(true, "Скин успешно добавлен в корзину", this.ToString());
            }
            else
            {
                return new OperationDetails(false, "Не удалось найти клиента", this.ToString());
            }
        }

        public OperationDetails DeleteFromBasket(int productId, string clientName = "")
        {
            if (clientName != "")
            {
                User client = GetUser(clientName);
                if (client == null || client.Client == null)
                    return new OperationDetails(false, "Не удалось найти клиента", this.ToString());
                IEnumerable<Basket> result = Database.Baskets.Find(b => b.Id == client.Client.Basket.Id); 
                if (result != null)
                {
                    Product product = Database.Products.Get(productId);
                    if (product == null)
                        return new OperationDetails(false, "Не удалось найти товар", this.ToString());
                    result.FirstOrDefault().Products.Remove(product);
                    Database.Baskets.Update(result.FirstOrDefault());
                    Database.Save();
                    return new OperationDetails(true, "Скин успешно удалён в корзину", this.ToString());
                }
                else
                {
                    return new OperationDetails(false, "Не удалось найти информацию о корзине пользователя", this.ToString());
                }
            }
            else
            {
                return new OperationDetails(false, "Не удалось найти клиента", this.ToString());
            }
        }

        public OperationDetails CountUp (int productId, Count countField)
        {
            Product product = Database.Products.Get(productId);
            if(product == null)
                return new OperationDetails(false, "Не удалось найти товар", this.ToString());
            if (countField == Count.Favorites)
                product.CountFavorites++;
            else if (countField == Count.Orders)
                product.CountOrders++;
            else if (countField == Count.Seen)
                product.CountSeen++;
            else
                return new OperationDetails(false, "Не удалось определить действие", this.ToString());
            Database.Products.Update(product);
            Database.Save();
            return new OperationDetails(true, "Количество успешно увеличено", this.ToString());
        }

        public User GetUser(string userName)
        {
            User item = Database.ClientManager.FindUser(x => x.Email == userName);
            return item;
        }

        public OperationDetails Replenish(double sum, string user)
        {
            if (sum <= 0 || user == "")
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            User localUser = Database.ClientManager.FindUser(x => x.UserName == user);
            localUser.Client.Money += sum;
            Database.ClientManager.Update(localUser);
            Database.Save();
            return new OperationDetails(true, "Ваш баланс успешно пополнен на " + sum, this.ToString());
        }

        public ContainerOpens OpenContainer(int id, string userName)
        {
            User user = Database.ClientManager.FindUser(x => x.UserName == userName);
            Product container = Database.Products.Get(id);
            Container cont = Database.Containers.Get(container.FromTableId);
            if(user == null || cont == null)
                return null;
            int resultNumber = -1;
            int chanceForCommon = 100 - cont.ChanseForLegendary - cont.ChanseForRare;
            int chanceForRare = chanceForCommon + cont.ChanseForRare;
            ContainerOpens result = new ContainerOpens();
            switch(cont.TypeOfHard)
            {
                case "Easy":
                    resultNumber = random.Next(1, 100);
                    result.Container = GetRandomProductFromContainer(chanceForCommon, chanceForRare, resultNumber, cont);
                    resultNumber -= 10;
                    result.ContainerLeft = GetRandomProductFromContainer(chanceForCommon, chanceForRare, resultNumber, cont);
                    resultNumber += 20;
                    result.ContainerRight = GetRandomProductFromContainer(chanceForCommon, chanceForRare, resultNumber, cont);
                    break;
                case "Medium":
                    resultNumber = random.Next(1, 1000);
                    result.Container = GetRandomProductFromContainer(chanceForCommon*10, chanceForRare * 10, resultNumber, cont);
                    resultNumber -= 100;
                    result.ContainerLeft = GetRandomProductFromContainer(chanceForCommon * 10, chanceForRare * 10, resultNumber, cont);
                    resultNumber += 200;
                    result.ContainerRight = GetRandomProductFromContainer(chanceForCommon * 10, chanceForRare * 10, resultNumber, cont);
                    break;
                case "Hard":
                    resultNumber = random.Next(1, 10000);
                    result.Container = GetRandomProductFromContainer(chanceForCommon * 100, chanceForRare * 100, resultNumber, cont);
                    resultNumber -= 1000;
                    result.ContainerLeft = GetRandomProductFromContainer(chanceForCommon * 100, chanceForRare * 100, resultNumber, cont);
                    resultNumber += 2000;
                    result.ContainerRight = GetRandomProductFromContainer(chanceForCommon * 100, chanceForRare * 100, resultNumber, cont);
                    break;
                case "Very hard":
                    resultNumber = random.Next(1, 100000);
                    result.Container = GetRandomProductFromContainer(chanceForCommon * 1000, chanceForRare * 1000, resultNumber, cont);
                    resultNumber -= 10000;
                    result.ContainerLeft = GetRandomProductFromContainer(chanceForCommon * 1000, chanceForRare * 1000, resultNumber, cont);
                    resultNumber += 20000;
                    result.ContainerRight = GetRandomProductFromContainer(chanceForCommon * 1000, chanceForRare * 1000, resultNumber, cont);
                    break;
                case "Ultra hard":
                    resultNumber = random.Next(1, 1000000);
                    result.Container = GetRandomProductFromContainer(chanceForCommon * 10000, chanceForRare * 10000, resultNumber, cont);
                    resultNumber -= 100000;
                    result.ContainerLeft = GetRandomProductFromContainer(chanceForCommon * 10000, chanceForRare * 10000, resultNumber, cont);
                    resultNumber += 200000;
                    result.ContainerRight = GetRandomProductFromContainer(chanceForCommon * 10000, chanceForRare * 10000, resultNumber, cont);
                    break;
            }
            if (user.Client.Products == null)
                user.Client.Products = new List<ContainerToCountOpens>();
            bool flag = true;
            foreach(var i in user.Client.Products)
            {
                if (i.Container.Id == result.Container.Id)
                {
                    i.CountOpens++;
                    flag = false;
                }
            }
            if(flag)
            {
                Product prod = Database.Products.Get(result.Container.Id);
                user.Client.Products.Add(new ContainerToCountOpens() { Container = prod, CountOpens = 1});
            }
            if (user.Client.ContainerToCountOpens == null)
            {
                user.Client.ContainerToCountOpens = new List<ContainerToCountOpens>();
                user.Client.ContainerToCountOpens.Add(new ContainerToCountOpens() { Container = container, CountOpens = 1 });
            }
            else
            {
                flag = true;
                foreach(var i in user.Client.ContainerToCountOpens)
                {
                    if(i.Container.Id == container.Id)
                    {
                        i.CountOpens++;
                        flag = false;
                    }
                }
                if(flag)
                    user.Client.ContainerToCountOpens.Add(new ContainerToCountOpens() { Container = container, CountOpens = 1 });
                    
            }
            foreach(var i in user.Client.Containers)
            {
                if (i.Container.Id == container.Id)
                    i.CountOpens--;
            }
            Database.ClientManager.Update(user);
            Database.Save();
            return result;
        }

        public ProductDTO GetRandomProductFromContainer(int chanceForCommon, int chanceForRare, int resultNumber, Container cont)
        {
            ProductDTO result = new ProductDTO();
            if(resultNumber <= chanceForCommon)
                result = _mappers.ToProductDTO.Map<Product, ProductDTO>(GetRandomProduct(cont, 0, cont.MinRare));
            else if(resultNumber > chanceForCommon && resultNumber <= chanceForRare)
                result = _mappers.ToProductDTO.Map<Product, ProductDTO>(GetRandomProduct(cont, cont.MinRare, cont.MaxRare));
            else
                result = _mappers.ToProductDTO.Map<Product, ProductDTO>(GetRandomProduct(cont, cont.MaxRare));
            return result;
        }

        public Product GetRandomProduct(Container cont, double minPrice, double maxPrice = 0)
        {
            List<Product> prods = new List<Product>();
            if (maxPrice != 0)
                prods = cont.Products.Where(x => (x.Sale != 0 ? (x.Price - (x.Price * x.Sale) / 100) : (x.Price)) >= minPrice && (x.Sale != 0 ? (x.Price - (x.Price * x.Sale) / 100) : (x.Price)) <= maxPrice).ToList();
            else
                prods = cont.Products.Where(x => (x.Sale != 0 ? (x.Price - (x.Price * x.Sale) / 100) : (x.Price)) >= minPrice).ToList();
            int number = random.Next(0, prods.Count);
            return prods[number];
        }

        public OrderDTO GetOrder(int id)
        {
            return _mappers.ToOrderDTO.Map<Order, OrderDTO>(Database.Orders.Get(id));
        }

        public OperationDetails BuyContainer(int id, int count, string userName)
        {
            if (id == 0 || count == 0 || userName == "")
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            User user = Database.ClientManager.FindUser(x => x.UserName == userName);
            if(user != null)
            {
                Product container = Database.Products.Get(id);
                if(container != null)
                {
                    double price = (container.Sale != 0 ? (container.Price - (container.Price * container.Sale) / 100) : (container.Price))*count;
                    if (price > user.Client.Money)
                        return new OperationDetails(false, "Недостаточно денег", this.ToString());
                    if (user.Client.Containers?.FirstOrDefault() != null)
                    {
                        bool flag = true;
                        foreach (var i in user.Client.Containers)
                        {
                            if (i.Container.Id == container.Id)
                            {
                                i.CountOpens += count;
                                flag = false;
                            }
                        }
                        if (flag)
                        {
                            ContainerToCountOpens model = new ContainerToCountOpens();
                            model.Container = container;
                            model.CountOpens = count;
                            user.Client.Containers.Add(model);
                        }
                    }
                    else
                    {
                        ContainerToCountOpens model = new ContainerToCountOpens();
                        model.Container = container;
                        model.CountOpens = count;
                        user.Client.Containers.Add(model);
                    }
                    user.Client.Money -= price;
                    Database.ClientManager.Update(user);
                    Database.Save();
                    return new OperationDetails(true, "Покупка прошла успешно", this.ToString());
                }
            }
            return new OperationDetails(false, "Не удалось найти объект", this.ToString());
        }

        public UserDTO GetUserDTO(string name)
        {
            User user = Database.ClientManager.FindUser(x => x.UserName == name);
            return _mappers.ToUserDTO.Map<User, UserDTO>(user);
        }

        public List<CommentDTO> GetCommentsForProduct(int id)
        {
            List<Comment> comments = Database.Comments.Find(com => com.ProductId == id).ToList();
            return _mappers.ToCommentDTO.Map<ICollection<Comment>, List<CommentDTO>>(comments);
        }

        public OperationDetails WriteComment(string text, string userName, int assessment, int productId, int parentId)
        {
            if (productId == 0)
                return new OperationDetails(false, "Товар не найден", this.ToString());
            Comment comment = new Comment();
            comment.ProductId = productId;
            comment.Text = text;
            comment.Assessment = assessment;
            comment.CommentDate = DateTime.Now;
            User user = Database.ClientManager.FindUser(u => u.Email == userName);
            if (user == null)
                return new OperationDetails(false, "Не удалось найти пользователя с именем " + userName, this.ToString());
            comment.User = user;
            //if(parentId != 0)
            //{
            //    comment.ParentCommentId = parentId;
            //    Comment parent = Database.Comments.Get(parentId);
            //    if (parent.Answers == null)
            //        parent.Answers = new List<Comment>();
            //    parent.Answers.Add(comment);
            //}
            Database.Comments.Add(comment);
            Database.Save();
            return new OperationDetails(true, "Сообщение успешно добавлено", this.ToString());
        }

        public OperationDetails SaleProduct(int id, int count, string userName)
        {
            User user = Database.ClientManager.FindUser(x => x.UserName == userName);
            bool flag = false;
            ContainerToCountOpens delete = new ContainerToCountOpens();
            foreach(var i in user.Client.Products)
            {
                if(i.Container.Id == id)
                {
                    if(i.CountOpens > count)
                    {
                        flag = true;
                        i.CountOpens -= count;
                        user.Client.Money += (i.Container.Sale != 0 ? (i.Container.Price - (i.Container.Price* i.Container.Sale)/100) : (i.Container.Price)) * count;
                    }
                    else if(i.CountOpens == count)
                    {
                        flag = true;
                        delete = i;
                        user.Client.Money += (i.Container.Sale != 0 ? (i.Container.Price - (i.Container.Price * i.Container.Sale) / 100) : (i.Container.Price)) * count;
                    }
                    else if(i.CountOpens < count)
                    {
                        return new OperationDetails(false, "Количество продуктов меньше, чем " + count, this.ToString());
                    }
                }
            }
            if (delete != null)
                user.Client.Products.Remove(delete);
            if(!flag)
                return new OperationDetails(false, "У вас нет такого продукта", this.ToString());
            Database.ClientManager.Update(user);
            Database.Save();
            if(count == 1)
                return new OperationDetails(true, "Товар успешно продан", this.ToString());
            else
                return new OperationDetails(true, "Товары успешно продан", this.ToString());
        }
    }
}
