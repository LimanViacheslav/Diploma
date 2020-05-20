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
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.Services
{
    public enum Tables
    {
        Cloth = 1,
        Color,
        Comment,
        ComputerComponent,
        Computer,
        Container,
        Game,
        Order,
        Product,
        Property,
        Skin,
        SkinRarity,
        Other
    }

    public class AdminService : IAdminService
    {
        IUnitOfWorK Database { get; set; }
        MappersForDTO _mappers = new MappersForDTO();

        public AdminService(IUnitOfWorK uow)
        {
            Database = uow;
        }

        public Product CreateProduct(Product product)
        {
            List<Image> images = new List<Image>();
            foreach (var i in product.Images)
            {
                if (i != null)
                {
                    Image image = Database.Images.Find(x => x.Photo == i.Photo)?.FirstOrDefault();
                    if (image == null)
                        image = new Image() { Photo = i.Photo, Text = i.Text };
                    images.Add(image);
                }
            }
            product.Images = images;
            product.DateOfAdded = DateTime.Now;

            return product;
        }

        public Product UpdateProduct(ProductDTO product)
        {
            Product oldPoduct = Database.Products.Get(product.Id);
            if (oldPoduct != null)
            {
                List<Image> images = new List<Image>();
                foreach (var i in product.Images)
                {
                    Image localImage = Database.Images.Find(x => x.Photo == i?.Photo).FirstOrDefault();
                    if (i != null)
                    {
                        if (localImage != null)
                        {
                            images.Add(localImage);
                        }
                        else
                        {
                            images.Add(new Image() { Photo = i.Photo, Text = i.Text });
                        }
                    }
                }
                oldPoduct.Images = images;
                oldPoduct.Name = product.Name;
                oldPoduct.Price = product.Price;
                oldPoduct.Sale = product.Sale;
                oldPoduct.Description = product.Description;
            }
            return oldPoduct;
        }

        public OperationDetails CreateGame(ProductDTO product, GameDTO item, string oldGame)
        {
            Product productInDB = Database.Products.Find(p => p.Name == oldGame).FirstOrDefault();
            if (productInDB != null)
                return UpdateGame(product, item, oldGame);

            if (item == null || product == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            item.Name = product.Name;
            Database.Games.Add(_mappers.ToGame.Map<GameDTO, Game>(item));
            Database.Save();

            Product localProduct = _mappers.ToProduct.Map<ProductDTO, Product>(product);
            if (localProduct == null)
                return new OperationDetails(false, "Не удалось преобразовать объект", this.ToString());
            localProduct = CreateProduct(localProduct);
            localProduct.Table = Goods.Game;
            Game game = Database.Games.Find(x => x.Name == localProduct.Name).FirstOrDefault();
            if (game == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            localProduct.FromTableId = game.Id;
            Database.Products.Add(localProduct);
            Database.Save();
            return new OperationDetails(true, "Игра была успешно добавлена", this.ToString());
        }

        public OperationDetails UpdateGame(ProductDTO product, GameDTO item, string oldGameName)
        {
            if (product == null || item == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Product localProduct = UpdateProduct(product);
            Database.Products.Update(localProduct);

            Game oldGame = Database.Games.Find(x => x.Name == oldGameName).FirstOrDefault();
            if (oldGame == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());

            oldGame.IsThingGame = item.IsThingGame;
            oldGame.Name = item.Name;
            oldGame.Properties = _mappers.ToProperty.Map<IEnumerable<PropertyDTO>, ICollection<Property>>(item.Properties);
            Database.Games.Update(oldGame);
            Database.Save();
            return new OperationDetails(true, "Игра успешно изменeнa", this.ToString());
        }

        public OperationDetails CreateSkin(ProductDTO product, SkinDTO item, string oldSkin)
        {
            Product productInDB = Database.Products.Find(p => p.Name == oldSkin).FirstOrDefault();
            if (productInDB != null)
                return UpdateSkin(product, item, oldSkin);

            if (item == null || product == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());

            Skin skin = _mappers.ToSkin.Map<SkinDTO, Skin>(item);
            if (skin == null)
                return new OperationDetails(false, "Не удалось преобразовать объект", this.ToString());
            if (skin.SkinRarity != null)
            {
                SkinRarity rarity = Database.SkinRareties.Find(r => r.RarityName == skin.SkinRarity.RarityName).FirstOrDefault();
                if (rarity == null)
                    return new OperationDetails(false, "Не удалось найти раритетность с таким названием", this.ToString());
                skin.SkinRarity = rarity;
            }
            if (skin.Game != null)
            {
                Game game = Database.Games.Find(g => g.Name == skin.Game.Name).FirstOrDefault();
                if (game == null)
                    return new OperationDetails(false, "Не удалось найти игру с таким названием", this.ToString());
                skin.Game = game;
            }
            skin.Name = product.Name;
            Database.Skins.Add(skin);
            Database.Save();

            Product localProduct = _mappers.ToProduct.Map<ProductDTO, Product>(product);
            if (localProduct == null)
                return new OperationDetails(false, "Не удалось преобразовать объект", this.ToString());
            localProduct = CreateProduct(localProduct);
            localProduct.Table = Goods.Skin;
            Skin skinInDB = Database.Skins.Find(x => x.Name == localProduct.Name).FirstOrDefault();
            if (skinInDB == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            localProduct.FromTableId = skinInDB.Id;
            Database.Products.Add(localProduct);
            Database.Save();
            return new OperationDetails(true, "Скин был успешно добавлен", this.ToString());
        }

        public OperationDetails UpdateSkin(ProductDTO product, SkinDTO item, string oldSkinName)
        {
            if (product == null || item == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Product localProduct = UpdateProduct(product);
            Database.Products.Update(localProduct);

            Skin oldSkin = Database.Skins.Find(s => s.Name == oldSkinName).FirstOrDefault();
            if (oldSkin == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            if (oldSkin.Game.Name != item.Game.Name)
            {
                Game game = Database.Games.Find(x => x.Name == item.Game.Name)?.FirstOrDefault();
                if (game == null)
                    return new OperationDetails(false, "Не удалось найти игру с таким названием", this.ToString());
                oldSkin.Game = game;
            }
            if (oldSkin.SkinRarity.RarityName != item.SkinRarity.RarityName)
            {
                SkinRarity skinRarity = Database.SkinRareties.Find(x => x.RarityName == item.SkinRarity.RarityName)?.FirstOrDefault();
                if (skinRarity == null)
                    return new OperationDetails(false, "Не удалось найти раритетность с таким названием", this.ToString());
                oldSkin.SkinRarity = skinRarity;
            }
            oldSkin.Properties = _mappers.ToProperty.Map<IEnumerable<PropertyDTO>, ICollection<Property>>(item.Properties);
            Database.Skins.Update(oldSkin);
            Database.Save();
            return new OperationDetails(true, "Скин успешно изменён", this.ToString());
        }

        public OperationDetails CreateCloth(ProductDTO product, ClothDTO item, string oldCloth)
        {
            Product productInDB = Database.Products.Find(p => p.Name == oldCloth).FirstOrDefault();
            if (productInDB != null)
                return UpdateCloth(product, item, oldCloth);

            if (item == null || product == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Cloth cloth = _mappers.ToCloth.Map<ClothDTO, Cloth>(item);
            List<Color> colors = new List<Color>();
            foreach(var col in item.Colors)
            {
                Color color = Database.Colors.Find(c => c.Name == col.Name).FirstOrDefault(); 
                if(color != null)
                    colors.Add(color);
            }
            cloth.Colors = colors;
            List<StringData> sizes = new List<StringData>();
            foreach (var siz in item.Sizes)
            {
                StringData size = Database.StringDatas.Find(c => c.Data == siz.Data).FirstOrDefault();
                if (size == null)
                    sizes.Add(new StringData() { Data = siz.Data });
                else
                    sizes.Add(size);
            }
            cloth.Sizes = sizes;
            cloth.Name = product.Name;
            Database.Clothes.Add(cloth);
            Database.Save();

            Product localProduct = _mappers.ToProduct.Map<ProductDTO, Product>(product);
            if (localProduct == null)
                return new OperationDetails(false, "Не удалось преобразовать объект", this.ToString());
            localProduct = CreateProduct(localProduct);
            localProduct.Table = Goods.Cloth;
            cloth = Database.Clothes.Find(x => x.Name == localProduct.Name).FirstOrDefault();
            if(cloth == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            localProduct.FromTableId = cloth.Id;
            Database.Products.Add(localProduct);
            Database.Save();
            return new OperationDetails(true, "Вещь была успешно добавлена", this.ToString());
        }

        public OperationDetails UpdateCloth(ProductDTO product, ClothDTO item, string oldClothName)
        {
            if (product == null || item == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Product localProduct = UpdateProduct(product);
            Database.Products.Update(localProduct);

            Cloth oldCloth = Database.Clothes.Find(x => x.Name == oldClothName).FirstOrDefault();
            if (oldCloth == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            List<Color> colors = new List<Color>();
            foreach (var col in item.Colors)
            {
                Color color = Database.Colors.Find(c => c.Name == col.Name).FirstOrDefault();
                if (color != null)
                    colors.Add(color);
            }
            oldCloth.Colors = colors;
            List<StringData> sizes = new List<StringData>();
            foreach (var siz in item.Sizes)
            {
                StringData size = Database.StringDatas.Find(c => c.Data == siz.Data).FirstOrDefault();
                if (size == null)
                    sizes.Add(new StringData() { Data = siz.Data });
                else
                    sizes.Add(size);
            }
            oldCloth.Sizes = sizes;
            oldCloth.Composition = item.Composition;
            oldCloth.Type = item.Type;
            oldCloth.ForMen = item.ForMen;
            oldCloth.Properties = _mappers.ToProperty.Map<IEnumerable<PropertyDTO>, ICollection<Property>>(item.Properties);
            Database.Clothes.Update(oldCloth);
            Database.Save();
            return new OperationDetails(true, "Вещь успешно изменeнa", this.ToString());
        }

        public OperationDetails CreateComputerComponent(ProductDTO product, ComputerComponentDTO item, string oldComputerComponent)
        {
            Product productInDB = Database.Products.Find(p => p.Name == oldComputerComponent).FirstOrDefault();
            if (productInDB != null)
                return UpdateComputerComponent(product, item, oldComputerComponent);

            if (item == null || product == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            item.Name = product.Name;
            Database.ComputerComponents.Add(_mappers.ToComputerComponent.Map<ComputerComponentDTO, ComputerComponent>(item));
            Database.Save();

            Product localProduct = _mappers.ToProduct.Map<ProductDTO, Product>(product);
            if (localProduct == null)
                return new OperationDetails(false, "Не удалось преобразовать объект", this.ToString());
            localProduct = CreateProduct(localProduct);
            localProduct.Table = Goods.ComputerComponent;
            ComputerComponent computerComponent = Database.ComputerComponents.Find(x => x.Name == localProduct.Name).FirstOrDefault();
            if (computerComponent == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            localProduct.FromTableId = computerComponent.Id;
            Database.Products.Add(localProduct);
            Database.Save();
            return new OperationDetails(true, "Компьютерный компонент был успешно добавлен", this.ToString());
        }

        public OperationDetails UpdateComputerComponent(ProductDTO product, ComputerComponentDTO item, string oldComputerComponentName)
        {
            if (product == null || item == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Product localProduct = UpdateProduct(product);
            Database.Products.Update(localProduct);

            ComputerComponent oldComputerComponent = Database.ComputerComponents.Find(x => x.Name == oldComputerComponentName).FirstOrDefault();
            if (oldComputerComponent == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());

            oldComputerComponent.Type = item.Type;
            oldComputerComponent.Properties = _mappers.ToProperty.Map<IEnumerable<PropertyDTO>, ICollection<Property>>(item.Properties);
            Database.ComputerComponents.Update(oldComputerComponent);
            Database.Save();
            return new OperationDetails(true, "Компьютерный компонент успешно изменён", this.ToString());
        }

        public OperationDetails CreateComputer(ProductDTO product, ComputerDTO item, string oldComputer)
        {
            Product productInDB = Database.Products.Find(p => p.Name == oldComputer).FirstOrDefault();
            if (productInDB != null)
                return UpdateComputer(product, item, oldComputer);

            if (item == null || product == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Computer computer = _mappers.ToComputer.Map<ComputerDTO, Computer>(item);
            computer.Name = product.Name;
            if(item.MotherBoard != null)
            {
                ComputerComponent motherBoard = Database.ComputerComponents.Find(c => c.Name == item.MotherBoard.Name).FirstOrDefault();
                if (motherBoard == null)
                    return new OperationDetails(false, "Не удалось найти материнскую плату", this.ToString());
                computer.MotherBoard = motherBoard;
            }
            if (item.PowerSupply != null)
            {
                ComputerComponent powerSypply = Database.ComputerComponents.Find(c => c.Name == item.PowerSupply.Name).FirstOrDefault();
                if (powerSypply == null)
                    return new OperationDetails(false, "Не удалось найти блок питания", this.ToString());
                computer.PowerSupply = powerSypply;
            }
            if (item.Processor != null)
            {
                ComputerComponent processor = Database.ComputerComponents.Find(c => c.Name == item.Processor.Name).FirstOrDefault();
                if (processor == null)
                    return new OperationDetails(false, "Не удалось найти процессор", this.ToString());
                computer.Processor = processor;
            }
            if (item.SystemBlock != null)
            {
                ComputerComponent systemBlock = Database.ComputerComponents.Find(c => c.Name == item.SystemBlock.Name).FirstOrDefault();
                if (systemBlock == null)
                    return new OperationDetails(false, "Не удалось найти системный блок", this.ToString());
                computer.SystemBlock = systemBlock;
            }
            List<ComputerComponent> RAMs = new List<ComputerComponent>();
            foreach(var r in item.RAM)
            {
                ComputerComponent ram = Database.ComputerComponents.Find(c => c.Name == r.Name && c.Type == ComponentType.RAM).FirstOrDefault();
                if(ram == null)
                    return new OperationDetails(false, "Не удалось найти оперативную память", this.ToString());
                RAMs.Add(ram);
            }
            computer.RAM = RAMs;
            List<ComputerComponent> ROMs = new List<ComputerComponent>();
            foreach (var r in item.ROM)
            {
                ComputerComponent rom = Database.ComputerComponents.Find(c => c.Name == r.Name && c.Type == ComponentType.ROM).FirstOrDefault();
                if (rom == null)
                    return new OperationDetails(false, "Не удалось найти постоянную память", this.ToString());
                ROMs.Add(rom);
            }
            computer.ROM = ROMs;
            List<ComputerComponent> videoCards = new List<ComputerComponent>();
            foreach (var v in item.VideoCard)
            {
                ComputerComponent videoCard = Database.ComputerComponents.Find(c => c.Name == v.Name && c.Type == ComponentType.VideoCard).FirstOrDefault();
                if (videoCard == null)
                    return new OperationDetails(false, "Не удалось найти видеокарту", this.ToString());
                videoCards.Add(videoCard);
            }
            computer.VideoCard = videoCards;
            Database.Computers.Add(computer);
            Database.Save();

            Product localProduct = _mappers.ToProduct.Map<ProductDTO, Product>(product);
            if (localProduct == null)
                return new OperationDetails(false, "Не удалось преобразовать объект", this.ToString());
            localProduct = CreateProduct(localProduct);
            localProduct.Table = Goods.Computer;
            Computer computerInDB = Database.Computers.Find(x => x.Name == localProduct.Name).FirstOrDefault();
            if (computerInDB == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            localProduct.FromTableId = computerInDB.Id;
            Database.Products.Add(localProduct);
            Database.Save();
            return new OperationDetails(true, "Компьютер был успешно добавлен", this.ToString());
        }

        public OperationDetails UpdateComputer(ProductDTO product, ComputerDTO item, string oldComputerName)
        {
            if (product == null || item == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Product localProduct = UpdateProduct(product);
            Database.Products.Update(localProduct);

            Computer oldComputer = Database.Computers.Find(x => x.Name == oldComputerName).FirstOrDefault();
            if (oldComputer == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            if (item.MotherBoard != null && item.MotherBoard.Type == ComponentType.MotherBoard)
            {
                ComputerComponent motherBoard = Database.ComputerComponents.Find(c => c.Name == item.MotherBoard.Name).FirstOrDefault();
                if (motherBoard == null)
                    return new OperationDetails(false, "Не удалось найти материнскую плату", this.ToString());
                oldComputer.MotherBoard = motherBoard;
            }
            if (item.PowerSupply != null && item.MotherBoard.Type == ComponentType.PowerSupply)
            {
                ComputerComponent powerSypply = Database.ComputerComponents.Find(c => c.Name == item.PowerSupply.Name).FirstOrDefault();
                if (powerSypply == null)
                    return new OperationDetails(false, "Не удалось найти блок питания", this.ToString());
                oldComputer.PowerSupply = powerSypply;
            }
            if (item.Processor != null && item.MotherBoard.Type == ComponentType.Processor)
            {
                ComputerComponent processor = Database.ComputerComponents.Find(c => c.Name == item.Processor.Name).FirstOrDefault();
                if (processor == null)
                    return new OperationDetails(false, "Не удалось найти процессор", this.ToString());
                oldComputer.Processor = processor;
            }
            if (item.SystemBlock != null && item.MotherBoard.Type == ComponentType.SystemBlock)
            {
                ComputerComponent systemBlock = Database.ComputerComponents.Find(c => c.Name == item.SystemBlock.Name).FirstOrDefault();
                if (systemBlock == null)
                    return new OperationDetails(false, "Не удалось найти системный блок", this.ToString());
                oldComputer.SystemBlock = systemBlock;
            }
            List<ComputerComponent> RAMs = new List<ComputerComponent>();
            foreach (var r in item.RAM)
            {
                ComputerComponent ram = Database.ComputerComponents.Find(c => c.Name == r.Name && c.Type == ComponentType.RAM).FirstOrDefault();
                if (ram == null)
                    return new OperationDetails(false, "Не удалось найти оперативную память", this.ToString());
                RAMs.Add(ram);
            }
            oldComputer.RAM = RAMs;
            List<ComputerComponent> ROMs = new List<ComputerComponent>();
            foreach (var r in item.ROM)
            {
                ComputerComponent rom = Database.ComputerComponents.Find(c => c.Name == r.Name && c.Type == ComponentType.ROM).FirstOrDefault();
                if (rom == null)
                    return new OperationDetails(false, "Не удалось найти постоянную память", this.ToString());
                ROMs.Add(rom);
            }
            oldComputer.ROM = ROMs;
            List<ComputerComponent> videoCards = new List<ComputerComponent>();
            foreach (var v in item.VideoCard)
            {
                ComputerComponent videoCard = Database.ComputerComponents.Find(c => c.Name == v.Name && c.Type == ComponentType.VideoCard).FirstOrDefault();
                if (videoCard == null)
                    return new OperationDetails(false, "Не удалось найти видеокарту", this.ToString());
                videoCards.Add(videoCard);
            }
            oldComputer.VideoCard = videoCards;

            oldComputer.Properties = _mappers.ToProperty.Map<IEnumerable<PropertyDTO>, ICollection<Property>>(item.Properties);
            Database.Computers.Update(oldComputer);
            Database.Save();
            return new OperationDetails(true, "Компьютер успешно изменён", this.ToString());
        }

        public OperationDetails CreateContainer(ProductDTO product, ContainerDTO item, string oldContainer)
        {
            Product productInDB = Database.Products.Find(p => p.Name == oldContainer).FirstOrDefault();
            if (productInDB != null)
                return UpdateContainer(product, item, oldContainer);

            if (item == null || product == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Container container = _mappers.ToContainer.Map<ContainerDTO, Container>(item);
            container.Name = product.Name;
            List<Product> products = new List<Product>();
            foreach(var pr in item.Products)
            {
                Product prod = Database.Products.Find(p => p.Name == pr.Name).FirstOrDefault();
                if(prod == null)
                    return new OperationDetails(false, "Не удалось найти продукт", this.ToString());
                products.Add(prod);
            }
            container.Products = products;
            Database.Containers.Add(container);
            Database.Save();

            Product localProduct = _mappers.ToProduct.Map<ProductDTO, Product>(product);
            if (localProduct == null)
                return new OperationDetails(false, "Не удалось преобразовать объект", this.ToString());
            localProduct = CreateProduct(localProduct);
            localProduct.Table = Goods.Container;
            Container conainerInDB = Database.Containers.Find(x => x.Name == localProduct.Name).FirstOrDefault();
            if (conainerInDB == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            localProduct.FromTableId = conainerInDB.Id;
            Database.Products.Add(localProduct);
            Database.Save();
            return new OperationDetails(true, "Контейнер был успешно добавлен", this.ToString());
        }

        public OperationDetails UpdateContainer(ProductDTO product, ContainerDTO item, string oldContainerName)
        {
            if (product == null || item == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Product localProduct = UpdateProduct(product);
            Database.Products.Update(localProduct);

            Container oldContainer = Database.Containers.Find(x => x.Name == oldContainerName).FirstOrDefault();
            if (oldContainer == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            List<Product> products = new List<Product>();
            foreach (var pr in item.Products)
            {
                Product prod = Database.Products.Find(p => p.Name == pr.Name).FirstOrDefault();
                if (prod == null)
                    return new OperationDetails(false, "Не удалось найти продукт", this.ToString());
                products.Add(prod);
            }
            oldContainer.Products = products;
            oldContainer.ChanseForLegendary = item.ChanseForLegendary;
            oldContainer.ChanseForRare = item.ChanseForRare;
            oldContainer.Type = item.Type;
            oldContainer.Properties = _mappers.ToProperty.Map<IEnumerable<PropertyDTO>, ICollection<Property>>(item.Properties);
            Database.Containers.Update(oldContainer);
            Database.Save();
            return new OperationDetails(true, "Компьютерный компонент успешно изменён", this.ToString());
        }

        public OperationDetails CreateSkinRarity(SkinRarityDTO item, string oldSkinRarity)
        {
            if (item == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            SkinRarity skinRarity = Database.SkinRareties.Find(x => x.RarityName == oldSkinRarity).FirstOrDefault();
            List<Color> colors = new List<Color>();
            foreach (var c in item.Colors)
            {
                Color color = Database.Colors.Find(cl => cl.Name == c.Name).FirstOrDefault();
                if (color != null)
                    colors.Add(color);
            }
            if (skinRarity != null)
            {
                skinRarity.Colors = colors;
                skinRarity.RarityName = item.RarityName;
                Database.SkinRareties.Update(skinRarity);
                Database.Save();
                return new OperationDetails(true, "Раритетность скина была успешно изменена", this.ToString());
            }
            else
            {
                skinRarity = _mappers.ToSkinRarity.Map<SkinRarityDTO, SkinRarity>(item);
                skinRarity.Colors = colors;
                Database.SkinRareties.Add(skinRarity);
                Database.Save();
                return new OperationDetails(true, "Раритетность скина была успешно добавлена", this.ToString());
            }
        }

        public OperationDetails CreateColor(ColorDTO item, string oldColor)
        {
            if (item == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Color color = Database.Colors.Find(x => x.Name == oldColor).FirstOrDefault();
            if (color != null)
            {
                color.Name = item.Name;
                color.ColorValue = item.ColorValue;
                Database.Colors.Update(color);
                Database.Save();
                return new OperationDetails(true, "Цвет был успешно изменён", this.ToString());
            }
            else
            {
                color = _mappers.ToColor.Map<ColorDTO, Color>(item);
                Database.Colors.Add(color);
                Database.Save();
                return new OperationDetails(true, "Цвет была успешно добавлен", this.ToString());
            }
        }

        public OperationDetails CreateOther(ProductDTO product, OtherDTO item, string oldOther)
        {
            Product productInDB = Database.Products.Find(p => p.Name == oldOther).FirstOrDefault();
            if (productInDB != null)
                return UpdateOther(product, item, oldOther);

            if (item == null || product == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Other other = _mappers.ToOther.Map<OtherDTO, Other>(item);
            item.Name = product.Name;
            Database.Others.Add(other);
            Database.Save();

            Product localProduct = _mappers.ToProduct.Map<ProductDTO, Product>(product);
            if (localProduct == null)
                return new OperationDetails(false, "Не удалось преобразовать объект", this.ToString());
            localProduct = CreateProduct(localProduct);
            localProduct.Table = Goods.Other;
            Other otherInDB = Database.Others.Find(x => x.Name == localProduct.Name).FirstOrDefault();
            if (otherInDB == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            localProduct.FromTableId = otherInDB.Id;
            Database.Products.Add(localProduct);
            Database.Save();
            return new OperationDetails(true, "Товар был успешно добавлен", this.ToString());
        }

        public OperationDetails UpdateOther(ProductDTO product, OtherDTO item, string oldOtherName)
        {
            if (product == null || item == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Product localProduct = UpdateProduct(product);
            Database.Products.Update(localProduct);

            Other oldOther = Database.Others.Find(x => x.Name == oldOtherName).FirstOrDefault();
            if (oldOther == null)
                return new OperationDetails(false, "Не удалось найти объект", this.ToString());
            oldOther.Name = item.Name;
            oldOther.Type = item.Type;
            oldOther.Properties = _mappers.ToProperty.Map<IEnumerable<PropertyDTO>, ICollection<Property>>(item.Properties);
            Database.Others.Update(oldOther);
            Database.Save();
            return new OperationDetails(true, "Товар успешно изменён", this.ToString());
        }

        public OperationDetails CreateColor(ColorDTO item)
        {
            if(item == null)
                return new OperationDetails(false, "ОбЪект ссылается на null", this.ToString());
            Color color = Database.Colors.Find(x => x.Name == item.Name).FirstOrDefault();
            if(color == null)
            {
                color = _mappers.ToColor.Map<ColorDTO, Color>(item);
                Database.Colors.Add(color);
                Database.Save();
                return new OperationDetails(true, "Цвет был успешно добавлен", this.ToString());
            }
            else
            {
                Database.Colors.Update(color);
                Database.Save();
                return new OperationDetails(true, "Цвет был успешно изменён", this.ToString());
            }
        }

        public ICollection<UserDTO> GetUsers()
        {
            ICollection<User> users = Database.ClientManager.GetUsers().ToList();
            return _mappers.ToUserDTO.Map<ICollection<User>, ICollection<UserDTO>>(users);
        }

        public async Task<OperationDetails> CreateEmployee(UserDTO userDTO)
        {
            User user = await Database.UserManager.FindByEmailAsync(userDTO.Email);
            if (user == null)
            {
                user = new User { Email = userDTO.Email, UserName = userDTO.Email, PhoneNumber = userDTO.PhoneNumber, Name = userDTO.Name, Adres = userDTO.Address };
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

        public OperationDetails Ban(string id)
        {
            User user = Database.ClientManager.GetUser(id);
            user.IsBanned = true;
            Database.UserManager.UpdateAsync(user);
            Database.Save();
            return new OperationDetails(true, "Пользователь успешно забанен", this.ToString());
        }

        public OperationDetails Unban(string id)
        {
            User user = Database.ClientManager.GetUser(id);
            user.IsBanned = false;
            Database.UserManager.UpdateAsync(user);
            Database.Save();
            return new OperationDetails(true, "Пользователь успешно разабанен", this.ToString());
        }

        public OperationDetails SoftDelete(Tables tables, int id)
        {
            switch ((int)tables)
            {
                case 1:
                    Database.Clothes.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Вещь успешно удалена", this.ToString());
                case 2:
                    Database.Colors.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Цвет успешно удалён", this.ToString());
                case 3:
                    Database.Comments.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Комментарий успешно удалён", this.ToString());
                case 4:
                    Database.Computers.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Компьютер успешно удалён", this.ToString());
                case 5:
                    Database.ComputerComponents.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Компьютерный компонент успешно удалён", this.ToString());
                case 6:
                    Database.Containers.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Контейнер успешно удалён", this.ToString());
                case 7:
                    Database.Games.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Игра успешно удалена", this.ToString());
                case 8:
                    Database.Orders.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Заказ успешно удалён", this.ToString());
                case 9:
                    Database.Products.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Товар успешно удалён", this.ToString());
                case 10:
                    Database.Properties.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Свойство успешно удалено", this.ToString());
                case 11:
                    Database.Skins.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Скин успешно удалён", this.ToString());
                case 12:
                    Database.SkinRareties.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Редкость для скина успешно удалена", this.ToString());
                case 13:
                    Database.Others.SoftDelete(id);
                    Database.Save();
                    return new OperationDetails(true, "Товар успешно удалён", this.ToString());
                default:
                    return new OperationDetails(false, "Таблица указана не верно", this.ToString());
            }
        }

        public ColorDTO GetColor(int id)
        {
            Color color = Database.Colors.Get(id);
            return _mappers.ToColorDTO.Map<Color, ColorDTO>(color);
        }

        public ProductDTO GetProduct(int id)
        {
            Product product = Database.Products.Get(id);
            return _mappers.ToProductDTO.Map<Product, ProductDTO>(product);
        }

        public ICollection<ProductDTO> GetProducts(Goods good)
        {
            ICollection<Product> products = Database.Products.Find(p => p.Table == good && !(p.IsDeleted)).ToList();
            return _mappers.ToProductDTO.Map<ICollection<Product>, ICollection<ProductDTO>>(products);
        }

        public ICollection<ProductDTO> GetAllProducts()
        {
            ICollection<Product> products = Database.Products.Show().ToList();
            return _mappers.ToProductDTO.Map<ICollection<Product>, ICollection<ProductDTO>>(products);
        }

        public ICollection<ColorDTO> GetColors()
        {
            List<Color> colors = Database.Colors.Show().ToList();
            return _mappers.ToColorDTO.Map<List<Color>, List<ColorDTO>>(colors);
        }

        public SkinDTO GetSkin(int id)
        {
            Skin skin = Database.Skins.Get(id);
            return _mappers.ToSkinDTO.Map<Skin, SkinDTO>(skin);
        }

        public ICollection<SkinDTO> GetSkins()
        {
            ICollection<Skin> skins = Database.Skins.Show();
            return _mappers.ToSkinDTO.Map<ICollection<Skin>, ICollection<SkinDTO>>(skins);
        }

        public GameDTO GetGame(int id)
        {
            Game game = Database.Games.Get(id);
            return _mappers.ToGameDTO.Map<Game, GameDTO>(game);
        }

        public ICollection<GameDTO> GetGames()
        {
            ICollection<Game> games = Database.Games.Show();
            return _mappers.ToGameDTO.Map<ICollection<Game>, ICollection<GameDTO>>(games);
        }

        public ClothDTO GetCloth(int id)
        {
            Cloth cloth = Database.Clothes.Get(id);
            return _mappers.ToClothDTO.Map<Cloth, ClothDTO>(cloth);
        }

        public ICollection<ClothDTO> GetClothes()
        {
            ICollection<Cloth> clothes = Database.Clothes.Show();
            return _mappers.ToClothDTO.Map<ICollection<Cloth>, ICollection<ClothDTO>>(clothes);
        }

        public ComputerComponentDTO GetComputerComponent(int id)
        {
            ComputerComponent compComp = Database.ComputerComponents.Get(id);
            return _mappers.ToComputerComponentDTO.Map<ComputerComponent, ComputerComponentDTO>(compComp);
        }

        public ICollection<ComputerComponentDTO> GetCompComponents()
        {
            ICollection<ComputerComponent> comp = Database.ComputerComponents.Show();
            return _mappers.ToComputerComponentDTO.Map<ICollection<ComputerComponent>, ICollection<ComputerComponentDTO>>(comp);
        }

        public ComputerDTO GetComputer(int id)
        {
            Computer comp = Database.Computers.Get(id);
            return _mappers.ToComputerDTO.Map<Computer, ComputerDTO>(comp);
        }

        public ICollection<ComputerDTO> GetComputers()
        {
            ICollection<Computer> comp = Database.Computers.Show();
            return _mappers.ToComputerDTO.Map<ICollection<Computer>, ICollection<ComputerDTO>>(comp);
        }

        public ContainerDTO GetContainer(int id)
        {
            Container container = Database.Containers.Get(id);
            return _mappers.ToContainerDTO.Map<Container, ContainerDTO>(container);
        }

        public ICollection<ContainerDTO> GetContainers()
        {
            ICollection<Container> comp = Database.Containers.Show();
            return _mappers.ToContainerDTO.Map<ICollection<Container>, ICollection<ContainerDTO>>(comp);
        }

        public OtherDTO GetOther(int id)
        {
            Other other = Database.Others.Get(id);
            return _mappers.ToOtherDTO.Map<Other, OtherDTO>(other);
        }

        public SkinRarityDTO GetSkinRarity(int id)
        {
            SkinRarity skinRarity = Database.SkinRareties.Get(id);
            return _mappers.ToSkinRarityDTO.Map<SkinRarity, SkinRarityDTO>(skinRarity);
        }

        public ICollection<SkinRarityDTO> GetSkinRarities()
        {
            ICollection<SkinRarity> skinRar = Database.SkinRareties.Show();
            return _mappers.ToSkinRarityDTO.Map<ICollection<SkinRarity>, ICollection<SkinRarityDTO>>(skinRar);
        }

        public ICollection<PropertyDTO> GetProperties(string propertyName)
        {
            List<Property> properties = Database.Properties.Find(pr => pr.Name == propertyName).ToList();
            return _mappers.ToPropertyDTO.Map<ICollection<Property>, ICollection<PropertyDTO>>(properties);
        }

        public ICollection<SkinDTO> ProductsIntoSkins(List<ProductDTO> products)
        {
            List<Skin> skins = new List<Skin>();
            foreach(var pr in products)
            {
                Skin skin = Database.Skins.Get(pr.FromTableId);
                if (skin != null)
                    skins.Add(skin);
            }
            return _mappers.ToSkinDTO.Map<ICollection<Skin>, ICollection<SkinDTO>>(skins);
        }

        public ICollection<GameDTO> ProductsIntoGames(List<ProductDTO> products)
        {
            List<Game> games = new List<Game>();
            foreach (var pr in products)
            {
                Game game = Database.Games.Get(pr.FromTableId);
                if (game != null)
                    games.Add(game);
            }
            return _mappers.ToGameDTO.Map<ICollection<Game>, ICollection<GameDTO>>(games);
        }

        public ICollection<ClothDTO> ProductsIntoClothes(List<ProductDTO> products)
        {
            List<Cloth> clothes = new List<Cloth>();
            foreach (var pr in products)
            {
                Cloth cloth = Database.Clothes.Get(pr.FromTableId);
                if (cloth != null)
                    clothes.Add(cloth);
            }
            return _mappers.ToClothDTO.Map<ICollection<Cloth>, ICollection<ClothDTO>>(clothes);
        }

        public ICollection<ComputerComponentDTO> ProductsIntoComputerComponents(List<ProductDTO> products)
        {
            List<ComputerComponent> compComponents = new List<ComputerComponent>();
            foreach (var pr in products)
            {
                ComputerComponent compComp = Database.ComputerComponents.Get(pr.FromTableId);
                if (compComp != null)
                    compComponents.Add(compComp);
            }
            return _mappers.ToComputerComponentDTO.Map<ICollection<ComputerComponent>, ICollection<ComputerComponentDTO>>(compComponents);
        }

        public ICollection<ComputerDTO> ProductsIntoComputers(List<ProductDTO> products)
        {
            List<Computer> comps = new List<Computer>();
            foreach (var pr in products)
            {
                Computer comp = Database.Computers.Get(pr.FromTableId);
                if (comp != null)
                    comps.Add(comp);
            }
            return _mappers.ToComputerDTO.Map<ICollection<Computer>, ICollection<ComputerDTO>>(comps);
        }

        public ICollection<ContainerDTO> ProductsIntoContainers(List<ProductDTO> products)
        {
            List<Container> comps = new List<Container>();
            foreach (var pr in products)
            {
                Container comp = Database.Containers.Get(pr.FromTableId);
                if (comp != null)
                    comps.Add(comp);
            }
            return _mappers.ToContainerDTO.Map<ICollection<Container>, ICollection<ContainerDTO>>(comps);
        }

        public ICollection<ComputerComponentDTO> GetComputerComponentsByType(ComponentType type)
        {
            ICollection<ComputerComponent> comp = Database.ComputerComponents.Find(c => c.Type == type && !(c.IsDeleted)).ToList();
            return _mappers.ToComputerComponentDTO.Map<ICollection<ComputerComponent>, ICollection<ComputerComponentDTO>>(comp);
        }
    }
}