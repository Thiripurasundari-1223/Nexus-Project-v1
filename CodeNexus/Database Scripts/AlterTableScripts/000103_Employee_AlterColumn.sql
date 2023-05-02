
USE [PMSNexus_Employees]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees' AND COLUMN_NAME = 'ExitType')
BEGIN
	ALTER TABLE [dbo].[Employees] ADD [ExitType] [varchar](100) NULL
	ALTER TABLE [dbo].[Employees] ADD [ResignationDate] [datetime] NULL
	ALTER TABLE [dbo].[Employees] ADD [ResignationReason] [nvarchar](max) NULL
	ALTER TABLE [dbo].[Employees] ADD [ResignationStatus] [nvarchar](250) NULL
END
GO
