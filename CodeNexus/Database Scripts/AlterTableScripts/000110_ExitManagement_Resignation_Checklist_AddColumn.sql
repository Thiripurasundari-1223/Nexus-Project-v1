USE [PMSNexus_ExitManagement]
IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationInterview' AND COLUMN_NAME = 'ResignationDetailsId')
BEGIN
  ALTER TABLE ResignationInterview Add [ResignationDetailsId] [int] NULL
END