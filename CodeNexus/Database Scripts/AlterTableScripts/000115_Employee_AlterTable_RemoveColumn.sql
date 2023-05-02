
USE [PMSNexus_Employees]
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees')
BEGIN
		ALTER TABLE [dbo].[Employees] drop column Extension
		ALTER TABLE [dbo].[Employees] drop column Qualification
		ALTER TABLE [dbo].[Employees] drop column PersonalMobileNumber
		ALTER TABLE [dbo].[Employees] drop column OtherEmail
		ALTER TABLE [dbo].[Employees] drop column BloodGroup
		ALTER TABLE [dbo].[Employees] drop column PermanentAddress
		ALTER TABLE [dbo].[Employees] drop column FathersName
		ALTER TABLE [dbo].[Employees] drop column EmergencyMobileNumber
		ALTER TABLE [dbo].[Employees] drop column Nationality
		ALTER TABLE [dbo].[Employees] drop column CommunicationAddress
		ALTER TABLE [dbo].[Employees] drop column PANNumber
		ALTER TABLE [dbo].[Employees] drop column UANNumber
		ALTER TABLE [dbo].[Employees] drop column DrivingLicense
		ALTER TABLE [dbo].[Employees] drop column AadhaarCardNumber
		ALTER TABLE [dbo].[Employees] drop column PassportNumber
END
GO