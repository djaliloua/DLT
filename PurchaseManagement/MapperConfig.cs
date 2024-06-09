using AutoMapper;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.MVVM.ViewModels;
using Loc = PurchaseManagement.MVVM.Models;
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
                cfg.CreateMap<Loc.Location, Location>();
                cfg.CreateMap<Location, Loc.Location>();
                //
                cfg.CreateMap<Location, LocationDto>();
                cfg.CreateMap<LocationDto, Location>();

                //
                cfg.CreateMap<MVVM.Models.Location, LocationDto>();
                cfg.CreateMap<LocationDto, MVVM.Models.Location>();
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
