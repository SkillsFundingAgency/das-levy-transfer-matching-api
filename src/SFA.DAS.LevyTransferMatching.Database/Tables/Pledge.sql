CREATE TABLE [dbo].[Pledge] (
    [PledgeId]          INT           IDENTITY (1, 1) NOT NULL,
    [EmployerAccountId] BIGINT        NOT NULL,
    [Amount]            INT           NOT NULL,
    [IsNamePublic]      BIT           NOT NULL,
    [CreationDate]      DATETIME2 (7) CONSTRAINT [DF_Pledge__CreationDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Pledge] PRIMARY KEY CLUSTERED ([PledgeId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Pledge_EmployerAccountId]
    ON [dbo].[Pledge]([EmployerAccountId] ASC);

