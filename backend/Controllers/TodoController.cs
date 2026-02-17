using Microsoft.AspNetCore.Mvc;
using TodoApi.Application.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private readonly TodoService _service;

        public TodoController(TodoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoRequest req)
            => Ok(await _service.CreateAsync(req.Text));

        [HttpPut("{id}")]
        public async Task<IActionResult> Toggle(Guid id)
        {
            await _service.ToggleAsync(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }

    public record CreateTodoRequest(string Text);
}
