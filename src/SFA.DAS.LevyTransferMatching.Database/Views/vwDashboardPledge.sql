CREATE VIEW [dbo].[vwDashboardPledge]
AS 
    SELECT  
        EmployerAccountId,
        Amount,
        RemainingAmount,
        IsNamePublic,
        CreatedOn,
        JobRoles,
        Levels,
        Sectors
        FROM Pledge
