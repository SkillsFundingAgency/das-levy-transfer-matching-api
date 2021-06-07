CREATE TABLE [dbo].[PledgeLevel] (
    [PledgeLevelId] INT     IDENTITY (1, 1) NOT NULL,
    [LevelId]        TINYINT NOT NULL,
    [PledgeId]       INT     NOT NULL,
    CONSTRAINT [PK_PledgeLevel] PRIMARY KEY CLUSTERED ([PledgeLevelId] ASC),
    CONSTRAINT [FK_PledgeLevel__Pledge] FOREIGN KEY ([PledgeId]) REFERENCES [dbo].[Pledge] ([PledgeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_PledgeLevel__PledgeId]
    ON [dbo].[PledgeLevel]([PledgeId] ASC);

