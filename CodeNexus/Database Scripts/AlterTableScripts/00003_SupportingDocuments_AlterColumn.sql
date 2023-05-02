USE [PMSNexus_Notifications]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='SupportingDocuments' AND COLUMN_NAME = 'DocumentCategory')
BEGIN
	ALTER TABLE [dbo].[SupportingDocuments] ADD DocumentCategory varchar(100) 
END
GO