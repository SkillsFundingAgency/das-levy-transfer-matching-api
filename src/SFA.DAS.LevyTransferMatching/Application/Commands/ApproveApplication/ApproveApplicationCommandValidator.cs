namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;

public class ApproveApplicationCommandValidator : AbstractValidator<ApproveApplicationCommand>
{
    public ApproveApplicationCommandValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty();
        RuleFor(x => x.PledgeId).NotEmpty();
        RuleFor(x => x.UserId).NotNull();
        RuleFor(x => x.UserDisplayName).NotNull();
    }
}