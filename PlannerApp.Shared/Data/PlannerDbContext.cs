using Microsoft.EntityFrameworkCore;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Shared.Data
{
    public class PlannerDbContext : DbContext
    {
        public PlannerDbContext(DbContextOptions<PlannerDbContext> options) : base(options)
        {
        }

        public DbSet<TaskPlanner> Tasks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Per le migrations, usa un percorso semplice
                optionsBuilder.UseSqlite("Data Source=planner_temp.db");
            }
        }
    }
}
