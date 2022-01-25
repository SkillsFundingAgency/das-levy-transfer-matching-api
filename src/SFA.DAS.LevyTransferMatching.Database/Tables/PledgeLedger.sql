CREATE TABLE [dbo].[PledgeLedger]
(
	[Id]            INT IDENTITY NOT NULL, 
    [PledgeId]      INT NOT NULL, 
    [ApplicationId] INT NULL,
    [UserAction]    NVARCHAR(256),
    [ActionDate]    DATETIME2 NOT NULL,
    [Amount]        INT NOT NULL,
    [Balance]       INT NOT NULL,
    CONSTRAINT [PK_PledgeLedger] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PledgeLedger_PledgeId] FOREIGN KEY ([PledgeId]) REFERENCES [Pledge]([Id]),
    CONSTRAINT [FK_PledgeLedger_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [Application]([Id])
)
