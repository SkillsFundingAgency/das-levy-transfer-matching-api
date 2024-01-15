namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount;

public class CreateAccountCommand : IRequest<CreateAccountCommandResult>
{
    public long AccountId { get; set; }
    public string AccountName { get; set; }
}