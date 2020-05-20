using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using SkinShop.DL.EF;
using SkinShop.DL.Entities.Identity;
using SkinShop.DL.Entities.SkinShop;
using SkinShop.DL.Interfaces.Identity;
using SkinShop.DL.Interfaces.SkinShop;
using SkinShop.DL.Managers;
using SkinShop.DL.Repositories.Identity;

namespace SkinShop.DL.Repositories.SkinShop
{
    public class UnitOfWork : IUnitOfWorK
    {
        Context _context;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private IClientManager clientManager;

        public UnitOfWork(string connectionString)
        {
            _context = new Context(connectionString);
            userManager = new ApplicationUserManager(new UserStore<User>(_context));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_context));
            clientManager = new ClientManager(_context);
        }

        private AbstractRepository<Game> _games;
        private AbstractRepository<Order> _orders;
        private AbstractRepository<Basket> _baskets;
        private AbstractRepository<Favorites> _favorites;
        private AbstractRepository<Skin> _skins;
        private AbstractRepository<SkinRarity> _skinRareties;
        private AbstractRepository<OrderCount> _orderCounts;
        private AbstractRepository<Image> _images;
        private AbstractRepository<Cloth> _clothes;
        private AbstractRepository<Color> _colors;
        private AbstractRepository<Computer> _computers;
        private AbstractRepository<ComputerComponent> _computerComponents;
        private AbstractRepository<Container> _containers;
        private AbstractRepository<Product> _products;
        private AbstractRepository<Property> _properties;
        private AbstractRepository<Other> _others;
        private AbstractRepository<Comment> _comments;
        private AbstractRepository<Delivery> _deliveries;
        private AbstractRepository<StringData> _stringDatas;

        public virtual AbstractRepository<StringData> StringDatas
        {
            get
            {
                if (_stringDatas == null)
                    _stringDatas = new Repository<StringData>(_context);
                return _stringDatas;
            }
        }

        public virtual AbstractRepository<Comment> Comments
        {
            get
            {
                if (_comments == null)
                    _comments = new Repository<Comment>(_context);
                return _comments;
            }
        }

        public virtual AbstractRepository<Delivery> Deliveries
        {
            get
            {
                if (_deliveries == null)
                    _deliveries = new Repository<Delivery>(_context);
                return _deliveries;
            }
        }

        public virtual AbstractRepository<Game> Games
        {
            get
            {
                if (_games == null)
                    _games = new Repository<Game>(_context);
                return _games;
            }
        }

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }

        public IClientManager ClientManager
        {
            get { return clientManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public virtual AbstractRepository<Order> Orders
        {
            get
            {
                if (_orders == null)
                    _orders = new Repository<Order>(_context);
                return _orders;
            }
        }

        public virtual AbstractRepository<Basket> Baskets
        {
            get
            {
                if (_baskets == null)
                    _baskets = new Repository<Basket>(_context);
                return _baskets;
            }
        }

        public virtual AbstractRepository<Favorites> Favorites
        {
            get
            {
                if (_favorites == null)
                    _favorites = new Repository<Favorites>(_context);
                return _favorites;
            }
        }

        public virtual AbstractRepository<Skin> Skins
        {
            get
            {
                if (_skins == null)
                    _skins = new Repository<Skin>(_context);
                return _skins;
            }
        }

        public virtual AbstractRepository<SkinRarity> SkinRareties
        {
            get
            {
                if (_skinRareties == null)
                    _skinRareties = new Repository<SkinRarity>(_context);
                return _skinRareties;
            }
        }

        public virtual AbstractRepository<OrderCount> OrderCounts
        {
            get
            {
                if (_orderCounts == null)
                    _orderCounts = new Repository<OrderCount>(_context);
                return _orderCounts;
            }
        }

        public virtual AbstractRepository<Image> Images
        {
            get
            {
                if (_images == null)
                    _images = new Repository<Image>(_context);
                return _images;
            }
        }

        public virtual AbstractRepository<Cloth> Clothes
        {
            get
            {
                if (_clothes == null)
                    _clothes = new Repository<Cloth>(_context);
                return _clothes;
            }
        }

        public virtual AbstractRepository<Color> Colors
        {
            get
            {
                if (_colors == null)
                    _colors = new Repository<Color>(_context);
                return _colors;
            }
        }

        public virtual AbstractRepository<Computer> Computers
        {
            get
            {
                if (_computers == null)
                    _computers = new Repository<Computer>(_context);
                return _computers;
            }
        }

        public virtual AbstractRepository<ComputerComponent> ComputerComponents
        {
            get
            {
                if (_computerComponents == null)
                    _computerComponents = new Repository<ComputerComponent>(_context);
                return _computerComponents;
            }
        }

        public virtual AbstractRepository<Container> Containers
        {
            get
            {
                if (_containers == null)
                    _containers = new Repository<Container>(_context);
                return _containers;
            }
        }

        public virtual AbstractRepository<Product> Products
        {
            get
            {
                if (_products == null)
                    _products = new Repository<Product>(_context);
                return _products;
            }
        }

        public virtual AbstractRepository<Property> Properties
        {
            get
            {
                if (_properties == null)
                    _properties = new Repository<Property>(_context);
                return _properties;
            }
        }

        public virtual AbstractRepository<Other> Others
        {
            get
            {
                if (_others == null)
                    _others = new Repository<Other>(_context);
                return _others;
            }
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}
