using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;

namespace TaskManager.API.Services
{


    public class TaskService : ITaskService
    {
        private readonly TaskDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(TaskDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskDto>> GetAllAsync()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            return task == null ? null : _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            var task = _mapper.Map<TaskItem>(dto);

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<bool> UpdateAsync(int id, TaskDto dto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _mapper.Map(dto, task);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<PagedResult<TaskDto>> GetFilteredAsync(
     string? search,
     bool? isCompleted,
     int page,
     int pageSize,
     string? sortBy,
     bool desc)
        {
            var query = _context.Tasks.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.Title.Contains(search));

            if (isCompleted.HasValue)
                query = query.Where(t => t.IsCompleted == isCompleted.Value);

            var totalCount = await query.CountAsync();

            query = desc
                ? query.OrderByDescending(t => t.Title)
                : query.OrderBy(t => t.Title);

            var tasks = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<TaskDto>
            {
                Items = _mapper.Map<IEnumerable<TaskDto>>(tasks),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}