using FluentValidation;
using ToDoList.Api.Models.Task;

namespace ToDoList.Api.Validations.TaskValidation
{
    public class TaskForCreationDtoValidation : AbstractValidator<TaskForCreationDto>
    {
        public TaskForCreationDtoValidation()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Task Name Is Required")
                .MaximumLength(300);

            RuleFor(x => x.TaskStatus)
                .IsInEnum();

            RuleFor(x => x.PriorityLevel)
                .IsInEnum();

            RuleFor(x => x.StartTime)
               .NotEmpty();

            RuleFor(x => x.ExpireTime)
                .NotEmpty()
                .GreaterThan(x => x.StartTime);

            RuleFor(task => task.Description)
               .MaximumLength(300)
               .WithMessage("Description Shuold Be Less Than 300 Character");
        }
    }
}
