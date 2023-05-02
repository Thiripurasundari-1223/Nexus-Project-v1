
USE [PMSNexus_Employees]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EducationDetail' AND COLUMN_NAME = 'UniversityName')
BEGIN
	ALTER TABLE [dbo].[EducationDetail] ADD [UniversityName] [varchar](200) NULL
END
GO
