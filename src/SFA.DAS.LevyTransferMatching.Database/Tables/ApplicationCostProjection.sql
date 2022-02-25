CREATE TABLE [dbo].[ApplicationCostProjection]
(
	[Id] INT NOT NULL IDENTITY,
	[ApplicationId] INT NOT NULL,
	[FinancialYear] VARCHAR(7) NOT NULL,
	[Amount] INT NOT NULL,
	CONSTRAINT [PK_ApplicationCostProjection] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_ApplicationCostProjection_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [Application]([Id]),
	CONSTRAINT [IX_ApplicationCostProjection_FinancialYear] UNIQUE ([ApplicationId],[FinancialYear])
)
GO

CREATE NONCLUSTERED INDEX [IX_ApplicationCostProjection_ApplicationId]
    ON [dbo].[ApplicationCostProjection]([ApplicationId] ASC);
