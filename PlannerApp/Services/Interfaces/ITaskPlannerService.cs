using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Services.Interfaces
{
    /// <summary>
    /// Interfaccia per la gestione delle attività del planner.
    /// Presenti metodi per recuperare, aggiungere, aggiornare, eliminare e completare attività.
    /// </summary>
    public interface ITaskPlannerService
    {
        /// <summary>
        /// Ritorna la lista di tutte le attività.
        /// </summary>
        /// <returns>Lista di modelli TaskPlanner.</returns>
        Task<List<TaskPlanner>> GetTasksAsync();

        /// <summary>
        /// Ritorna una singola attività tramite il suo identificativo.
        /// </summary>
        /// <param name="id">Identificativo dell'attività.</param>
        /// <returns>Oggetto TaskPlanner corrispondente all'id.</returns>
        Task<TaskPlanner> GetTaskByIdAsync(int id);

        /// <summary>
        /// Inserisce una nuova attività.
        /// </summary>
        /// <param name="task">Oggetto TaskPlanner da aggiungere.</param>
        Task AddTaskAsync(TaskPlanner task);

        /// <summary>
        /// Aggiorna un'attività esistente sul DB.
        /// </summary>
        /// <param name="task">Oggetto TaskPlanner aggiornato.</param>
        Task UpdateTaskAsync(TaskPlanner task);

        /// <summary>
        /// Elimina un'attività tramite il suo ID.
        /// </summary>
        /// <param name="id">Identificativo dell'attività da eliminare.</param>
        Task DeleteTaskAsync(int id);

        /// <summary>
        /// Imposta lo stato di un Task..
        /// </summary>
        /// <param name="id">ID dell'attività.</param>
        /// <param name="isCompleted">True se l'attività è completata, false altrimenti.</param>
        Task SetTaskCompletedAsync(int id, bool isCompleted);
    }
}
