USE [PMSNexus_ExitManagement]
IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'ActualRelievingDate')
BEGIN
  ALTER TABLE EmployeeResignationDetails Add [ActualRelievingDate] [datetime] NULL
END