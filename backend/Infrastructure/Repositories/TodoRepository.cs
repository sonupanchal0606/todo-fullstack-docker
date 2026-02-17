using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Interfaces;
using TodoApi.Domain.Entities;
using TodoApi.Infrastructure.Data;

namespace TodoApi.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _db;

        public TodoRepository(TodoDbContext db)
        {
            _db = db;
        }


        public Task<List<TodoItem>> GetAllAsync() =>
            _db.Todos.OrderByDescending(t => t.CreatedAt).ToListAsync();

        public Task<TodoItem?> GetByIdAsync(Guid id) =>
            _db.Todos.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<TodoItem> AddAsync(TodoItem todo)
        {
            _db.Todos.Add(todo);
            await _db.SaveChangesAsync();
            return todo;
        }

        public async Task UpdateAsync(TodoItem todo)
        {
            _db.Todos.Update(todo);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var todo = await _db.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (todo != null)
            {
                _db.Todos.Remove(todo);
                await _db.SaveChangesAsync();
            }
        }
    }
}
