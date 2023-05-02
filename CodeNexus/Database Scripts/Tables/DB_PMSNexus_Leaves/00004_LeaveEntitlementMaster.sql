IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='LeaveMaxLimitAction')
	CREATE TABLE [dbo].[LeaveMaxLimitAction](
		[LeaveMaxLimitActionId] [int] IDENTITY(1,1) NOT NULL,
		[LeaveMaxLimitActionName] [varchar](500) NULL,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedOn] [datetime] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
		(
			[LeaveMaxLimitActionId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	GO