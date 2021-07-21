CREATE TABLE [dbo].[Application]
(
	[Id]                INT           IDENTITY (1, 1) NOT NULL,
    [EmployerAccountId] BIGINT        NOT NULL,
	[PledgeId]			INT			  NOT NULL,
	[CreatedOn]         DATETIME2 (7) CONSTRAINT [DF_Application__CreationDate] DEFAULT (getdate()) NOT NULL,
	[RowVersion]        TIMESTAMP     NOT NULL,
    CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Application_EmployerAccount] FOREIGN KEY ([EmployerAccountId]) REFERENCES [dbo].[EmployerAccount] ([Id]),
	CONSTRAINT [FK_Application_Pledge] FOREIGN KEY ([PledgeId]) REFERENCES [dbo].[Pledge] ([Id])
)

