using API.Data;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly OpenAiPriorityService _ai;

        public TasksController(AppDbContext db, OpenAiPriorityService ai)
        {
            _db = db;
            _ai = ai;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _db.Tasks
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return Ok(list.Select(TaskMapping.ToDto));
        }


        [HttpPost]
        public async Task<IActionResult> Create(TaskItem task)
        {
            task.Priority = await _ai.PredictPriorityAsync(
                task.Title,
                task.Description,
                task.DueDate
            );

            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();

            return Ok(task);
        }
    }

}
