using FluentValidation;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge;

public class CreditPledgeCommandValidator : AbstractValidator<CreditPledgeCommand>
{
    public CreditPledgeCommandValidator()
    {
        RuleFor(x => x.PledgeId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}