using Microsoft.EntityFrameworkCore;
using TaskManager.API.DTOs;

namespace TaskManager.API.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllAsync();
        Task<TaskDto?> GetByIdAsync(int id);
        Task<TaskDto> CreateAsync(CreateTaskDto dto);
        Task<bool> UpdateAsync(int id, TaskDto dto);
        Task<bool> DeleteAsync(int id);

        Task<PagedResult<TaskDto>> GetFilteredAsync(
     string? search,
     bool? isCompleted,
     int page,
     int pageSize,
     string? sortBy,
     bool desc);

    }
}
