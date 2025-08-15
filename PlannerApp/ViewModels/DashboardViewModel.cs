using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly ITaskPlannerService _taskService;
        public DashboardViewModel(ITaskPlannerService taskService)
        {
            _taskService = taskService;
        }

#region OnPropertyCHnged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

#endregion

#region Proprietà

        private ObservableCollection<TaskPlanner> _tasks = new();
        public ObservableCollection<TaskPlanner> Tasks
        {
            get => _tasks;
            set
            {
                if (_tasks != value)
                {
                    _tasks = value;
                    OnPropertyChanged(nameof(Tasks));
                    OnPropertyChanged(nameof(TotalTasksCount));
                    OnPropertyChanged(nameof(TodayTasks));
                    OnPropertyChanged(nameof(UpcomingTasks));
                }
            }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }
#endregion

#region Proprietà calcolate
        public int TotalTasksCount => Tasks.Count;
        public List<TaskPlanner> TodayTasks => Tasks.Where(t => t.EndDate.Date == DateTime.Today.Date && !t.IsCompleted).ToList();
        public List<TaskPlanner> UpcomingTasks => Tasks.Where(t => t.EndDate.Date > DateTime.Today.Date && !t.IsCompleted).ToList();
        public List<TaskPlanner> CompletedTasks => Tasks.Where(t => t.IsCompleted).ToList();

        #endregion

        #region Metodi
        /// <summary>
        /// Carico tutte le attività del Planner.
        /// </summary>
        /// <returns>Ritorna la lista dei Task.</returns>
        public async Task LoadPlannerTasksAsync()
        {
            IsLoading = true;
            try
            {
                var loadedTasks = await _taskService.GetTasksAsync();

                //Ad ogni Add notifico alla UI che la collezione è cambiata
                Tasks.Clear();
                foreach (var task in loadedTasks)
                {
                    Tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Imposta lo stato di completamento di una specifica attività e aggiorna la lista delle attività.
        /// </summary>
        /// <param name="taskId">Identificativo dell'attività da aggiornare.</param>
        /// <param name="isCompleted">Valore booleano che indica se l'attività è completata.</param>
        /// <returns>Task asincrono che rappresenta l'operazione di aggiornamento.</returns>
        public async Task SetTaskCompletedAsync(int taskId, bool isCompleted)
        {
            await _taskService.SetTaskCompletedAsync(taskId, isCompleted);
            await LoadPlannerTasksAsync(); // aggiorno la lista dopo aver modificato lo stato
        }
#endregion
    }
}
