IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AccountChangeRequest')
CREATE TABLE [dbo].[AccountChangeRequest](
[AccountChangeRequestId] [int] IDENTITY(1,1) NOT NULL,
[AccountId] int NOT NULL,
[AccountRelatedIssueId] int NOT NULL,
[Comments] [varchar](2000) NULL,
[IsActive] [bit] NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL
PRIMARY KEY CLUSTERED 
	(
		[AccountChangeRequestId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AccountComments')
CREATE TABLE [dbo].[AccountComments](
[AccountCommentId] [int] IDENTITY(1,1) NOT NULL,
[AccountId] int NOT NULL,
[Comments] [varchar](2000) NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL
PRIMARY KEY CLUSTERED 
	(
		[AccountCommentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO