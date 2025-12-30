using CRUD_APP.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CRUD_APP.Models;

namespace CRUD_APP;

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

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory,"users.db");
            options.UseSqlite($"Data Source ={dbPath}");
            
        });


#if DEBUG
        builder.Logging.AddDebug();
#endif
        var app= builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();

            if(!context.Users.Any())
            {
                context.Users.AddRange( new User{Name="ahmad" },new User{Name="Omar" } );
                context.SaveChanges();
            }
        }


            return app;
            
    }
}
