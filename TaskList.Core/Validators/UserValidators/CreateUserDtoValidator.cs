using FluentValidation;
using TaskList.Core.Dtos.UserDtos;

namespace TaskList.Core.Validators.UserValidators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .Length(1, 255)
            .WithMessage("First name length must be from 1 up to 255 characters");

        RuleFor(u => u.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .Length(1, 255)
            .WithMessage("Last name length must be from 1 up to 255 characters");
    }
}
