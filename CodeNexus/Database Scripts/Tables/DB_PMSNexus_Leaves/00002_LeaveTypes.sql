IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='LeaveTypes')
	CREATE TABLE [dbo].[LeaveTypes](
		[LeaveTypeId] [int] IDENTITY(1,1) NOT NULL,
		[LeaveType] [nvarchar](250) NULL,
		[LeaveCode] [nvarchar](100) NULL,
		[LeaveAccruedType] [int] NULL,
		[LeaveAccruedDay] [nvarchar](100) NULL,
		[LeaveAccruedNoOfDays] [decimal](18,2) NULL,
		[LeaveDescription] [nvarchar](max) NULL,
		[ProRate] [bit] NULL,
		[EffectiveFromDate] [datetime] NULL,
		[EffectiveToDate] [datetime] NULL,
		[BalanceBasedOn] [int] null,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedOn] [datetime] NULL,
		[ModifiedBy] [int] NULL,
		[IsActive] [bit] NULL,
		[AllowTimesheet] [bit] NULL,
	PRIMARY KEY CLUSTERED 
		(
			[LeaveTypeId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	GO