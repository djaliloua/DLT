using CameraApp.DataAccess.Abstraction;
using CameraApp.DataAccess.Repository;
using CameraApp.Models;
using CameraApp.ViewModels;
using Mapster;
using Patterns.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraApp.ExtensionMethods
{
    public static class MapperExtension
    {
        public static IList<CameraViewModel> ToDto(this List<Camera> items) => items.Adapt<List<CameraViewModel>>();
        public static CameraViewModel ToDto(this Camera item) => item.Adapt<CameraViewModel>();
        public static Camera FromDto(this CameraViewModel item) => item.Adapt<Camera>();

    }
    public static class Extensions
    {
        public static MauiAppBuilder PagesExtensions(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            
            return mauiAppBuilder;
        }
        public static MauiAppBuilder RepositoryExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<ICameraRepository, CameraRepository>();

            return mauiAppBuilder;
        }
        public static MauiAppBuilder ContextExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<RepositoryContext>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder ViewModelsExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainViewModel>();
            mauiAppBuilder.Services.AddSingleton<DialogViewModel>();
            mauiAppBuilder.Services.AddSingleton<ComboBoViewModel>();
            mauiAppBuilder.Services.AddSingleton<DialogListOfCameraViewModel>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder UtilityExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddMapster();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder LoadBIExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddScoped<ILoadService<CameraViewModel>, LoadCameraService>();


            return mauiAppBuilder;
        }
    }
}
