USE [PMSNexus_ExitManagement]
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AppConstants' AND COLUMN_NAME = 'DisplayName')
BEGIN
   ALTER TABLE AppConstants   
   alter COLUMN DisplayName nvarchar(max);
END