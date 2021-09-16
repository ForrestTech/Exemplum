namespace Exemplum.Application.Todo.Models
{
    using AutoMapper;
    using Common.Mapping;
    using Domain.Todo;

    public class TodoItemDto : IMapFrom<TodoItem>
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public bool Done { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TodoItem, TodoItemDto>()
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(source => source.Priority != null ? 
                    source.Priority.ToString() : 
                    PriorityLevel.None));
        }
    }
}