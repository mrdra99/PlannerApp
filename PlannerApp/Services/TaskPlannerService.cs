using Microsoft.EntityFrameworkCore;
using PlannerApp.Services.Interfaces;
using PlannerApp.Shared.Data;
using PlannerApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Services
{
    public class TaskPlannerService : ITaskPlannerService
    {
        private readonly PlannerDbContext _dbContext;

        public TaskPlannerService(PlannerDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddTaskAsync(TaskPlanner task)
        {
            _dbContext.Tasks.Add(task); 
            await _dbContext.SaveChangesAsync(); 
        }

        public async Task DeleteTaskAsync(int id)
        {
            var taskToDelete = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (taskToDelete != null)
            {
                _dbContext.Tasks.Remove(taskToDelete); // Rimuove il task dal DbSet
                await _dbContext.SaveChangesAsync(); // Salva le modifiche
            }
        }
        public async Task<TaskPlanner?> GetTaskByIdAsync(int id)
        {
            return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<TaskPlanner>> GetTasksAsync()
        {
            return await _dbContext.Tasks.ToListAsync();
        }

        public async Task SetTaskCompletedAsync(int id, bool isCompleted)
        {
            var taskToUpdate = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (taskToUpdate != null)
            {
                taskToUpdate.IsCompleted = isCompleted; // Modifica la proprietà
                await _dbContext.SaveChangesAsync(); // Salva le modifiche
            }
        }
        public async Task UpdateTaskAsync(TaskPlanner task)
        {
            // EF Core traccia automaticamente le modifiche se l'entità è già stata recuperata dal contesto.
            // Se l'entità non è tracciata (es. arriva dalla UI), puoi usare _dbContext.Tasks.Update(task);
            // In questo caso, stiamo assumendo che l'entità 'task' sia un'entità 'disconnessa'
            // (cioè, non è stata recuperata dal contesto corrente ma creata o modificata altrove).
            // Per assicurare che EF Core la tracci come modificata:
            _dbContext.Entry(task).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
