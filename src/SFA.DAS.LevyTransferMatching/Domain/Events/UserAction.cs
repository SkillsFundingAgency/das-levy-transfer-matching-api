namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public enum UserAction
    {
        CreatePledge,
        CreateApplication,
        ApproveApplication,
        DebitPledge,
        UndoApplicationApproval,
        RejectApplication
    }
}