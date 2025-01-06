CREATE PROCEDURE [dbo].[GetApplicationsToAutoExpire]
	@implementationDate datetime
AS
BEGIN	
	SET NOCOUNT ON;
SELECT 
    app.*
FROM 
    [dbo].[Audit] adt
INNER JOIN 
    dbo.Application app ON app.Id = adt.EntityId
WHERE 
    adt.EntityType = 'Application'
    AND adt.UserAction = 'AcceptFunding'
    AND app.[Status] = 3 
    AND adt.AuditDate < DATEADD(MONTH, -3, GETDATE())
	AND adt.AuditDate> @implementationDate;
END
GO