﻿CREATE TABLE [dbo].[Application]
(
	[Id]					INT				IDENTITY (1, 1) NOT NULL,
    [EmployerAccountId]		BIGINT			NOT NULL,
	[PledgeId]				INT				NOT NULL,
	[Details]				NVARCHAR(MAX)	NOT NULL,
	[NumberOfApprentices]	INT				NOT NULL,
	[StandardId]			VARCHAR(20)		NOT NULL,
	[StandardTitle]			VARCHAR(500)	DEFAULT '' NOT NULL,
	[StandardLevel]			INT				DEFAULT 0  NOT NULL,
	[StandardDuration]		INT				DEFAULT 0 NOT NULL,
	[StandardMaxFunding]	INT				DEFAULT 0 NOT NULL,
	[StandardRoute]			VARCHAR(500)	DEFAULT '' NOT NULL,
	[StartDate]				DATETIME2		NOT NULL,
	[Amount]				INT				NOT NULL,
	[TotalAmount]			INT				DEFAULT 0 NOT NULL,
	[HasTrainingProvider]	BIT				NOT NULL,
	[Sectors]				INT		        NOT NULL,
	[PostCode]				VARCHAR(8)		NULL,
	[AdditionalLocation]	NVARCHAR(MAX)	NULL,
	[SpecificLocation]		NVARCHAR(MAX)	NULL,
	[FirstName]				NVARCHAR(25)	NOT NULL,
	[LastName]				NVARCHAR(25)	NOT NULL,
	[BusinessWebsite]		NVARCHAR(75)	NOT NULL,
	[CreatedOn]				DATETIME2 (7)	CONSTRAINT [DF_Application__CreationDate] DEFAULT (getdate()) NOT NULL,
	[Status]				TINYINT			CONSTRAINT [DF_Application__Status] DEFAULT (0)  NOT NULL,
	[NumberOfApprenticesUsed] INT			DEFAULT 0 NOT NULL,
	[AmountUsed]			INT				DEFAULT 0 NOT NULL,
	[UpdatedOn]				DATETIME2		NULL,
	[RowVersion]			TIMESTAMP		NOT NULL,
    [AutomaticApproval]		BIT				NOT NULL DEFAULT 0, 
	[MatchSector]			BIT				NOT NULL DEFAULT 0,
	[MatchLevel]			BIT				NOT NULL DEFAULT 0,
	[MatchLocation]			BIT				NOT NULL DEFAULT 0,
	[MatchJobRole]			BIT				NOT NULL DEFAULT 0,
	[MatchPercentage]		TINYINT			NOT NULL DEFAULT 255,
	[CostingModel]			TINYINT			NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Application_EmployerAccount] FOREIGN KEY ([EmployerAccountId]) REFERENCES [dbo].[EmployerAccount] ([Id]),
	CONSTRAINT [FK_Application_Pledge] FOREIGN KEY ([PledgeId]) REFERENCES [dbo].[Pledge] ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_Application_PledgeId]
    ON [dbo].[Application]([PledgeId] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Application_EmployerAccountId] ON [dbo].[Application]
(
	[EmployerAccountId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Application_CreatedOnIdEmployerAccountId] ON [dbo].[Application]
(
	[CreatedOn] DESC,
	[Id] ASC,
	[EmployerAccountId] ASC
)