CREATE TABLE [dbo].[Pledge] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [EmployerAccountId] BIGINT        NOT NULL,
    [Amount]            INT           NOT NULL,
    [IsNamePublic]      BIT           NOT NULL,
    [CreatedOn]         DATETIME2 (7) CONSTRAINT [DF_Pledge__CreationDate] DEFAULT (getdate()) NOT NULL,
    [JobRoles]          INT           NOT NULL,
    [Levels]            INT           NOT NULL,
    [Sectors]           INT           NOT NULL,
    [RowVersion]        TIMESTAMP     NOT NULL,
    CONSTRAINT [PK_Pledge] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Pledge_EmployerAccount] FOREIGN KEY ([EmployerAccountId]) REFERENCES [dbo].[EmployerAccount] ([Id])
);

GO
CREATE NONCLUSTERED INDEX [IX_Pledge_EmployerAccountId]
    ON [dbo].[Pledge]([EmployerAccountId] ASC);