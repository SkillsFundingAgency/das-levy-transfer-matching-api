using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication
{
    public class CreateApplicationCommand : IRequest<CreateApplicationCommandResult>
    {
        public long EmployerAccountId { get; set; }
        public int PledgeId { get; set; }
        public long ReceiverEmployerAccountId { get; set; }
    }
}
