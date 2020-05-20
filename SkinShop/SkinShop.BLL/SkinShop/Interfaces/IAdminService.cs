using SkinShop.BLL.Identity.IdentityDTO;
using SkinShop.BLL.Identity.Infrastructure;
using SkinShop.BLL.SkinShop.Services;
using SkinShop.BLL.SkinShop.SkinShopDTO;
using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.Interfaces
{
    public interface IAdminService
    {
        OperationDetails CreateSkin(ProductDTO product, SkinDTO item, string oldSkin);
        OperationDetails CreateGame(ProductDTO product, GameDTO item, string oldGame);
        OperationDetails CreateCloth(ProductDTO product, ClothDTO item, string oldCloth);
        OperationDetails CreateComputer(ProductDTO product, ComputerDTO item, string oldComputer);
        OperationDetails CreateComputerComponent(ProductDTO product, ComputerComponentDTO item, string oldComputerComponent);
        OperationDetails CreateContainer(ProductDTO product, ContainerDTO item, string oldContainer);
        OperationDetails CreateOther(ProductDTO product, OtherDTO item, string oldOther);
        OperationDetails CreateColor(ColorDTO item);
        OperationDetails CreateSkinRarity(SkinRarityDTO item, string oldSkinRarity);

        Task<OperationDetails> CreateEmployee(UserDTO userDTO);
        OperationDetails Unban(string id);
        OperationDetails Ban(string id);

        ICollection<UserDTO> GetUsers();

        SkinRarityDTO GetSkinRarity(int id);
        OtherDTO GetOther(int id);
        ContainerDTO GetContainer(int id);
        ComputerDTO GetComputer(int id);
        ComputerComponentDTO GetComputerComponent(int id);
        ClothDTO GetCloth(int id);
        GameDTO GetGame(int id);
        SkinDTO GetSkin(int id);
        ProductDTO GetProduct(int id);
        ColorDTO GetColor(int id);

        ICollection<GameDTO> GetGames();
        ICollection<SkinRarityDTO> GetSkinRarities();
        ICollection<SkinDTO> GetSkins();
        ICollection<ProductDTO> GetProducts(Goods good);
        ICollection<PropertyDTO> GetProperties(string propertyName);
        ICollection<ColorDTO> GetColors();
        ICollection<ClothDTO> GetClothes();
        ICollection<ComputerDTO> GetComputers();
        ICollection<ComputerComponentDTO> GetCompComponents();
        ICollection<ContainerDTO> GetContainers();

        OperationDetails SoftDelete(Tables tables, int id);
        ICollection<ComputerComponentDTO> GetComputerComponentsByType(ComponentType type);
        ICollection<SkinDTO> ProductsIntoSkins(List<ProductDTO> products);
        ICollection<GameDTO> ProductsIntoGames(List<ProductDTO> products);
        ICollection<ClothDTO> ProductsIntoClothes(List<ProductDTO> products);
        ICollection<ComputerComponentDTO> ProductsIntoComputerComponents(List<ProductDTO> products);
        ICollection<ComputerDTO> ProductsIntoComputers(List<ProductDTO> products);
        ICollection<ContainerDTO> ProductsIntoContainers(List<ProductDTO> products);
        ICollection<ProductDTO> GetAllProducts();
    }
}
