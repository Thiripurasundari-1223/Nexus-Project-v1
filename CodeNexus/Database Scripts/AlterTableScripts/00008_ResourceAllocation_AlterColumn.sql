USE [PMSNexus_Projects]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResourceAllocation' AND COLUMN_NAME = 'Experience')
BEGIN
	ALTER TABLE [dbo].[ResourceAllocation] ADD [Experience] [decimal](18, 2) 
END
GO