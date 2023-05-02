IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AccountRelatedIssue')
	CREATE TABLE [dbo].[AccountRelatedIssue](
		[AccountRelatedIssueId] [int] IDENTITY(1,1) NOT NULL,
		[AccountRelatedIssueReason] [varchar](1000) NULL,
		[ReasonDescription] [varchar](2000) NULL,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedOn] [datetime] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[AccountRelatedIssueId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
