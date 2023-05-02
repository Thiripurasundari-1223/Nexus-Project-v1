USE [PMSNexus_Employees]
GO
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees' AND COLUMN_NAME = 'TVSNextExperience')
BEGIN
    ALTER TABLE [dbo].[Employees] drop column TVSNextExperience
END
GO
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees' AND COLUMN_NAME = 'TotalExperience')
BEGIN
    ALTER TABLE [dbo].[Employees] drop column TotalExperience
END

