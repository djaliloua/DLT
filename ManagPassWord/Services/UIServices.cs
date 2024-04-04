using ManagPassWord.Pages;
using ManagPassWord.Pages.Debt;
using ManagPassWord.ViewModels.Debt;
using ManagPassWord.Views;

namespace ManagPassWord.Services
{
    public static class UIServices
    {
        public static MauiAppBuilder RegisterUIPages(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<SettingPage>();
            mauiAppBuilder.Services.AddTransient<SearchPage>();
            mauiAppBuilder.Services.AddSingleton<AddPassworPage>();
            mauiAppBuilder.Services.AddSingleton<DetailPage>();
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            mauiAppBuilder.Services.AddSingleton<DebtPage>();
            mauiAppBuilder.Services.AddSingleton<DebtDetailsPage>();
            mauiAppBuilder.Services.AddSingleton<AboutPage>();
            mauiAppBuilder.Services.AddSingleton<DebtSettingView>();
            mauiAppBuilder.Services.AddSingleton<PasswordSettingView>();
            mauiAppBuilder.Services.AddSingleton<DebtSettingPage>();
            mauiAppBuilder.Services.AddSingleton<DebtFormPage>();
            return mauiAppBuilder;
        }
    }
}
