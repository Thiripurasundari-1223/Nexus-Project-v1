USE [PMSNexus_ExitManagement]
GO
CREATE TABLE [dbo].[FinanceCheckList](
    [FinanceCheckListId]  [int] IDENTITY(1,1) NOT NULL,
	[ResignationChecklistId] [int]  NULL,
	[ApprovedBy] [int]  NULL,
	[Status] [nvarchar](100) NULL,
	[JoiningBonus] [nvarchar](max) NULL,
	[JoiningBonusRemark] [nvarchar](max) NULL,
	[RetentionBonus] [nvarchar](max) NULL,
	[RetentionBonusRemark] [nvarchar](max) NULL,
	[SalaryAdvance] [nvarchar](max) NULL,
	[SalaryAdvanceRemark] [nvarchar](max) NULL,
	[Loans] [nvarchar](max) NULL,
	[LoansRemark] [nvarchar](max) NULL,
	[TravelAdvance] [nvarchar](max) NULL,
	[TravelAdvanceRemark] [nvarchar](max) NULL,
	[TravelCardReturned] [nvarchar](max) NULL,
	[TravelCardReturnedRemark] [nvarchar](max) NULL,
	[RelocationCost] [nvarchar](max) NULL,
	[RelocationCostRemark] [nvarchar](max) NULL,
	[TravelKitAllowance] [nvarchar](max) NULL,
	[TravelKitAllowanceRemark] [nvarchar](max) NULL,
	[ITProofsId] [int]  NULL,
	[ITProofsRemark] [nvarchar](max) NULL,
	[TrainingBond] [nvarchar](max) NULL,
	[TrainingBondRemark] [nvarchar](max) NULL,
	[ITRecovery] [nvarchar](max) NULL,
	[ITRecoveryRemark] [nvarchar](max) NULL,
	[AdministrationRecovery] [nvarchar](max) NULL,
	[AdministrationRecoveryRemark] [nvarchar](max) NULL,
	[GratuityEligibilityId] [int]  NULL,
	[GratuityEligibilityRemark] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[FinanceCheckListId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
