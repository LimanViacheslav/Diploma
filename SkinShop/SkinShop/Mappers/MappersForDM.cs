using AutoMapper;
using SkinShop.BLL.Identity.IdentityDTO;
using SkinShop.BLL.SkinShop.SkinShopDTO;
using SkinShop.Models.Account;
using SkinShop.Models.SkinShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkinShop.Mappers
{
    public class MappersForDM
    {
        public virtual IMapper ToDeliveryDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<DeliveryDM, DeliveryDTO>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToDeliveryDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<DeliveryDTO, DeliveryDM>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToCommentDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<CommentDM, CommentDTO>()
                .ForMember(x => x.User, c => c.MapFrom(k => ToUserDTO.Map<UserDM, UserDTO>(k.User))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToCommentDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<CommentDTO, CommentDM>()
                .ForMember(x => x.User, c => c.MapFrom(k => ToUserDM.Map<UserDTO, UserDM>(k.User))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToPropertyDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<PropertyDM, PropertyDTO>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToPropertyDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<PropertyDTO, PropertyDM>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToProductDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ProductDM, ProductDTO>()
                .ForMember(x => x.Images, c => c.MapFrom(k => ToImageDTO.Map<IEnumerable<ImageDM>, IEnumerable<ImageDTO>>(k.Images)))
                ).CreateMapper();
            }
        }

        public virtual IMapper ToProductDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductDM>()
                .ForMember(x => x.Images, c => c.MapFrom(k => ToImageDM.Map<IEnumerable<ImageDTO>, IEnumerable<ImageDM>>(k.Images)))
                ).CreateMapper();
            }
        }

        public virtual IMapper ToColorDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ColorDM, ColorDTO>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToColorDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ColorDTO, ColorDM>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToStringDataDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<StringDataDM, StringDataDTO>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToStringDataDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<StringDataDTO, StringDataDM>())
                    .CreateMapper();
            }
        }

        public virtual IMapper ToClothDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ClothDM, ClothDTO>()
                .ForMember(x => x.Colors, c => c.MapFrom(k => ToColorDTO.Map<IEnumerable<ColorDM>, IEnumerable<ColorDTO>>(k.Colors)))
                .ForMember(x => x.Sizes, c => c.MapFrom(k => ToStringDataDTO.Map<ICollection<StringDataDM>, ICollection<StringDataDTO>>(k.Sizes)))
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<PropertyDM>, IEnumerable<PropertyDTO>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToClothDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ClothDTO, ClothDM>()
                .ForMember(x => x.Colors, c => c.MapFrom(k => ToColorDM.Map<IEnumerable<ColorDTO>, IEnumerable<ColorDM>>(k.Colors)))
                .ForMember(x => x.Sizes, c => c.MapFrom(k => ToStringDataDM.Map<ICollection<StringDataDTO>, ICollection<StringDataDM>>(k.Sizes)))
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDM.Map<IEnumerable<PropertyDTO>, IEnumerable<PropertyDM>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToComputerDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ComputerDM, ComputerDTO>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<PropertyDM>, IEnumerable<PropertyDTO>>(k.Properties)))
                .ForMember(x => x.MotherBoard, c => c.MapFrom(k => ToComputerComponentDTO.Map<ComputerComponentDM, ComputerComponentDTO>(k.MotherBoard)))
                .ForMember(x => x.PowerSupply, c => c.MapFrom(k => ToComputerComponentDTO.Map<ComputerComponentDM, ComputerComponentDTO>(k.PowerSupply)))
                .ForMember(x => x.Processor, c => c.MapFrom(k => ToComputerComponentDTO.Map<ComputerComponentDM, ComputerComponentDTO>(k.Processor)))
                .ForMember(x => x.SystemBlock, c => c.MapFrom(k => ToComputerComponentDTO.Map<ComputerComponentDM, ComputerComponentDTO>(k.SystemBlock)))
                .ForMember(x => x.RAM, c => c.MapFrom(k => ToComputerComponentDTO.Map<ICollection<ComputerComponentDM>, ICollection<ComputerComponentDTO>>(k.RAM)))
                .ForMember(x => x.ROM, c => c.MapFrom(k => ToComputerComponentDTO.Map<ICollection<ComputerComponentDM>, ICollection<ComputerComponentDTO>>(k.ROM)))
                .ForMember(x => x.VideoCard, c => c.MapFrom(k => ToComputerComponentDTO.Map<ICollection<ComputerComponentDM>, ICollection<ComputerComponentDTO>>(k.VideoCard)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToComputerDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ComputerDTO, ComputerDM>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDM.Map<IEnumerable<PropertyDTO>, IEnumerable<PropertyDM>>(k.Properties)))
                .ForMember(x => x.MotherBoard, c => c.MapFrom(k => ToComputerComponentDM.Map<ComputerComponentDTO, ComputerComponentDM>(k.MotherBoard)))
                .ForMember(x => x.PowerSupply, c => c.MapFrom(k => ToComputerComponentDM.Map<ComputerComponentDTO, ComputerComponentDM>(k.PowerSupply)))
                .ForMember(x => x.Processor, c => c.MapFrom(k => ToComputerComponentDM.Map<ComputerComponentDTO, ComputerComponentDM>(k.Processor)))
                .ForMember(x => x.SystemBlock, c => c.MapFrom(k => ToComputerComponentDM.Map<ComputerComponentDTO, ComputerComponentDM>(k.SystemBlock)))
                .ForMember(x => x.RAM, c => c.MapFrom(k => ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, ICollection<ComputerComponentDM>>(k.RAM)))
                .ForMember(x => x.ROM, c => c.MapFrom(k => ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, ICollection<ComputerComponentDM>>(k.ROM)))
                .ForMember(x => x.VideoCard, c => c.MapFrom(k => ToComputerComponentDM.Map<ICollection<ComputerComponentDTO>, ICollection<ComputerComponentDM>>(k.VideoCard)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToComputerComponentDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ComputerComponentDM, ComputerComponentDTO>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<PropertyDM>, IEnumerable<PropertyDTO>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToComputerComponentDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ComputerComponentDTO, ComputerComponentDM>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDM.Map<IEnumerable<PropertyDTO>, IEnumerable<PropertyDM>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToContainerDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ContainerDM, ContainerDTO>()
                .ForMember(x => x.Products, c => c.MapFrom(k => ToProductDTO.Map<IEnumerable<ProductDM>, IEnumerable<ProductDTO>>(k.Products)))
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<PropertyDM>, IEnumerable<PropertyDTO>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToContainerDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ContainerDTO, ContainerDM>()
                .ForMember(x => x.Products, c => c.MapFrom(k => ToProductDM.Map<IEnumerable<ProductDTO>, IEnumerable<ProductDM>>(k.Products)))
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDM.Map<IEnumerable<PropertyDTO>, IEnumerable<PropertyDM>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToOtherDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<OtherDM, OtherDTO>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<PropertyDM>, IEnumerable<PropertyDTO>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToOtherDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<OtherDTO, OtherDM>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDM.Map<IEnumerable<PropertyDTO>, IEnumerable<PropertyDM>>(k.Properties))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToSkinDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<SkinDM, SkinDTO>()
                .ForMember(x => x.Game, c => c.MapFrom(k => ToGameDTO.Map<GameDM, GameDTO>(k.Game)))
                .ForMember(x => x.SkinRarity, c => c.MapFrom(k => ToSkinRarityDTO.Map<SkinRaretyDM, SkinRarityDTO>(k.SkinRarity)))
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<PropertyDM>, IEnumerable<PropertyDTO>>(k.Properties)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToSkinDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<SkinDTO, SkinDM>()
                .ForMember(x => x.Game, c => c.MapFrom(k => ToGameDM.Map<GameDTO, GameDM>(k.Game)))
                .ForMember(x => x.SkinRarity, c => c.MapFrom(k => ToSkinRarityDM.Map<SkinRarityDTO, SkinRaretyDM>(k.SkinRarity)))
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDM.Map<IEnumerable<PropertyDTO>, IEnumerable<PropertyDM>>(k.Properties)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToImageDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ImageDM, ImageDTO>()
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToImageDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ImageDTO, ImageDM>()
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToSkinRarityDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<SkinRaretyDM, SkinRarityDTO>()
                .ForMember(x => x.Colors, c => c.MapFrom(k => ToColorDTO.Map<ICollection<ColorDM>, ICollection<ColorDTO>>(k.Colors))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToSkinRarityDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<SkinRarityDTO, SkinRaretyDM>()
                .ForMember(x => x.Colors, c => c.MapFrom(k => ToColorDM.Map<ICollection<ColorDTO>, ICollection<ColorDM>>(k.Colors))))

                    .CreateMapper();
            }
        }

        public virtual IMapper ToGameDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<GameDM, GameDTO>()
                .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDTO.Map<IEnumerable<PropertyDM>, IEnumerable<PropertyDTO>>(k.Properties)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToGameDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<GameDTO, GameDM>()
                 .ForMember(x => x.Properties, c => c.MapFrom(k => ToPropertyDM.Map<IEnumerable<PropertyDTO>, IEnumerable<PropertyDM>>(k.Properties)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToUserDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<UserDM, UserDTO>()
                .ForMember(x => x.Client, c => c.MapFrom(k => ToClientProfileDTO.Map<ClientProfileDM, ClientProfileDTO>(k.Client)))
                .ForMember(x => x.Image, c => c.MapFrom(k => ToImageDTO.Map<ImageDM, ImageDTO>(k.Image))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToUserDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserDM>()
                .ForMember(x => x.Client, c => c.MapFrom(k => ToClientProfileDM.Map<ClientProfileDTO, ClientProfileDM>(k.Client)))
                .ForMember(x => x.Image, c => c.MapFrom(k => ToImageDM.Map<ImageDTO, ImageDM>(k.Image))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToContainerToCountOpensDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ContainerToCountOpensDM, ContainerToCountOpensDTO>()
                .ForMember(x => x.Container, c => c.MapFrom(k => ToProductDTO.Map<ProductDM, ProductDTO>(k.Container)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToContainerToCountOpensDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ContainerToCountOpensDTO, ContainerToCountOpensDM>()
                .ForMember(x => x.Container, c => c.MapFrom(k => ToProductDM.Map<ProductDTO, ProductDM>(k.Container)))
                ).CreateMapper();
            }
        }

        public virtual IMapper ToClientProfileDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ClientProfileDM, ClientProfileDTO>()
                .ForMember(x => x.Basket, c => c.MapFrom(k => ToBasketDTO.Map<BasketDM, BasketDTO>(k.Basket)))
                .ForMember(x => x.Favorites, c => c.MapFrom(k => ToFavoritesDTO.Map<FavoritesDM, FavoritesDTO>(k.Favorites)))
                .ForMember(x => x.Containers, c => c.MapFrom(k => ToContainerToCountOpensDTO.Map<IEnumerable<ContainerToCountOpensDM>, IEnumerable<ContainerToCountOpensDTO>>(k.Containers)))
                .ForMember(x => x.Products, c => c.MapFrom(k => ToContainerToCountOpensDTO.Map<IEnumerable<ContainerToCountOpensDM>, IEnumerable<ContainerToCountOpensDTO>>(k.Products)))
                .ForMember(x => x.ContainerToCountOpens, c => c.MapFrom(k => ToContainerToCountOpensDTO.Map<IEnumerable<ContainerToCountOpensDM>, IEnumerable<ContainerToCountOpensDTO>>(k.ContainerToCountOpens)))
                ).CreateMapper();

            }
        }

        public virtual IMapper ToClientProfileDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<ClientProfileDTO, ClientProfileDM>()
                .ForMember(x => x.Basket, c => c.MapFrom(k => ToBasketDM.Map<BasketDTO, BasketDM>(k.Basket)))
                .ForMember(x => x.Favorites, c => c.MapFrom(k => ToFavoritesDM.Map<FavoritesDTO, FavoritesDM>(k.Favorites)))
                .ForMember(x => x.Containers, c => c.MapFrom(k => ToContainerToCountOpensDM.Map<IEnumerable<ContainerToCountOpensDTO>, IEnumerable<ContainerToCountOpensDM>>(k.Containers)))
                .ForMember(x => x.Products, c => c.MapFrom(k => ToContainerToCountOpensDM.Map<IEnumerable<ContainerToCountOpensDTO>, IEnumerable<ContainerToCountOpensDM>>(k.Products)))
                .ForMember(x => x.ContainerToCountOpens, c => c.MapFrom(k => ToContainerToCountOpensDM.Map<IEnumerable<ContainerToCountOpensDTO>, IEnumerable<ContainerToCountOpensDM>>(k.ContainerToCountOpens)))
                ).CreateMapper();

            }
        }

        public virtual IMapper ToBasketDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<BasketDM, BasketDTO>()
                    .ForMember(x => x.Products, c => c.MapFrom(k => ToProductDTO.Map<IEnumerable<ProductDM>, IEnumerable<ProductDTO>>(k.Products))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToBasketDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<BasketDTO, BasketDM>()
                    .ForMember(x => x.Products, c => c.MapFrom(k => ToProductDM.Map<IEnumerable<ProductDTO>, IEnumerable<ProductDM>>(k.Products))))
                    .CreateMapper();
            }
        }

        public virtual IMapper ToFavoritesDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<FavoritesDM, FavoritesDTO>()
                .ForMember(x => x.Products, c => c.MapFrom(k => ToProductDTO.Map<IEnumerable<ProductDM>, IEnumerable<ProductDTO>>(k.Products)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToFavoritesDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<FavoritesDTO, FavoritesDM>()
                .ForMember(x => x.Products, c => c.MapFrom(k => ToProductDM.Map<IEnumerable<ProductDTO>, IEnumerable<ProductDM>>(k.Products)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToOrderCountDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<OrderCountDM, OrderCountDTO>()
                .ForMember(x => x.Product, c => c.MapFrom(k => ToProductDTO.Map<ProductDM, ProductDTO>(k.Product)))
                .ForMember(x => x.Color, c => c.MapFrom(k => ToColorDTO.Map<ColorDM, ColorDTO>(k.Color)))
                )
                   .CreateMapper();
            }
        }

        public virtual IMapper ToOrderCountDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<OrderCountDTO, OrderCountDM>()
                .ForMember(x => x.Product, c => c.MapFrom(k => ToProductDM.Map<ProductDTO, ProductDM>(k.Product)))
                .ForMember(x => x.Color, c => c.MapFrom(k => ToColorDM.Map<ColorDTO, ColorDM>(k.Color)))
                )
                   .CreateMapper();
            }
        }

        public virtual IMapper ToOrderDTO
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<OrderDM, OrderDTO>()
                .ForMember(x => x.Client, c => c.MapFrom(k => ToUserDTO.Map<UserDM, UserDTO>(k.Client)))
                .ForMember(x => x.Employee, c => c.MapFrom(k => ToUserDTO.Map<UserDM, UserDTO>(k.Employee)))
                .ForMember(x => x.OrderCounts, c => c.MapFrom(k => ToOrderCountDTO.Map<IEnumerable<OrderCountDM>, IEnumerable<OrderCountDTO>>(k.OrderCounts)))
                .ForMember(x => x.Delivery, c => c.MapFrom(k => ToDeliveryDTO.Map<DeliveryDM, DeliveryDTO>(k.Delivery)))
                )
                    .CreateMapper();
            }
        }

        public virtual IMapper ToOrderDM
        {
            get
            {
                return new MapperConfiguration(cfg => cfg.CreateMap<OrderDTO, OrderDM>()
                .ForMember(x => x.Client, c => c.MapFrom(k => ToUserDM.Map<UserDTO, UserDM>(k.Client)))
                .ForMember(x => x.Employee, c => c.MapFrom(k => ToUserDM.Map<UserDTO, UserDM>(k.Employee)))
                .ForMember(x => x.OrderCounts, c => c.MapFrom(k => ToOrderCountDM.Map<IEnumerable<OrderCountDTO>, IEnumerable<OrderCountDM>>(k.OrderCounts)))
                .ForMember(x => x.Delivery, c => c.MapFrom(k => ToDeliveryDM.Map<DeliveryDTO, DeliveryDM>(k.Delivery)))
                )
                    .CreateMapper();
            }
        }
    }
}