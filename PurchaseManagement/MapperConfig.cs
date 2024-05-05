using AutoMapper;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.ViewModels;

namespace PurchaseManagement
{
    public static class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                //Configuring Employee and EmployeeDTO
                cfg.CreateMap<Purchase_Items,Purchase_ItemsProxy>();
                //Any Other Mapping Configuration ....
            });

            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
        public static Mapper InitializeFromDbAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                //Configuring Employee and EmployeeDTO
                cfg.CreateMap<Purchase_ItemsProxy, Purchase_Items>();
                //Any Other Mapping Configuration ....
            });

            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
