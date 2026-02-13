using FluentValidation;
using TaskList.Core.Dtos.TaskListDtos;

namespace TaskList.Core.Validators.TaskListValidators;

public class UpdateTaskListDtoValidator : AbstractValidator<UpdateTaskListDto>
{
    public UpdateTaskListDtoValidator()
    {
        RuleFor(utl => utl.NewName)
            .NotEmpty()
            .WithMessage("Name is required")
            .Length(1, 255)
            .WithMessage("Name length must be from 1 up to 255 characters");
    }
}
