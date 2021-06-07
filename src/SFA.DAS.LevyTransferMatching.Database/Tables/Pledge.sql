CREATE TABLE [dbo].[Pledge] (
    [PledgeId]          INT           IDENTITY (1, 1) NOT NULL,
    [EncodedId]         VARCHAR (100) NOT NULL,
    [EmployerAccountId] BIGINT        NOT NULL,
    [Amount]            DECIMAL (18)  NOT NULL,
    [HideEmployerName]  BIT           NOT NULL,
    [CreationDate]      DATETIME2 (7) CONSTRAINT [DF_Pledge__CreationDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Pledge] PRIMARY KEY CLUSTERED ([PledgeId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Pledge_EmployerAccountId]
    ON [dbo].[Pledge]([EmployerAccountId] ASC);

