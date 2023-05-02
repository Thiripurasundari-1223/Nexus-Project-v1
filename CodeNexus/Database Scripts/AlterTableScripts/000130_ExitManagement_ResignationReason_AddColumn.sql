USE [PMSNexus_ExitManagement]
IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationReason' AND COLUMN_NAME = 'IsInvoluntary')
BEGIN
  ALTER TABLE ResignationReason Add [IsInvoluntary]  bit NULL default(0)
END

GO
Update ResignationReason SET [IsInvoluntary] = 0 WHERE ISNULL([IsInvoluntary],0) = 0
GO