using Microsoft.Extensions.Logging;
using SandwichSpammer.Database;
using SandwichSpammer.ViewModels;

namespace SandwichSpammer;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddSingleton<AccompanimentViewModel>();
        builder.Services.AddSingleton<BoutiqueViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<Boutique>();
        builder.Services.AddSingleton<Reset>();
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<AppShell>();
        return builder.Build();
    }
}