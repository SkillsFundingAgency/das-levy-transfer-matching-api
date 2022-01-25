namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public enum UserAction
    {
        CreatePledge,
        CreateApplication,
        ApproveApplication,
        UndoApplicationApproval,
        AcceptFunding,
        DebitApplication,
        RejectApplication,
        DeclineFunding,
        WithdrawApplication
    }
}