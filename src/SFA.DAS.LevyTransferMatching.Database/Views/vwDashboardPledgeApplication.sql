CREATE VIEW [dbo].[vwDashboardPledgeApplication]
AS 
	SELECT  
        EmployerAccountId,
        PledgeId,
        NumberOfApprentices,
        StandardId,
        StartDate,
        Amount,
        HasTrainingProvider,
        PostCode,
        CreatedOn
        FROM Application
