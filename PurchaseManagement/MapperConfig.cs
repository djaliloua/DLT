using AutoMapper;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;

namespace PurchaseManagement
{
    public static class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product,ProductDto>();
                cfg.CreateMap<ProductDto, Product>();
                //
                cfg.CreateMap<MVVM.Models.Location, Microsoft.Maui.Devices.Sensors.Location>();
                cfg.CreateMap<Microsoft.Maui.Devices.Sensors.Location, MVVM.Models.Location>();
                //
                cfg.CreateMap<Microsoft.Maui.Devices.Sensors.Location, LocationDTO>();
                cfg.CreateMap<LocationDTO, Microsoft.Maui.Devices.Sensors.Location>();

                //
                cfg.CreateMap<MVVM.Models.Location, LocationDTO>();
                cfg.CreateMap<LocationDTO, MVVM.Models.Location>();
                //
                cfg.CreateMap<Account, AccountDTO>();
                cfg.CreateMap<AccountDTO, Account>();
                //
                cfg.CreateMap<Purchase, PurchaseDto>();
                cfg.CreateMap<PurchaseDto, Purchase>();
                //
                cfg.CreateMap<PurchaseStatistics, ProductStatisticsDto>();
                cfg.CreateMap<ProductStatisticsDto, PurchaseStatistics>();
                //

            });

            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
