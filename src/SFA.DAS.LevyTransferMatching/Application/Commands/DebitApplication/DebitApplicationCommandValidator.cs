namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;

public class DebitApplicationCommandValidator : AbstractValidator<DebitApplicationCommand>
{
    public DebitApplicationCommandValidator()
    {
        RuleFor(x => x.ApplicationId).GreaterThan(0);
        RuleFor(x => x.NumberOfApprentices).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}