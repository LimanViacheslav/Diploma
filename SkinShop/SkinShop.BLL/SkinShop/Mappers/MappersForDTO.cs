using AutoMapper;
using SkinShop.BLL.Identity.IdentityDTO;
using SkinShop.BLL.SkinShop.SkinShopDTO;
using SkinShop.DL.Entities.Identity;
using SkinShop.DL.Entities.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinShop.BLL.SkinShop.Mappers
{
    public class MappersForDTO
    {
        public virtual IMapper ToDeliveryDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Delivery, DeliveryDTO>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToDelivery
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<DeliveryDTO, Delivery>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToCommentDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Comment, CommentDTO>()
                .ForMember(x => x.User, c => c.MapFrom(k => ToUserDTO.Map<User, UserDTO>(k.User))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToComment
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<CommentDTO, Comment>()
               .ForMember(x => x.User, c => c.MapFrom(k => ToUser.Map<UserDTO, User>(k.User))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToPropertyDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Property, PropertyDTO>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToProperty
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<PropertyDTO, Property>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToProductDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDTO>()
                .ForMember(x => x.Images, c => c.MapFrom(k => ToImageDTO.Map<IEnumerable<Image>, IEnumerable<ImageDTO>>(k.Images)))
                ).CreateMapper();
            }
        }

        public virtual IMapper ToProduct
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, Product>()
                .ForMember(x => x.Images, c => c.MapFrom(k => ToImage.Map<IEnumerable<ImageDTO>, IEnumerable<Image>>(k.Images)))
                ).CreateMapper();
            }
        }

        public virtual IMapper ToColorDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Color, ColorDTO>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToColor
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ColorDTO, Color>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToStringDataDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<StringData, StringDataDTO>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToStringData
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<StringDataDTO, StringData>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToClothDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Cloth, ClothDTO>()
                .ForMember(x => x.Colors, c => c.MapFrom(k => ToColorDTO.Map<IEnumerable<Color>, IEnumerable<ColorDTO>>(k.Colors)))
                .ForMember(x => x.Sizes, c => c.MapFrom(k => ToStringDataDTO.Map<ICollection<StringData>, ICollection<StringDataDTO>>(k.Sizes)))
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<Property>, IEnumerable<PropertyDTO>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToCloth
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ClothDTO, Cloth>()
                .ForMember(x => x.Colors, c => c.MapFrom(k => ToColor.Map<IEnumerable<ColorDTO>, IEnumerable<Color>>(k.Colors)))
                .ForMember(x => x.Sizes, c => c.MapFrom(k => ToStringData.Map<ICollection<StringDataDTO>, ICollection<StringData>>(k.Sizes)))
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToProperty.Map<IEnumerable<PropertyDTO>, IEnumerable<Property>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToComputerDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Computer, ComputerDTO>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<Property>, IEnumerable<PropertyDTO>>(k.Properties)))
                .ForMember(x => x.MotherBoard, c => c.MapFrom(k => ToComputerComponentDTO.Map<ComputerComponent, ComputerComponentDTO>(k.MotherBoard)))
                .ForMember(x => x.PowerSupply, c => c.MapFrom(k => ToComputerComponentDTO.Map<ComputerComponent, ComputerComponentDTO>(k.PowerSupply)))
                .ForMember(x => x.Processor, c => c.MapFrom(k => ToComputerComponentDTO.Map<ComputerComponent, ComputerComponentDTO>(k.Processor)))
                .ForMember(x => x.SystemBlock, c => c.MapFrom(k => ToComputerComponentDTO.Map<ComputerComponent, ComputerComponentDTO>(k.SystemBlock)))
                .ForMember(x => x.RAM, c => c.MapFrom(k => ToComputerComponentDTO.Map<ICollection<ComputerComponent>, ICollection<ComputerComponentDTO>>(k.RAM)))
                .ForMember(x => x.ROM, c => c.MapFrom(k => ToComputerComponentDTO.Map<ICollection<ComputerComponent>, ICollection<ComputerComponentDTO>>(k.ROM)))
                .ForMember(x => x.VideoCard, c => c.MapFrom(k => ToComputerComponentDTO.Map<ICollection<ComputerComponent>, ICollection<ComputerComponentDTO>>(k.VideoCard)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToComputer
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ComputerDTO, Computer>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToProperty.Map<IEnumerable<PropertyDTO>, IEnumerable<Property>>(k.Properties)))
                .ForMember(x => x.MotherBoard, c => c.MapFrom(k => ToComputerComponent.Map<ComputerComponentDTO, ComputerComponent>(k.MotherBoard)))
                .ForMember(x => x.PowerSupply, c => c.MapFrom(k => ToComputerComponent.Map<ComputerComponentDTO, ComputerComponent>(k.PowerSupply)))
                .ForMember(x => x.Processor, c => c.MapFrom(k => ToComputerComponent.Map<ComputerComponentDTO, ComputerComponent>(k.Processor)))
                .ForMember(x => x.SystemBlock, c => c.MapFrom(k => ToComputerComponent.Map<ComputerComponentDTO, ComputerComponent>(k.SystemBlock)))
                .ForMember(x => x.RAM, c => c.MapFrom(k => ToComputerComponent.Map<ICollection<ComputerComponentDTO>, ICollection<ComputerComponent>>(k.RAM)))
                .ForMember(x => x.ROM, c => c.MapFrom(k => ToComputerComponent.Map<ICollection<ComputerComponentDTO>, ICollection<ComputerComponent>>(k.ROM)))
                .ForMember(x => x.VideoCard, c => c.MapFrom(k => ToComputerComponent.Map<ICollection<ComputerComponentDTO>, ICollection<ComputerComponent>>(k.VideoCard)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToComputerComponentDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ComputerComponent, ComputerComponentDTO>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<Property>, IEnumerable<PropertyDTO>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToComputerComponent
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ComputerComponentDTO, ComputerComponent>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToProperty.Map<IEnumerable<PropertyDTO>, IEnumerable<Property>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToContainerDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Container, ContainerDTO>()
                .ForMember(x => x.Products, c => c.MapFrom(k => ToProductDTO.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(k.Products)))
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<Property>, IEnumerable<PropertyDTO>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToContainer
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ContainerDTO, Container>()
                .ForMember(x => x.Products, c => c.MapFrom(k => ToProduct.Map<IEnumerable<ProductDTO>, IEnumerable<Product>>(k.Products)))
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToProperty.Map<IEnumerable<PropertyDTO>, IEnumerable<Property>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToOtherDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Other, OtherDTO>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<Property>, IEnumerable<PropertyDTO>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToOther
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<OtherDTO, Other>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToProperty.Map<IEnumerable<PropertyDTO>, IEnumerable<Property>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToSkinDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Skin, SkinDTO>()
                .ForMember(x => x.Game, c => c.MapFrom(k => ToGameDTO.Map<Game, GameDTO>(k.Game)))
                .ForMember(x => x.SkinRarity, c => c.MapFrom(k => ToSkinRarityDTO.Map<SkinRarity, SkinRarityDTO>(k.SkinRarity)))
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<Property>, IEnumerable<PropertyDTO>>(k.Properties)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToSkin
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<SkinDTO, Skin>()
                .ForMember(x => x.Game, c => c.MapFrom(k => ToGame.Map<GameDTO, Game>(k.Game)))
                .ForMember(x => x.SkinRarity, c => c.MapFrom(k => ToSkinRarity.Map<SkinRarityDTO, SkinRarity>(k.SkinRarity)))
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToProperty.Map<IEnumerable<PropertyDTO>, IEnumerable<Property>>(k.Properties)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToImageDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Image, ImageDTO>()
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToImage
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ImageDTO, Image>()
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToSkinRarityDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<SkinRarity, SkinRarityDTO>()
                .ForMember(x => x.Colors, c => c.MapFrom(k => ToColorDTO.Map<ICollection<Color>, ICollection<ColorDTO>>(k.Colors))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToSkinRarity
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<SkinRarityDTO, SkinRarity>()
                .ForMember(x => x.Colors, c => c.MapFrom(k => ToColor.Map<ICollection<ColorDTO>, ICollection<Color>>(k.Colors))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToGameDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Game, GameDTO>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<Property>, IEnumerable<PropertyDTO>>(k.Properties)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToGame
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<GameDTO, Game>()
                 .ForMember(x => x.Properties, c => c.MapFrom(k => ToProperty.Map<IEnumerable<PropertyDTO>, IEnumerable<Property>>(k.Properties)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToUserDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()
                .ForMember(x => x.Client, c => c.MapFrom(k => ToClientProfileDTO.Map<ClientProfile, ClientProfileDTO>(k.Client)))
                .ForMember(x => x.Image, c => c.MapFrom(k => ToImageDTO.Map<Image, ImageDTO>(k.Image))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToUser
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()
                .ForMember(x => x.Client, c => c.MapFrom(k => ToClientProfile.Map<ClientProfileDTO, ClientProfile>(k.Client)))
                .ForMember(x => x.Image, c => c.MapFrom(k => ToImage.Map<ImageDTO, Image>(k.Image))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToContainerToCountOpensDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ContainerToCountOpens, ContainerToCountOpensDTO>()
                .ForMember(x => x.Container, c => c.MapFrom(k => ToProductDTO.Map<Product, ProductDTO>(k.Container))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToContainerToCountOpens
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ContainerToCountOpensDTO, ContainerToCountOpens>()
                .ForMember(x => x.Container, c => c.MapFrom(k => ToProduct.Map<ProductDTO, Product>(k.Container))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToClientProfileDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ClientProfile, ClientProfileDTO>()
                .ForMember(x => x.Basket, c => c.MapFrom(k => ToBasketDTO.Map<Basket, BasketDTO>(k.Basket)))
                .ForMember(x => x.Favorites, c => c.MapFrom(k => ToFavoritesDTO.Map<Favorites, FavoritesDTO>(k.Favorites)))
                .ForMember(x => x.Containers, c => c.MapFrom(k => ToContainerToCountOpensDTO.Map<IEnumerable<ContainerToCountOpens>, IEnumerable<ContainerToCountOpensDTO>>(k.Containers)))
                .ForMember(x => x.Products, c => c.MapFrom(k => ToContainerToCountOpensDTO.Map<IEnumerable<ContainerToCountOpens>, IEnumerable<ContainerToCountOpensDTO>>(k.Products)))
                .ForMember(x => x.ContainerToCountOpens, c => c.MapFrom(k => ToContainerToCountOpensDTO.Map<IEnumerable<ContainerToCountOpens>, IEnumerable<ContainerToCountOpensDTO>>(k.ContainerToCountOpens)))
                ).CreateMapper();
                
            }
        }

        public virtual IMapper ToClientProfile
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ClientProfileDTO, ClientProfile>()
                .ForMember(x => x.Basket, c => c.MapFrom(k => ToBasket.Map<BasketDTO, Basket>(k.Basket)))
                .ForMember(x => x.Favorites, c => c.MapFrom(k => ToFavorites.Map<FavoritesDTO, Favorites>(k.Favorites)))
                .ForMember(x => x.Containers, c => c.MapFrom(k => ToContainerToCountOpens.Map<IEnumerable<ContainerToCountOpensDTO>, IEnumerable<ContainerToCountOpens>>(k.Containers)))
                .ForMember(x => x.Products, c => c.MapFrom(k => ToContainerToCountOpens.Map<IEnumerable<ContainerToCountOpensDTO>, IEnumerable<ContainerToCountOpens>>(k.Products)))
                .ForMember(x => x.ContainerToCountOpens, c => c.MapFrom(k => ToContainerToCountOpens.Map<IEnumerable<ContainerToCountOpensDTO>, IEnumerable<ContainerToCountOpens>>(k.ContainerToCountOpens)))
                ).CreateMapper();

            }
        }

        public virtual IMapper ToBasketDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Basket, BasketDTO>()
                    .ForMember(x => x.Products, c => c.MapFrom(k => ToProductDTO.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(k.Products))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToBasket
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<BasketDTO, Basket>()
                    .ForMember(x => x.Products, c => c.MapFrom(k => ToProduct.Map<IEnumerable<ProductDTO>, IEnumerable<Product>>(k.Products))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToFavoritesDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Favorites,FavoritesDTO>()
                .ForMember(x => x.Products, c => c.MapFrom(k => ToProductDTO.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(k.Products)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToFavorites
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<FavoritesDTO, Favorites>()
                .ForMember(x => x.Products, c => c.MapFrom(k => ToProduct.Map<IEnumerable<ProductDTO>, IEnumerable<Product>>(k.Products)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToOrderCountDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<OrderCount, OrderCountDTO>()
                .ForMember(x => x.Product, c => c.MapFrom(k => ToProductDTO.Map<Product, ProductDTO>(k.Product)))
                .ForMember(x => x.Color, c => c.MapFrom(k => ToColorDTO.Map<Color, ColorDTO>(k.Color)))
                )
                   .CreateMapper();
            }
        }

        public virtual IMapper ToOrderCount
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<OrderCountDTO, OrderCount>()
                .ForMember(x => x.Product, c => c.MapFrom(k => ToProduct.Map<ProductDTO, Product>(k.Product)))
                .ForMember(x => x.Color, c => c.MapFrom(k => ToColor.Map<ColorDTO, Color>(k.Color)))
                )
                   .CreateMapper();
            }
        }

        public virtual IMapper ToOrderDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDTO>()
                .ForMember(x => x.Client, c => c.MapFrom(k => ToUserDTO.Map<User, UserDTO>(k.Client)))
                .ForMember(x => x.Employee, c => c.MapFrom(k => ToUserDTO.Map<User, UserDTO>(k.Employee)))
                .ForMember(x => x.OrderCounts, c => c.MapFrom(k => ToOrderCountDTO.Map<IEnumerable<OrderCount>, IEnumerable<OrderCountDTO>>(k.OrderCounts)))
                .ForMember(x => x.Delivery, c => c.MapFrom(k => ToDeliveryDTO.Map<Delivery, DeliveryDTO>(k.Delivery)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToOrder
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<OrderDTO, Order>()
                .ForMember(x => x.Client, c => c.MapFrom(k => ToUser.Map<UserDTO, User>(k.Client)))
                .ForMember(x => x.Employee, c => c.MapFrom(k => ToUser.Map<UserDTO, User>(k.Employee)))
                .ForMember(x => x.OrderCounts, c => c.MapFrom(k => ToOrderCount.Map<IEnumerable<OrderCountDTO>, IEnumerable<OrderCount>>(k.OrderCounts)))
                .ForMember(x => x.Delivery, c => c.MapFrom(k => ToDelivery.Map<DeliveryDTO, Delivery>(k.Delivery)))
                )
                    .CreateMapper();
            }
        }
    }
}