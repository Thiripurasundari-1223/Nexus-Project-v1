USE [PMSNexus_ExitManagement]
GO
CREATE TABLE [dbo].[HRCheckList](
    [HRCheckListId]  [int] IDENTITY(1,1) NOT NULL,
	[ResignationChecklistId] [int]  NULL,
	[ApprovedBy] [int]  NULL,
	[Status] [nvarchar](100) NULL,
	[NoticePayId] [int]  NULL,
	[NoticePayDay] [DECIMAL](18,2) NULL,
	[NoticePayRemark] [nvarchar](max) NULL,
	[ELBalanceId] [int]  NULL,
	[ELBalanceDay] [DECIMAL](18,2) NULL,
	[ELBalanceRemark] [nvarchar](max) NULL,
	[NoticePeriodWaiverRequestId] [int]  NULL,
	[NoticePeriodWaiverRequestRemark] [nvarchar](max) NULL,
	[LeaveBalanceSummaryId] [int]  NULL,
	[LeaveBalanceSummaryRemark] [nvarchar](max) NULL,
	[RehireEligibleId] [int]  NULL,
	[Comments] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[HRCheckListId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
