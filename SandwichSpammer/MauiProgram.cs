using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Plugin.Maui.Audio;
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

        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddTransient<MainPage>();
        builder.AddAudio();

        // Enregistrer le service DB avant les ViewModels pour éviter les dépendances nulles
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<AccompanimentViewModel>();
        builder.Services.AddSingleton<BoutiqueViewModel>();
        builder.Services.AddSingleton<DetailViewModel>();

        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<Boutique>();
        builder.Services.AddSingleton<Reset>();
        builder.Services.AddSingleton<DetailPage>();

        /*
         Régler problème avec premeirs boutons réinitialiser pour qu'il ne réinitialise pas les clickbonus optenus par les accompagnement
         */
        return builder.Build();
    }
}