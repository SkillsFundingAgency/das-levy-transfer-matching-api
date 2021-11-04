/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

/* DashbordReporting Role Access */

IF DATABASE_PRINCIPAL_ID('DashbordReporting') IS NULL
BEGIN
    CREATE ROLE [DashbordReporting]
END

GRANT SELECT ON vwDashboardPledge TO DashbordReporting
GRANT SELECT ON vwDashboardPledgeApplication TO DashbordReporting




/* TM-203 - Close a specific pledge */
update Pledge set Status=1 where Id = 51 and Status=0

