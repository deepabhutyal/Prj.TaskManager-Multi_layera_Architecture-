using Microsoft.EntityFrameworkCore;
using Prj.TaskManager.Data;
using Prj.TaskManager.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prj.TaskManager.Sevice
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _Context;
        public TaskService(AppDbContext appDbContext)
        {
            _Context = appDbContext;
        }
        public async Task<TaskItemModel> Create(TaskItemModel model)
        {
            _Context.Tasks.Add(model);
            await _Context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> Delete(int id)
        {
            var task=await GetTask(id);
            _Context.Tasks.Remove(task);
            return true;
        }

        public async Task<IEnumerable<TaskItemModel>> GetAllTasks()
        {
           return  await _Context.Tasks.ToListAsync();

        }

        public async Task<TaskItemModel> GetTask(int id)
        {
            return await _Context.Tasks.FindAsync(id);

        }

        public async Task<TaskItemModel> Update(TaskItemModel model)
        {
            _Context.Tasks.Update(model);
            await _Context.SaveChangesAsync();
            return model;

        }
    }
}
