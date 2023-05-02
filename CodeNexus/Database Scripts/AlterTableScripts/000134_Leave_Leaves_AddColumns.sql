USE [PMSNexus_Leaves]

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Leaves' AND COLUMN_NAME = 'ApproveRejectBy' AND COLUMN_NAME = 'ApproveRejectOn')
BEGIN
  ALTER TABLE Leaves Add [ApproveRejectBy]  int NULL
  ALTER TABLE Leaves Add [ApproveRejectOn]  datetime NULL
  ALTER TABLE Leaves Add [ApproveRejectName]  nvarchar(250) NULL
END
