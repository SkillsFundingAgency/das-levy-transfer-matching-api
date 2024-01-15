using FluentValidation;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplication;

public class WithdrawApplicationCommandValidator : AbstractValidator<WithdrawApplicationCommand>
{
    public WithdrawApplicationCommandValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty();
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.UserDisplayName).NotEmpty();
    }
}