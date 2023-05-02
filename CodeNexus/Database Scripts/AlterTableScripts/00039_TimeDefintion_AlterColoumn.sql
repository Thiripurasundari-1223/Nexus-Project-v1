USE [PMSNexus_Attendance]

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='TimeDefinition')
BEGIN
	ALTER TABLE [dbo].[TimeDefinition] DROP COLUMN [AbsentFromOperatorId],[AbsentToOperatorId],
	[HalfaDayFromOperatorId],[HalfaDayToOperatorId],[PresentOperatorId]
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='TimeDefinition' AND COLUMN_NAME = 'AbsentFromOperator' AND COLUMN_NAME = 'AbsentToOperator' 
AND COLUMN_NAME = 'HalfaDayFromOperator' AND COLUMN_NAME = 'HalfaDayToOperator' AND COLUMN_NAME = 'PresentOperator')
BEGIN
	ALTER TABLE [dbo].[TimeDefinition] ADD AbsentFromOperator [nvarchar](20) NULL
	ALTER TABLE [dbo].[TimeDefinition] ADD AbsentToOperator [nvarchar](20) NULL
	ALTER TABLE [dbo].[TimeDefinition] ADD HalfaDayFromOperator [nvarchar](20) NULL
	ALTER TABLE [dbo].[TimeDefinition] ADD HalfaDayToOperator [nvarchar](20) NULL
	ALTER TABLE [dbo].[TimeDefinition] ADD PresentOperator [nvarchar](20) NULL
END
GO