USE [PMSNexus_ExitManagement]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationApprovalStatus' AND COLUMN_NAME = 'ApproverType')
BEGIN
    
   ALTER TABLE ResignationApprovalStatus Add ApproverType nvarchar(250);
END