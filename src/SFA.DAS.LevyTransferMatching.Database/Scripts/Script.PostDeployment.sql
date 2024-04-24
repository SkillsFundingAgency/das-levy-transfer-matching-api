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


--CON-5105: Update existing applications to switch to new costing model
--This script can be safely re-executed but can be removed at any time following deployment
update Application set [CostingModel] = 1 where [Status] = 0 and [CostingModel] = 0;

:r .\ClosePledgesUnder2000.sql
