using TodoApi.Application.Interfaces;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Services
{
    public class TodoService
    {
        private readonly ITodoRepository _repo;

        public TodoService(ITodoRepository repo)
        {
            _repo = repo;
        }

        public Task<List<TodoItem>> GetAllAsync() => _repo.GetAllAsync();

        public async Task<TodoItem> CreateAsync(string text)
        {
            var todo = new TodoItem
            {
                Id = Guid.NewGuid(),
                Text = text,
                Done = false
            };

            return await _repo.AddAsync(todo);
        }

        public async Task ToggleAsync(Guid id)
        {
            var todo = await _repo.GetByIdAsync(id);
            if (todo == null) throw new Exception("Todo not found");

            todo.Done = !todo.Done;
            await _repo.UpdateAsync(todo);
        }

        public Task DeleteAsync(Guid id) => _repo.DeleteAsync(id);
    }
}
