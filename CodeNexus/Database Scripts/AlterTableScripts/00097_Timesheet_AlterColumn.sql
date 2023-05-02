USE [PMSNexus_Timesheet]

Go
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Timesheet' AND COLUMN_NAME = 'TotalRequiredHours')
BEGIN
	ALTER TABLE [dbo].[Timesheet] ADD TotalRequiredHours [nvarchar](15) NULL
END
GO
