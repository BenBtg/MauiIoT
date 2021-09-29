using Microsoft.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using ZXing.Net.Maui;
using Microsoft.Maui.Essentials;
using MauiIoT.Services;

#if ANDROID
[assembly: Android.App.UsesPermission(Android.Manifest.Permission.Camera)]
#endif

namespace MauiIoT
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()

                .ConfigureLifecycleEvents(life =>
                {
#if __ANDROID__
                    global::Microsoft.Maui.Essentials.Platform.Init(MauiApplication.Current);

                    life.AddAndroid(android => android
                        .OnRequestPermissionsResult((activity, requestCode, permissions, grantResults) =>
                        {
                            global::Microsoft.Maui.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                        })
                        .OnNewIntent((activity, intent) =>
                        {
                            global::Microsoft.Maui.Essentials.Platform.OnNewIntent(intent);
                        })
                        .OnResume((activity) =>
                        {
                            global::Microsoft.Maui.Essentials.Platform.OnResume();
                        }));
#elif __IOS__
                    //life.AddiOS(ios => ios
                    //	.ContinueUserActivity((application, userActivity, completionHandler) =>
                    //	{
                    //		return Platform.ContinueUserActivity(application, userActivity, completionHandler);
                    //	})
                    //	.OpenUrl((application, url, options) =>
                    //	{
                    //		return Platform.OpenUrl(application, url, options);
                    //	})
                    //	.PerformActionForShortcutItem((application, shortcutItem, completionHandler) =>
                    //	{
                    //		Platform.PerformActionForShortcutItem(application, shortcutItem, completionHandler);
                    //	}));
#elif WINDOWS
				//life.AddWindows(windows => windows
				//	.OnLaunched((application, args) =>
				//	{
				//		Platform.OnLaunched(args);
				//	}));
#endif
                })


                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<IDeviceConfigService, DeviceConfigService>();
            builder.Services.AddSingleton<IIoTDeviceClientService, IoTDeviceClientService>();

            return builder.Build();
        }
    }
}