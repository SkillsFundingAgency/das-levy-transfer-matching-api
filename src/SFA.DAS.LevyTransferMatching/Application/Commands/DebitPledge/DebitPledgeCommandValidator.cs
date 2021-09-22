using FluentValidation;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge
{
    public class DebitPledgeCommandValidator : AbstractValidator<DebitPledgeCommand>
    {
        public DebitPledgeCommandValidator()
        {
            RuleFor(x => x.PledgeId).GreaterThan(0);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}