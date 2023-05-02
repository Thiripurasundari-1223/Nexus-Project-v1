IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='ShiftWeekendDefinition')
	CREATE TABLE [dbo].[ShiftWeekendDefinition](
		[ShiftWeekendDefinitionId] [int] IDENTITY(1,1) NOT NULL,
		[WeekendDayId] [int] Null,
		[ShiftDetailsId] [int] NULL,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedOn] [datetime] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
		(
			[ShiftWeekendDefinitionId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	GO


	