using MudBlazor;

namespace PlannerApp.Components.Pages
{
    public partial class NewEvent
    {
        private MudForm? _form;

        private async Task SubmitForm()
        {
            // La validazione è già avvenuta, avverrà in un secondo step dentro il ViewModel.
            bool success = await NewEventViewModel.SaveEventAsync();

            if (success)
            {
                Snackbar.Add("Evento creato con successo!", Severity.Success);
                NavManager.NavigateTo("/dashboard");
            }
            else
            {
                Snackbar.Add("Errore durante il salvataggio del task. Controlla i campi.", Severity.Error);
            }
        }

        private void Cancel()
        {
            NewEventViewModel.ResetForm();
            NavManager.NavigateTo("/dashboard");
        }
    }
}