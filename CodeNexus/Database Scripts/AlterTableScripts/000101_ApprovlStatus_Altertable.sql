USE [PMSNexus_ExitManagement]
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationApprovalStatus' AND COLUMN_NAME = 'ApprovalType')
BEGIN
    
   ALTER TABLE ResignationApprovalStatus Add ApprovalType nvarchar(100);
   ALTER TABLE ResignationApprovalStatus Add ApprovedBy int;
END