IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='TimeDefinition')
	CREATE TABLE [dbo].[TimeDefinition](
		[TimeDefinitionId] [int] IDENTITY(1,1) NOT NULL,
		[TimeFrom] [Time](0) NULL,
		[TimeTo] [Time](0) NULL,
		[BreakTime] [Time](0) NULL,
		[TotalHours] [Time](0) NULL,			
		[AbsentFromHour] [Time](0) NULL,
		[AbsentFromOperator] [nvarchar](20) NULL,
		[AbsentToHour] [Time](0) NULL,
		[AbsentToOperator] [nvarchar](20) NULL,
		[HalfaDayFromHour] [Time](0) NULL,
		[HalfaDayFromOperator] [nvarchar](20) NULL,
		[HalfaDayToHour] [Time](0) NULL,
		[HalfaDayToOperator] [nvarchar](20) NULL,
		[PresentHour] [Time](0) NULL,
		[PresentOperator] [nvarchar](20) Null,
		[ShiftDetailsId] [int] NULL,
		[IsConsiderAbsent] [bit] NULL,
		[IsConsiderPresent] [bit] NULL,
		[IsConsiderHalfaDay] [bit] NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[TimeDefinitionId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO


