CREATE TABLE [dbo].[PledgeRole] (
    [PledgeRoleId] INT     IDENTITY (1, 1) NOT NULL,
    [RoleId]       TINYINT NOT NULL,
    [PledgeId]     INT     NOT NULL,
    CONSTRAINT [PK_PledgeRole] PRIMARY KEY CLUSTERED ([PledgeRoleId] ASC),
    CONSTRAINT [FK_PledgeRole__Pledge] FOREIGN KEY ([PledgeId]) REFERENCES [dbo].[Pledge] ([PledgeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_PledgeRole__PledgeId]
    ON [dbo].[PledgeRole]([PledgeId] ASC);

