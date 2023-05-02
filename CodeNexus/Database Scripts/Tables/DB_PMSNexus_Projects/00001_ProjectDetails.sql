IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='ProjectDetails')
CREATE TABLE [dbo].[ProjectDetails](
ProjectId [int] IDENTITY(1,1) NOT NULL,
AccountId Int NOT NULL,
ProjectName [varchar](2000) NULL,
ProjectDescription [varchar](2000) NULL,
ProjectDuration Decimal(18,2) NULL,
ProjectStartdate Date NULL,
ProjectEndDate Date NULL,
Documents [varchar](2000) NULL,
Comments [varchar](2000) NULL,
ProjectTypeId  [int] NULL,
ProjectSPOC  [int] NULL,
CurrencyTypeId  [int] NULL,
FinanceManagerId [int] NULL,
EngineeringLeadId int NULL,
SkillSet  [varchar](2000) NULL,
Experience  [varchar](2000) NULL,
SkillRate  [varchar](2000) NULL,
RateFrequency  [varchar](2000) NULL,
TotalSOWAmount  Decimal(18,2) NULL,
ProjectStatus  [varchar](2000) NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[IsDraft] [bit] NULL,
[ProjectStatusCode] [varchar](100) NULL,
[ProjectChanges] [varchar](max) NULL,
FormattedProjectId [varchar](250) NULL,
BillableTypeId  int NULL,
BillableFrequencyId  int NULL,
BillableValue  int NULL,
PRIMARY KEY CLUSTERED 
	(
		[ProjectId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO