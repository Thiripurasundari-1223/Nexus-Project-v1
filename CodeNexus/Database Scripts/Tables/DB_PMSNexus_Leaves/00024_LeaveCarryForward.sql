USE [PMSNexus_Leaves]
GO

/****** Object:  Table [dbo].[LeaveCarryForward]    Script Date: 10/04/2021 17:47:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LeaveCarryForward](
	[LeaveCarryForwardID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[LeaveTypeID] [int] NOT NULL,
	[BalanceLeave] [decimal](18, 2) NULL,
	[AdjustmentBalanceLeave] [decimal](18, 2) NULL,
	[AdjustmentEffectiveFromDate] [datetime] NULL,
	[CarryForwardLeaves] [decimal](18, 2) NULL,
	[ReimbursementLeaves] [decimal](18, 2) NULL,
	[AdjustmentDays] [decimal](18, 2) NULL,
	[ResetDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_LeaveCarryForward] PRIMARY KEY CLUSTERED 
(
	[LeaveCarryForwardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


