using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using PlannerApp.Services;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Data;

namespace PlannerApp
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
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            // --- INIZIO CONFIGURAZIONE EF CORE E SERVIZI ---

            // 1. Configurazione di Entity Framework Core per SQLite
            // Questo registra il tuo PlannerDbContext per la Dependency Injection.
            // Il percorso del database viene definito qui per l'applicazione in esecuzione.
            builder.Services.AddDbContext<PlannerDbContext>(options =>
            {
                // FileSystem.AppDataDirectory è un percorso cross-platform sicuro per i dati dell'app.
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "planner.db");
                options.UseSqlite($"Filename={dbPath}");
            });

            // 2. Registrazione del tuo TaskPlannerService
            // Registra l'interfaccia ITaskPlannerService con la sua implementazione TaskPlannerService.
            // AddSingleton è una buona scelta per i servizi che interagiscono con un DbContext locale
            // in un'app mobile, garantendo una singola istanza per tutta la durata dell'applicazione.
            builder.Services.AddSingleton<ITaskPlannerService, TaskPlannerService>();

            // --- FINE CONFIGURAZIONE EF CORE E SERVIZI ---


            return builder.Build();
        }
    }
}
