using FluentValidation;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding
{
    public class AcceptFundingCommandValidator : AbstractValidator<AcceptFundingCommand>
    {
        public AcceptFundingCommandValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserDisplayName).NotEmpty();
        }
    }
}
