CREATE TABLE [dbo].[ApplicationLocation]
(
	[Id] INT NOT NULL IDENTITY,
	[ApplicationId] INT NOT NULL,
	[PledgeLocationId] INT NOT NULL,
	CONSTRAINT [PK_ApplicationLocation] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_ApplicationLocation_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [Application]([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_ApplicationLocation_ApplicationId]
    ON [dbo].[ApplicationLocation]([ApplicationId] ASC);
