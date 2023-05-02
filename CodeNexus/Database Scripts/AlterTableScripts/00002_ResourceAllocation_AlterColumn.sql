USE [PMSNexus_Projects]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResourceAllocation' AND COLUMN_NAME = 'IsBillable')
BEGIN
	ALTER TABLE [dbo].[ResourceAllocation] ADD IsBillable bit 
END
GO
UPDATE [dbo].[ResourceAllocation] SET IsBillable = 1 WHERE IsBillable IS NULL
GO