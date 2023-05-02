IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='LeaveDetails')
	CREATE TABLE [dbo].[LeaveDetails](
		[LeaveDetailsId] [int] IDENTITY(1,1) NOT NULL,
		[LeaveType] [nvarchar](250) NULL,
		[LeaveCode] [nvarchar](100) NULL,
		[LeaveAccruedMonthlyId] [int] NULL,
		[LeaveAccruedLastDayId] [int] NULL,
		[LeaveAccruedNoOfDays] [int] NULL,
		[LeaveDescription] [nvarchar](max) NULL,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedOn] [datetime] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
		(
			[LeaveDetailsId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	GO	