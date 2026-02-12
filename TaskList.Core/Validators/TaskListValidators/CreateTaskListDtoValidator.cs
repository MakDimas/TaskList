using FluentValidation;
using TaskList.Core.Dtos.TaskListDtos;

namespace TaskList.Core.Validators.TaskListValidators;

public class CreateTaskListDtoValidator : AbstractValidator<CreateTaskListDto>
{
    public CreateTaskListDtoValidator()
    {
        RuleFor(tl => tl.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .Length(1, 255)
            .WithMessage("Name length must be from 1 up to 255 characters");

        RuleFor(tl => tl.OwnerId)
            .NotEmpty()
            .WithMessage("Owner id is required");
    }
}
