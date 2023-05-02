USE [PMSNexus_ExitManagement]
IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationInterview' AND COLUMN_NAME = 'FeelAboutManagement')
BEGIN
  ALTER TABLE ResignationInterview Add [FeelAboutManagement]  VARCHAR(MAX) NULL
END

GO

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationInterview' AND COLUMN_NAME = 'FeedbackOnPerformance')
BEGIN
  ALTER TABLE ResignationInterview Add [FeedbackOnPerformance]  VARCHAR(MAX) NULL
END

GO

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationInterview' AND COLUMN_NAME = 'SufficientSupportTraining')
BEGIN
  ALTER TABLE ResignationInterview Add [SufficientSupportTraining]  VARCHAR(MAX) NULL
END

GO

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationInterview' AND COLUMN_NAME = 'PreventFromLeaving')
BEGIN
  ALTER TABLE ResignationInterview Add [PreventFromLeaving]  VARCHAR(MAX) NULL
END

GO
IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationInterview' AND COLUMN_NAME = 'RejoinORRecommend')
BEGIN
  ALTER TABLE ResignationInterview Add [RejoinORRecommend]  VARCHAR(MAX) NULL
END

GO

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResignationInterview' AND COLUMN_NAME = 'Suggestion')
BEGIN
  ALTER TABLE ResignationInterview Add [Suggestion]  VARCHAR(MAX) NULL
END

GO