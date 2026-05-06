using TaskManager.API.Models;
using AutoMapper;
using TaskManager.API.DTOs;



namespace TaskManager.API.Mappings
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskItem, TaskDto>();
            CreateMap<CreateTaskDto, TaskItem>();
            CreateMap<TaskDto, TaskItem>();
        }
    }
}
