USE [PMSNexus_Notifications]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='SupportingDocuments' AND COLUMN_NAME = 'IsApproved')
BEGIN
	ALTER TABLE [dbo].[SupportingDocuments] ADD IsApproved BIT 
END
GO

UPDATE [SupportingDocuments] SET IsApproved = 0 WHERE IsApproved IS NULL

GO