using FluentValidation;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

public class CreateApplicationCommandValidator : AbstractValidator<CreateApplicationCommand>
{
    public CreateApplicationCommandValidator()
    {
        RuleFor(x => x.EmployerAccountId).NotEmpty();
        RuleFor(x => x.PledgeId).NotEmpty();
    }
}