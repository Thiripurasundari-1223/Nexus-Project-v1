
USE [PMSNexus_Employees]
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees' AND COLUMN_NAME='NoticePeriod')
BEGIN
		ALTER TABLE [dbo].[Employees] drop column [NoticePeriod]
END
IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees' AND COLUMN_NAME='NoticeCategory')
BEGIN
		ALTER TABLE [dbo].[Employees] add [NoticeCategory] [nvarchar](20)
END