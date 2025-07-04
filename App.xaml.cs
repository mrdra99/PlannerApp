using PlannerApp.Shared.Data; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using System.Threading.Tasks;

namespace PlannerApp
{
    public partial class App : Application
    {
        public App(PlannerDbContext dbContext)
        {
            InitializeComponent();
            Task.Run(() => dbContext.Database.Migrate());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "PlannerApp" };
        }
    }
}
