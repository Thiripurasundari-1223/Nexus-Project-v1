USE [PMSNexus_Leaves]
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EmployeeLeaveDetails')
	CREATE TABLE [dbo].[EmployeeLeaveDetails](
		[EmployeeLeaveDetailsID] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeID] [int] NULL,
		[LeaveTypeID] [int] Null,
		[BalanceLeave] DECIMAL(18, 2) NULL,					
		[AdjustmentBalanceLeave] DECIMAL(18, 2) NULL,
		[AdjustmentEffectiveFromDate] [datetime] NULL,
		[AdjustmentDays] DECIMAL(18, 2) NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,		
	PRIMARY KEY CLUSTERED 
	(
		[EmployeeLeaveDetailsID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
