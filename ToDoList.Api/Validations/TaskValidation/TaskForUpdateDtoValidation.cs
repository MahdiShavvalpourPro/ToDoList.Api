using FluentValidation;
using ToDoList.Api.Models.Task;

namespace ToDoList.Api.Validations.TaskValidation
{
    public class TaskForUpdateDtoValidation : AbstractValidator<TaskForUpdateDto>
    {
        public TaskForUpdateDtoValidation()
        {
            RuleFor(task => task.Name)
                .NotEmpty()
                .WithMessage("Name Is Requierd")
                .MaximumLength(200)
                .WithMessage("Name Shuold Be Less Than 200 Character");

            RuleFor(task => task.Description)
                .MaximumLength(300)
                .WithMessage("Description Shuold Be Less Than 300 Character");

            RuleFor(task => task.TaskStatus)
                .IsInEnum();

            RuleFor(task => task.PriorityLevel)
                .IsInEnum();

            RuleFor(task => task.StartTime)
                .NotEmpty()
                .WithMessage("the start time task should be less than Date Time Of Expired");

            RuleFor(task => task.ExpireTime)
                .NotEmpty()
                .WithMessage("The expiration date should not be same as start date");


        }
    }
}
