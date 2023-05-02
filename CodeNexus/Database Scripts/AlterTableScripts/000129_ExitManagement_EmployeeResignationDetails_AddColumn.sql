USE [PMSNexus_ExitManagement]
IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'ResignationType')
BEGIN
  ALTER TABLE EmployeeResignationDetails Add [ResignationType]  VARCHAR(100) NULL
END

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'Remarks')
BEGIN
  ALTER TABLE EmployeeResignationDetails Add [Remarks]  VARCHAR(500) NULL
END

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'ProfilePicture')
BEGIN
  ALTER TABLE EmployeeResignationDetails Add [ProfilePicture]  NVARCHAR(MAX) NULL
END

