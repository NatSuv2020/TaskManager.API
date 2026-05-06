using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Services;


namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {

      
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service = service;
        }
        // GET
        /* [HttpGet]
         public async Task<ActionResult<IEnumerable<TaskItem>>> Get()
         {
             return Ok(await _context.Tasks.ToListAsync());
         }
        */
        /* [HttpGet]
         public ActionResult<IEnumerable<TaskDto>> Get()
         {
             var tasks = _context.Tasks
                 .Select(t => new TaskDto
                 {
                     Id = t.Id,
                     Title = t.Title,
                     IsCompleted = t.IsCompleted
                 })
                 .ToList();

             return Ok(tasks);
         }
        */
        [HttpGet]
        public async Task<IActionResult> Get(
    string? search,
    bool? isCompleted,
    int page = 1,
    int pageSize = 5,
    string? sortBy = "id",
    bool desc = false)
        {
            var result = await _service.GetFilteredAsync(search, isCompleted, page, pageSize, sortBy, desc);
            return Ok(result);
        }
       
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var task = await _service.GetByIdAsync(id);
            return task == null ? NotFound() : Ok(task);
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
       
        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered(
    string? search,
    bool? isCompleted,
    int page = 1,
    int pageSize = 10,
    string? sortBy = null,
    bool desc = false)
        {
            var result = await _service.GetFilteredAsync(
                search,
                isCompleted,
                page,
                pageSize,
                sortBy,
                desc);

            return Ok(result);
        }
    }
    }

