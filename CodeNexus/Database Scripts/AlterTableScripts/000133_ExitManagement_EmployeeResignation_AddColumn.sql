USE [PMSNexus_ExitManagement]
IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'WithdrawalSubmmitedDate')
BEGIN
  ALTER TABLE EmployeeResignationDetails Add [WithdrawalSubmmitedDate]  datetime NULL
END
