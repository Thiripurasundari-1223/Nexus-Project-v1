IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EmployeeShiftDetails')
	CREATE TABLE [dbo].[EmployeeShiftDetails] (
		[EmployeeShiftDetailsId] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeID] [int] NULL,
		[ShiftDetailsId] [int] NULL,
		[ShiftFromDate] [datetime] NULL,
		[ShiftToDate] [datetime] NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[EmployeeShiftDetailsId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO