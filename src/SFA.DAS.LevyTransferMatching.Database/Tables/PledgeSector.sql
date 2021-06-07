CREATE TABLE [dbo].[PledgeSector] (
    [PledgeSectorId] INT     IDENTITY (1, 1) NOT NULL,
    [SectorId]       TINYINT NOT NULL,
    [PledgeId]       INT     NOT NULL,
    CONSTRAINT [PK_PledgeSector] PRIMARY KEY CLUSTERED ([PledgeSectorId] ASC),
    CONSTRAINT [FK_PledgeSector__Pledge] FOREIGN KEY ([PledgeId]) REFERENCES [dbo].[Pledge] ([PledgeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_PledgeSector__PledgeId]
    ON [dbo].[PledgeSector]([PledgeId] ASC);

