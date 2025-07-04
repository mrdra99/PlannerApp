// File: PlannerApp/ViewModels/NewEventViewModel.cs
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.ViewModels
{
    public class NewEventViewModel : INotifyPropertyChanged
    {
        private readonly ITaskPlannerService _taskService;

        // Costruttore: inietti il servizio
        public NewEventViewModel(ITaskPlannerService taskService)
        {
            _taskService = taskService;
            ValidationErrors = new List<string>();
            // Inizializza le proprietà a valori di default.
            // Le proprietà nullable possono essere inizializzate a null o a un valore non-null.
            Title = string.Empty;
            Description = null;
            StartDate = DateTime.Today; // Inizializza a non-null per comodità
            EndDate = DateTime.Today.AddDays(1); // Inizializza a non-null per comodità
            StartTime = TimeSpan.FromHours(9); // Inizializza a non-null per comodità
            EndTime = TimeSpan.FromHours(10); // Inizializza a non-null per comodità
            Category = "Generale";
            PriorityLevel = PriorityLevel.Medium;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Proprietà Getter and Setter

        private bool _isSaving = false;
        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                if (_isSaving != value)
                {
                    _isSaving = value;
                    OnPropertyChanged(nameof(IsSaving));
                }
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                    OnPropertyChanged(nameof(CanSaveTask));
                }
            }
        }

        private string? _description;
        public string? Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                    OnPropertyChanged(nameof(CanSaveTask));
                }
            }
        }

        // --- PROPRIETÀ DATE E ORE NULLABLE ---
        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                    OnPropertyChanged(nameof(CanSaveTask));
                }
            }
        }

        private TimeSpan? _startTime;
        public TimeSpan? StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged(nameof(StartTime));
                    OnPropertyChanged(nameof(CanSaveTask));
                }
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                    OnPropertyChanged(nameof(CanSaveTask));
                }
            }
        }

        private TimeSpan? _endTime;
        public TimeSpan? EndTime
        {
            get => _endTime;
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    OnPropertyChanged(nameof(EndTime));
                    OnPropertyChanged(nameof(CanSaveTask));
                }
            }
        }
        // --- FINE PROPRIETÀ NULLABLE ---

        private string _category;
        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                    OnPropertyChanged(nameof(CanSaveTask));
                }
            }
        }

        private PriorityLevel _priorityLevel;
        public PriorityLevel PriorityLevel
        {
            get => _priorityLevel;
            set
            {
                if (_priorityLevel != value)
                {
                    _priorityLevel = value;
                    OnPropertyChanged(nameof(PriorityLevel));
                }
            }
        }

        private List<string> _validationErrors = new List<string>();
        public List<string> ValidationErrors
        {
            get => _validationErrors;
            set
            {
                if (_validationErrors != value)
                {
                    _validationErrors = value;
                    OnPropertyChanged(nameof(ValidationErrors));
                }
            }
        }

        public bool CanSaveTask => ValidateForm(false);

        #endregion

        #region Metodi

        private bool ValidateForm(bool setErrors = true)
        {
            if (setErrors)
            {
                ValidationErrors = new List<string>();
            }

            bool isValid = true;

            if (string.IsNullOrWhiteSpace(Title))
            {
                if (setErrors) ValidationErrors.Add("Il titolo è obbligatorio.");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(Description))
            {
                if (setErrors) ValidationErrors.Add("La descrizione è obbligatoria.");
                isValid = false;
            }

            // --- GESTIONE E VALIDAZIONE TIPI NULLABLE ---
            if (!StartDate.HasValue || !StartTime.HasValue || !EndDate.HasValue || !EndTime.HasValue)
            {
                if (setErrors) ValidationErrors.Add("Tutti i campi data e ora sono obbligatori.");
                isValid = false;
            }
            else
            {
                DateTime startDateTime = StartDate.Value.Date + StartTime.Value;
                DateTime endDateTime = EndDate.Value.Date + EndTime.Value;

                if (startDateTime > endDateTime)
                {
                    if (setErrors) ValidationErrors.Add("La data/ora di inizio non può essere successiva alla data/ora di fine.");
                    isValid = false;
                }

                if (endDateTime < DateTime.Now)
                {
                    if (setErrors) ValidationErrors.Add("La data e l'orario di fine non possono essere nel passato.");
                    isValid = false;
                }
            }
            // --- FINE GESTIONE NULLABLE ---

            if (string.IsNullOrWhiteSpace(Category))
            {
                if (setErrors) ValidationErrors.Add("La categoria è obbligatoria.");
                isValid = false;
            }

            if (setErrors)
            {
                OnPropertyChanged(nameof(ValidationErrors));
            }

            return isValid;
        }

        public async Task<bool> SaveEventAsync()
        {
            ValidationErrors = new List<string>();

            if (!ValidateForm(true))
            {
                return false;
            }

            IsSaving = true;
            try
            {
                // --- CREAZIONE TASKPLANNER CON TIPI NULLABLE ---
                // Se la validazione prima garantisce che non siano null, puoi usare .Value
                var newTask = new TaskPlanner
                {
                    Title = Title,
                    Description = Description,
                    StartDate = StartDate.Value.Date + StartTime.Value,
                    EndDate = EndDate.Value.Date + EndTime.Value,
                    Category = Category,
                    Priority = PriorityLevel,
                    IsCompleted = false
                };
                // --- FINE CREAZIONE ---

                await _taskService.AddTaskAsync(newTask);

                ResetForm();

                return true;
            }
            catch (Exception ex)
            {
                ValidationErrors = new List<string> { $"Errore durante il salvataggio: {ex.Message}" };
                OnPropertyChanged(nameof(ValidationErrors));
                return false;
            }
            finally
            {
                IsSaving = false;
            }
        }

        public void ResetForm()
        {
            Title = string.Empty;
            Description = null;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(1);
            StartTime = TimeSpan.FromHours(9);
            EndTime = TimeSpan.FromHours(10);
            Category = "Generale";
            PriorityLevel = PriorityLevel.Medium;
            ValidationErrors = new List<string>();
        }

        #endregion
    }
}
