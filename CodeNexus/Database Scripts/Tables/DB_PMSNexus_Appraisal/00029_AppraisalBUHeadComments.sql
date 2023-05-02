USE [PMSNexus_Appraisal]
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AppraisalBUHeadComments')

CREATE TABLE [dbo].[AppraisalBUHeadComments](
	[AppraisalBUHeadCommentsId] [int] IDENTITY(1,1) NOT NULL,
	[AppCycle_Id] [int] NOT NULL,
	[Department_Id] [int] NOT NULL,
	[Employee_Id] [int] NOT NULL,
	[Comment] [nvarchar](500) NOT NULL,
	[Created_By] [int] NULL,
	[Created_On] [datetime] NULL,
	[Updated_By] [int] NULL,
	[Updated_On] [datetime] NULL,
 CONSTRAINT [PK_AppraisalBUHeadComments] PRIMARY KEY CLUSTERED 
(
	[AppraisalBUHeadCommentsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


