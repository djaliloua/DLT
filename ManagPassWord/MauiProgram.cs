using Microsoft.Extensions.Logging;
using UraniumUI;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using CommunityToolkit.Maui;

namespace ManagPassWord;
public static class MauiProgram
{


    // Register View mode
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseUraniumUI()
            .UseMauiCommunityToolkit()
            .RegisterViewModel()
            .UseUraniumUIMaterial()
            .UseMauiApp<App>()
            .ConfigureLifecycleEvents(events =>
            {
#if ANDROID
events.AddAndroid(android => android
                        //.OnActivityResult((activity, requestCode, resultCode, data) => LogEvent(nameof(AndroidLifecycle.OnActivityResult), requestCode.ToString()))
                        //.OnStart((activity) => LogEvent(nameof(AndroidLifecycle.OnStart)))
                        .OnCreate((activity, bundle) => {
                        
                        })
                        //.OnBackPressed((activity) => LogEvent(nameof(AndroidLifecycle.OnBackPressed)) && false)
                        .OnStop((activity) => {
                        //MainViewModel.Save();
                        
                        }));
#endif
            })
            .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            fonts.AddMaterialIconFonts();
            //fonts.AddFontAwesomeIconFonts();

        });
#if DEBUG
        builder.Logging.AddDebug();
#endif
        
        builder.Services.AddSingleton(typeof(IFingerprint), CrossFingerprint.Current);
       
        return builder.Build();
    }
}