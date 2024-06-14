using AutoMapper;
using PurchaseManagement.MVVM.Models.Accounts;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.MVVM.Models.MarketModels;
using MarketModels = PurchaseManagement.MVVM.Models.MarketModels;
using Location = Microsoft.Maui.Devices.Sensors.Location;


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
                cfg.CreateMap<MarketModels.Location, Location>();
                cfg.CreateMap<Location, MarketModels.Location>();
                //
                cfg.CreateMap<Location, LocationDto>();
                cfg.CreateMap<LocationDto, Location>();

                //
                cfg.CreateMap<MarketModels.Location, LocationDto>();
                cfg.CreateMap<LocationDto, MarketModels.Location>();
                //
                cfg.CreateMap<Account, AccountDTO>();
                cfg.CreateMap<AccountDTO, Account>();
                //
                cfg.CreateMap<Purchase, PurchasesDTO>();
                cfg.CreateMap<PurchasesDTO, Purchase>();
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
