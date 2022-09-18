namespace SimpleGantt.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        MauiAppBuilder unused2 = builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                IFontCollection unused1 = fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                IFontCollection unused = fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        return builder.Build();
    }
}