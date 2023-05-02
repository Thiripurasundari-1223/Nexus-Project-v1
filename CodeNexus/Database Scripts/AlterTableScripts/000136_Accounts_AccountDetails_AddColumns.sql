USE [PMSNexus_Accounts]

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AccountDetails' AND COLUMN_NAME = 'AccountManagerName')
BEGIN
  ALTER TABLE AccountDetails Add AccountManagerName [nvarchar](500) NULL
END
