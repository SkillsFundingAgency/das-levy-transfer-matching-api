CREATE TABLE [dbo].[PledgeLocation]
(
	[Id] INT IDENTITY NOT NULL, 
    [PledgeId] INT NOT NULL, 
    [Name] VARCHAR(MAX) NOT NULL, 
    [Latitude] FLOAT NOT NULL, 
    [Longitude] FLOAT NOT NULL,
    CONSTRAINT [FK_PledgeLocation_ToTable] FOREIGN KEY ([PledgeId]) REFERENCES [Pledge]([Id]), 
    CONSTRAINT [PK_PledgeLocation] PRIMARY KEY ([Id])
)
