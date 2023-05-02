USE [PMSNexus_ExitManagement]


IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'StateId')
BEGIN
  ALTER TABLE EmployeeResignationDetails drop column [StateId]
END
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'CountryId')
BEGIN
  ALTER TABLE EmployeeResignationDetails drop column [CountryId]
END
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'DepartmentID')
BEGIN
  ALTER TABLE EmployeeResignationDetails drop column [DepartmentID]
END
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'LocationId')
BEGIN
  ALTER TABLE EmployeeResignationDetails drop column [LocationId]
END


IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'EmployeeName')
BEGIN
  ALTER TABLE EmployeeResignationDetails Add [EmployeeName] VARCHAR(1000) NULL
END	

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'FormattedEmployeeId')
BEGIN
  ALTER TABLE EmployeeResignationDetails Add [FormattedEmployeeId] VARCHAR(100) NULL
END	

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'ReportingManager')
BEGIN
  ALTER TABLE EmployeeResignationDetails Add [ReportingManager] VARCHAR(1000) NULL
END	


IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'State')
BEGIN
  ALTER TABLE EmployeeResignationDetails Add [State] VARCHAR(100) NULL
END	

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'Country')
BEGIN
  ALTER TABLE EmployeeResignationDetails Add [Country] VARCHAR(100) NULL
END	

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'Department')
BEGIN
  ALTER TABLE EmployeeResignationDetails Add [Department] VARCHAR(100) NULL
END	

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'Location')
BEGIN
  ALTER TABLE EmployeeResignationDetails Add [Location] VARCHAR(100) NULL
END	
