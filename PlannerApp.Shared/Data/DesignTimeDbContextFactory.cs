// File: PlannerApp.Shared/Data/PlannerDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using PlannerApp.Shared.Models;

namespace PlannerApp.Shared.Data 
{
    // Questa factory dice a Entity Framework Core come creare un'istanza del tuo DbContext
    // quando esegui i comandi di migrazione (Add-Migration, Update-Database).
    public class PlannerDbContextFactory : IDesignTimeDbContextFactory<PlannerDbContext>
    {
        public PlannerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PlannerDbContext>();

            // Per la fase di design-time (migrazioni), usiamo un percorso semplice per SQLite.
            // Il database 'planner_design_time.db' verrà creato nella cartella da cui esegui il comando dotnet ef.
            // Questo DB è solo per le migrazioni, non è il DB che l'app userà in runtime.
            string designTimeDbPath = Path.Combine(Directory.GetCurrentDirectory(), "planner_design_time.db");

            optionsBuilder.UseSqlite($"Filename={designTimeDbPath}");

            return new PlannerDbContext(optionsBuilder.Options);
        }
    }
}
