USE [PMSNexus_Leaves]
GO
/****** Object:  Table [dbo].[LeaveRestrictions]    Script Date: 11-10-2021 20:23:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeaveRestrictions](
	[LeaveRestrictionsId] [int] IDENTITY(1,1) NOT NULL,
	[ExceedLeaveBalance] [bit] NULL,
	[AllowUsersViewId] [int] NULL,
	[BalanceDisplayedId] [decimal](18, 2) NULL,
	[DaysInAdvance] [decimal](18, 2) NULL,
	[AllowRequestDates] [varchar](100) NULL,
	[AllowRequestNextDays] [decimal](18, 2) NULL,
	[DatesAppliedAdvance] [decimal](18, 2) NULL,
	[MaximumLeavePerApplication] [decimal](18, 2) NULL,
	[MinimumGapTwoApplication] [decimal](18, 2) NULL,
	[MaximumConsecutiveDays] [decimal](18, 2) NULL,
	[EnableFileUpload] [decimal](18, 2) NULL,
	[MinimumNoOfApplicationsPeriod] [decimal](18, 2) NULL,
	[AllowRequestPeriodId] [int] NULL,
	[MaximumLeave] [bit] NULL,
	[MinimumGap] [bit] NULL,
	[MaximumConsecutive] [bit] NULL,
	[EnableFile] [bit] NULL,
	[CannotBeTakenTogether] [nvarchar](100) NULL,
	[LeaveTypeId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[AllowPastDates] [bit] NULL,
	[AllowFutureDates] [bit] NULL,
	[IsAllowRequestNextDays] [bit] NULL,
	[IsToBeApplied] [bit] NULL,
	[Weekendsbetweenleaveperiod] [bit] NULL,
	[Holidaybetweenleaveperiod] [bit] NULL,
	[GrantMinimumNoOfRequestDay] [decimal](18, 2) NULL,
	[GrantMaximumNoOfRequestDay] [decimal](18, 2) NULL,
	[GrantMaximumNoOfPeriod] [int] NULL,
	[GrantMaximumNoOfDay] [int] NULL,
	[GrantMinimumGapTwoApplicationDay] [decimal](18, 2) NULL,
	[GrantUploadDocumentSpecificPeriodDay] [decimal](18, 2) NULL,
	[IsGrantRequestPastDay] [bit] NULL,
	[GrantRequestPastDay] [int] NULL,
	[IsGrantRequestFutureDay] [bit] NULL,
	[GrantRequestFutureDay] [int] NULL,
	[GrantResetLeaveAfterDays] [decimal](18, 2) NULL,
	[ToBeAdvanced] [decimal](18, 2) NULL
PRIMARY KEY CLUSTERED 
(
	[LeaveRestrictionsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
