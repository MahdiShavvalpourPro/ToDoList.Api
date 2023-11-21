using FluentValidation;
using ToDoList.Api.Models.People;

namespace ToDoList.Api.Validations.PeopleValidation
{
    public class PeopleForCreationDtoValidation : AbstractValidator<PeopleForCreationDto>
    {
        public PeopleForCreationDtoValidation()
        {
            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .WithMessage("MobileNumber Is Requierd")
                .Matches("^(\\+98|0)?9\\d{9}$")
                .WithMessage("Format MobileNumber Is Not Valid");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Firstname Is Required")
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Lastname Is Required")
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email Is Optional")
                .EmailAddress()
                .WithMessage("Email Format Not Valid");


        }
    }
}
