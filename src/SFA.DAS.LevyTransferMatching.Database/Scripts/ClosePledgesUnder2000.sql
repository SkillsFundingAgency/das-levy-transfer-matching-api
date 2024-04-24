SET NOCOUNT ON;

PRINT 'Close Active LTM Pledges with <= £2000 remaining';

--1. GET PLEDGES

DROP TABLE IF EXISTS #PledgeID;

SELECT ID
INTO #PledgeID
FROM [dbo].[Pledge]
WHERE RemainingAmount <= 2000
	AND Status = 0;

--2. PRE-RUN AUDIT  -- TAKE BACKUP OF DATA

SELECT * FROM [dbo].[Pledge]
WHERE ID IN (SELECT ID FROM #PledgeID);

--3. UPDATE SCRIPT. Number of affected rows should match row count of #PledgeID.

BEGIN TRAN
BEGIN TRY
    UPDATE Pledge
    SET [STATUS] = 1, --CLOSED
        ClosedOn = SYSDATETIME()
    WHERE ID IN (SELECT ID FROM #PledgeID);
    
    COMMIT TRAN;
    SELECT * FROM [dbo].[Pledge]
    WHERE ID IN (SELECT ID FROM #PledgeID);
END TRY
BEGIN CATCH
	PRINT Error_message();
	PRINT 'Pledges under £2,000 not closed, see error.';
	PRINT 'Rolling back transaction';
	
	ROLLBACK TRAN;

	THROW;

END CATCH;

DROP TABLE IF EXISTS #PledgeID;