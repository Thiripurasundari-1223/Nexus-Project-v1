IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='LeaveEntitlement')
	CREATE TABLE [dbo].[LeaveEntitlement](
		[LeaveEntitlementId] [int] IDENTITY(1,1) NOT NULL,
		[MaxLeaveAvailedYearId] [int] NULL,
		[MaxLeaveAvailedDays] [int] NULL,
		[CarryForwardId] [int] NULL,
	    [MaximumCarryForwardDays] [decimal](18, 2) NULL,
	    [ReimbursementId] [int] NULL,
	    [MaximumReimbursementDays] [decimal](18, 2) NULL,
	    [ResetYear] [int] NULL,
	    [ResetMonth] [int] NULL,
	    [ResetDay] [varchar](100) NULL,
		[LeaveTypeId] [int] Null,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedOn] [datetime] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
		(
			[LeaveEntitlementId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	GO