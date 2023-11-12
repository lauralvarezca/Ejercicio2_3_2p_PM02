﻿using CommunityToolkit.Maui;
using Ejer2_3.Views;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;

namespace Ejer2_3
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<Lista>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}