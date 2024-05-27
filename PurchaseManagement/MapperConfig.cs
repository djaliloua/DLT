using AutoMapper;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.MVVM.ViewModels;
using Microsoft.Maui.Devices.Sensors;

namespace PurchaseManagement
{
    public static class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Purchase_Items,Purchase_ItemsDTO>();
                cfg.CreateMap<Purchase_ItemsDTO, Purchase_Items>();
                //
                cfg.CreateMap<Location, MarketLocation>();
                cfg.CreateMap<MarketLocation, Location>();
                //
                cfg.CreateMap<Location, MarketLocationDTO>();
                cfg.CreateMap<MarketLocationDTO, Location>();

                //
                cfg.CreateMap<MarketLocation, MarketLocationDTO>();
                cfg.CreateMap<MarketLocationDTO, MarketLocation>();
                //
                cfg.CreateMap<Account, AccountDTO>();
                cfg.CreateMap<AccountDTO, Account>();
                //
                cfg.CreateMap<Purchases, PurchasesDTO>();
                cfg.CreateMap<PurchasesDTO, Purchases>();
                //
                cfg.CreateMap<PurchaseStatistics, PurchaseStatisticsDTO>();
                cfg.CreateMap<PurchaseStatisticsDTO, PurchaseStatistics>();
                //

            });

            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
