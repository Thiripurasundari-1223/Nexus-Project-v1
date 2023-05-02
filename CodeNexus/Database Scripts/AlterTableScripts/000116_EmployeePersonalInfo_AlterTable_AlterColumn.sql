USE [PMSNexus_Employees]
GO
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeesPersonalInfo' AND COLUMN_NAME = 'HighestQualificationId')
BEGIN
    ALTER TABLE [dbo].[EmployeesPersonalInfo] drop column HighestQualificationId
	ALTER TABLE EmployeesPersonalInfo ADD  HighestQualification varchar(2000)
END
GO
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeesPersonalInfo' AND COLUMN_NAME = 'PermanentState')
BEGIN
	ALTER TABLE EmployeesPersonalInfo ALTER COLUMN PermanentState int Null
END
GO
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeesPersonalInfo' AND COLUMN_NAME = 'PermanentCountry')
BEGIN
	ALTER TABLE EmployeesPersonalInfo ALTER COLUMN PermanentCountry int Null
END
GO
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeesPersonalInfo' AND COLUMN_NAME = 'CommunicationState')
BEGIN
	ALTER TABLE EmployeesPersonalInfo ALTER COLUMN CommunicationState int Null
END
GO
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeesPersonalInfo' AND COLUMN_NAME = 'CommunicationCountry')
BEGIN
	ALTER TABLE EmployeesPersonalInfo ALTER COLUMN CommunicationCountry int Null
END
GO