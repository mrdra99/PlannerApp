namespace PlannerApp.Components.Pages
{
    public partial class Dashboard
    {
        protected override async Task OnInitializedAsync()
        {
            await dashboardVm.LoadPlannerTasksAsync();
        }

        private void NavigateToNewPage()
        {
            NavManager.NavigateTo("/newEvent");
        }
    }
}