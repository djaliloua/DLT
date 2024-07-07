﻿using ManagPassWord.Models;

namespace ManagPassWord.DataAcessLayer.Abstractions
{
    public interface IPasswordRepository: IGenericRepository<User>
    {
        Task<int> SaveToCsv();
    }
}
