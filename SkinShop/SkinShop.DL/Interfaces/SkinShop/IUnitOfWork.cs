using SkinShop.DL.Entities.SkinShop;
using SkinShop.DL.Interfaces.Identity;
using SkinShop.DL.Managers;
using SkinShop.DL.Repositories.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.DL.Interfaces.SkinShop
{
    public interface IUnitOfWorK
    {
        AbstractRepository<Game> Games { get; }

        AbstractRepository<Order> Orders { get; }

        AbstractRepository<Basket> Baskets { get; }

        AbstractRepository<Favorites> Favorites { get; }

        AbstractRepository<Skin> Skins { get; }

        AbstractRepository<SkinRarity> SkinRareties { get; }

        AbstractRepository<OrderCount> OrderCounts { get; }

        AbstractRepository<Image> Images { get; }

        AbstractRepository<Cloth> Clothes { get; }

        AbstractRepository<Color> Colors { get; }

        AbstractRepository<Computer> Computers { get; }

        AbstractRepository<ComputerComponent> ComputerComponents { get; }

        AbstractRepository<Container> Containers { get; }

        AbstractRepository<Product> Products { get; }

        AbstractRepository<Property> Properties { get; }

        AbstractRepository<Other> Others { get; }

        AbstractRepository<Comment> Comments { get; }

        AbstractRepository<StringData> StringDatas { get; }

        AbstractRepository<Delivery> Deliveries { get; }

        ApplicationUserManager UserManager { get; }

        IClientManager ClientManager { get; }

        ApplicationRoleManager RoleManager { get; }

        void Save();
    }
}
