using Microsoft.Extensions.Logging;
using GamingApp.Repositories;
namespace GamingApp
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
                });
            string dbPath = FileAccessHelper.GetLocalFilePath("GabrielCalderon.db3");
            builder.Services.AddSingleton<UserRepository>(s => ActivatorUtilities.CreateInstance<UserRepository>(s, dbPath));
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            return System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);
        }
    }
}
