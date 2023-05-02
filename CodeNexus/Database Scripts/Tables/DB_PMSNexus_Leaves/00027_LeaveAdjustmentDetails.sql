USE [PMSNexus_Leaves]
GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='LeaveAdjustmentDetails')
	CREATE TABLE [dbo].[LeaveAdjustmentDetails](
		[LeaveAdjustmentDetailsId] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeId] [int] NULL,
		[LeavetypeId] [int] NULL,
		[EffectiveFromDate] [datetime] NULL,			
		[PreviousBalance] DECIMAL(18, 2) NULL,		
		[AdjustmentBalance] DECIMAL(18, 2) Null,
		[NoOfDays] DECIMAL(18, 2) Null,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,		
	PRIMARY KEY CLUSTERED 
	(
		[LeaveAdjustmentDetailsId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO