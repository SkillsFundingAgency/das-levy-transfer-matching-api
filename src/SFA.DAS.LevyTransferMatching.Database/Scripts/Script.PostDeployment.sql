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


--TM-260 - Pledge Ledger backfill

--Create opening balance entries
insert into PledgeLedger (PledgeId, ApplicationId, UserAction, ActionDate, Amount, Balance)
select Id, null, 'CreatePledge', CreatedOn, Amount, Amount
from Pledge p

--Create ApproveApplication entries (without ApplicationId)
insert into PledgeLedger (PledgeId, ApplicationId, UserAction, ActionDate, Amount, Balance)
select
p.Id,
null,
'ApproveApplication', a.AuditDate,
-(CONVERT(INT,JSON_VALUE(a.UpdatedState,'$.Amount')) - CONVERT(INT,JSON_VALUE(a.UpdatedState,'$.RemainingAmount'))),
CONVERT(INT,JSON_VALUE(a.UpdatedState,'$.RemainingAmount'))
from Audit a
join Pledge p on p.Id = a.EntityId
where a.UserAction = 'ApproveApplication' and a.EntityType = 'Pledge'

--Create DeclineFunding entries (without ApplicationId)
insert into PledgeLedger (PledgeId, ApplicationId, UserAction, ActionDate, Amount, Balance)
select
p.Id,
null,
'DeclineFunding', a.AuditDate,
(CONVERT(INT,JSON_VALUE(a.InitialState,'$.Amount')) - CONVERT(INT,JSON_VALUE(a.InitialState,'$.RemainingAmount'))),
CONVERT(INT,JSON_VALUE(a.UpdatedState,'$.RemainingAmount'))
from Audit a
join Pledge p on p.Id = a.EntityId
where a.UserAction = 'DeclineFunding' and a.EntityType = 'Pledge'
