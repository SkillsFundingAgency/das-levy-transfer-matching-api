CREATE TABLE [dbo].[ApplicationEmailAddress]
(
	[Id] INT IDENTITY NOT NULL,
	[ApplicationId] INT NOT NULL,
	[EmailAddress] NVARCHAR(50) NOT NULL,
	CONSTRAINT [FK_ApplicationId_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [Application]([Id]), 
    CONSTRAINT [PK_ApplicationEmailAddress] PRIMARY KEY ([Id])
)
