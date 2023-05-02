USE [PMSNexus_ExitManagement]
IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationFeedbackToManagement' AND COLUMN_NAME = 'OrgPoliciesId')
BEGIN
  ALTER TABLE ResignationFeedbackToManagement Add [OrgPoliciesId]  int NULL
END

GO

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationFeedbackToManagement' AND COLUMN_NAME = 'OrgPoliciesRemark')
BEGIN
  ALTER TABLE ResignationFeedbackToManagement Add [OrgPoliciesRemark]  VARCHAR(MAX) NULL
END

GO