using FluentValidation;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication
{
    public class RejectApplicationCommandValidator : AbstractValidator<RejectApplicationCommand>
    {
        public RejectApplicationCommandValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
            RuleFor(x => x.PledgeId).NotEmpty();
            RuleFor(x => x.UserId).NotNull();
            RuleFor(x => x.UserDisplayName).NotNull();
        }
    }
}
