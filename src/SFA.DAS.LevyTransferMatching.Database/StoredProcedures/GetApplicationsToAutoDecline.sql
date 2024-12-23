CREATE PROCEDURE [dbo].[GetApplicationsToAutoDecline]
AS

SELECT DISTINCT
    app.*
FROM 
    [dbo].[Audit] adt
INNER JOIN 
    dbo.Application app ON app.Id = adt.EntityId
WHERE 
    adt.EntityType = 'Application'
    AND adt.UserAction = 'ApproveApplication'
    AND app.[Status] = 1
    AND adt.AuditDate < DATEADD(WEEK, -6, GETDATE());