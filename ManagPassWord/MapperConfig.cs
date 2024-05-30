using AutoMapper;
using ManagPassWord.Models;

namespace ManagPassWord
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DebtModel, DebtModelDTO>();
                cfg.CreateMap<DebtModelDTO, DebtModel>();
                //
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>();
            });

            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
