IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='WeekendDefinition')
	CREATE TABLE [dbo].[WeekendDefinition](
		[WeekendDayId] [int] IDENTITY(1,1) Primary Key NOT NULL,
		[WeekendDayName] [nvarchar](100) NULL,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [int] NULL
		)
GO

