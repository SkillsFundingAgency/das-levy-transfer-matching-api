CREATE TABLE [dbo].[ApplicationStatusHistory]
(
	[Id] INT IDENTITY NOT NULL,
	[ApplicationId] INT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL,
	[Status] TINYINT NOT NULL,
	CONSTRAINT [FK_ApplicationStatusHistory_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [Application]([Id]), 
    CONSTRAINT [PK_ApplicationStatusHistory] PRIMARY KEY ([Id])
)