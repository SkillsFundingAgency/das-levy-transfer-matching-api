﻿CREATE TABLE [dbo].[PledgeLocation]
(
	[Id] INT IDENTITY NOT NULL, 
    [PledgeId] INT NOT NULL, 
    [Name] VARCHAR(MAX) NOT NULL, 
    [Latitude] FLOAT NOT NULL, 
    [Longitude] FLOAT NOT NULL,
    [LocalAuthorityName] VARCHAR(256) NULL, 
    [LocalAuthorityDistrict] VARCHAR(256) NULL, 
    [County] VARCHAR(256) NULL, 
    [Region] VARCHAR(256) NULL, 
    CONSTRAINT [FK_PledgeLocation_ToTable] FOREIGN KEY ([PledgeId]) REFERENCES [Pledge]([Id]), 
    CONSTRAINT [PK_PledgeLocation] PRIMARY KEY ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_PledgeLocation_PledgeId]
    ON [dbo].[PledgeLocation]([PledgeId] ASC);

