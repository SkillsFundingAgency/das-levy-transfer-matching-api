using FluentValidation;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeCommandValidator : AbstractValidator<CreatePledgeCommand>
    {
        public CreatePledgeCommandValidator()
        {
            RuleFor(x => x.AccountId).GreaterThan(0);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}