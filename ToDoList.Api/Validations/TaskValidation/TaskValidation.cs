using FluentValidation;
using ToDoList.Api.Models.Task;

namespace ToDoList.Api.Validations.TaskValidation
{
    public class TaskValidation : AbstractValidator<TaskForCreationDto>
    {
        public TaskValidation()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Task Name Is Required")
                .MaximumLength(300);

            RuleFor(x => x.TaskStatus)
                .NotNull()
                .WithMessage("Task Status Is Required");

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .LessThan(x => x.ExpireDate)
                .WithMessage("Start Time Should Be Less Than Edn Time");

            RuleFor(x => x.PriorityLevel)
                .NotEmpty();



        }
    }
}
