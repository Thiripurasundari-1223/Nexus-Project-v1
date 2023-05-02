USE [PMSNexus_ExitManagement]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationChecklist' AND COLUMN_NAME = 'ResignationDetailsId')
BEGIN
   ALTER TABLE ResignationChecklist Add [ResignationDetailsId] [int] NULL
END