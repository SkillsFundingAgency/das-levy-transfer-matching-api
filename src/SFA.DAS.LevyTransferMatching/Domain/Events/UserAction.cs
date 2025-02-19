﻿namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public enum UserAction
{
    CreatePledge,
    CreateApplication,
    ApproveApplication,
    DebitPledge,
    UndoApplicationApproval,
    AcceptFunding,
    DebitApplication,
    RejectApplication,
    DeclineFunding,
    CreditPledge,
    WithdrawApplication,
    ClosePledge,
    WithdrawApplicationAfterAcceptance,
    ExpireAcceptedFunding
}