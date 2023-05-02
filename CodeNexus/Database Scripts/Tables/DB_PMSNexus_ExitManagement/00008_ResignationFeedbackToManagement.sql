USE [PMSNexus_ExitManagement]
GO
CREATE TABLE [dbo].[ResignationFeedbackToManagement](
    [ResignationFeedbackId]  [int] IDENTITY(1,1) NOT NULL,
	[ResignationInterviewId] [int] NULL,
	[TrainingId] [int] NULL,
	[TrainingRemark] [nvarchar](max) NULL,
    [NatureOfWorkId] [int] NULL,
	[NatureOfWorkRemark] [nvarchar](max) NULL,
    [ImmediateSupervisorInvolmentId] [int] NULL,
	[ImmediateSupervisorInvolmentRemark] [nvarchar](max) NULL,
    [JobRecognitionId] [int] NULL,
	[JobRecognitionRemark] [nvarchar](max) NULL,
    [PerformanceFeedbackId] [int] NULL,
	[PerformanceFeedbackRemark] [nvarchar](max) NULL,
    [GrowthOpportunityId] [int] NULL,
	[GrowthOpportunityRemark] [nvarchar](max) NULL,
    [NewSkillsOpportunityId] [int] NULL,
	[NewSkillsOpportunityRemark] [nvarchar](max) NULL,
    [CompensationId] [int] NULL,
	[CompensationRemark] [nvarchar](max) NULL,
    [AnnualIncrementId] [int] NULL,
	[AnnualIncrementRemark] [nvarchar](max) NULL,
    [InformationSharingId] [int] NULL,
	[InformationSharingRemark] [nvarchar](max) NULL,
	[Other] [bit] NULL,
	[OtherRemark] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ResignationFeedbackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO