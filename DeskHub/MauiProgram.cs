using Microsoft.Extensions.Logging;

namespace DeskHub
{
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
                    fonts.AddFont("jost_bold.ttf", "FontTest");
                    fonts.AddFont("jost_medium.ttf", "FontTestTwo");
                    fonts.AddFont("nunito_bold.ttf", "Primary");
                    fonts.AddFont("nunito_medium.ttf", "Secondary");
                    fonts.AddFont("nunito_light.ttf", "Third");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}