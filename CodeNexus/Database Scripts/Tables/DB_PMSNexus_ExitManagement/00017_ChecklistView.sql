USE [PMSNexus_ExitManagement]
GO
CREATE TABLE [dbo].[CheckListView](
    [CheckListViewId]  [int] IDENTITY(1,1) NOT NULL,
	[ApproverRole] [nvarchar](100) NULL,
	[Manager] [bit] NULL,
	[PMO] [bit] NULL,
	[IT] [bit] NULL,
	[Finance] [bit] NULL,
	[Admin] [bit] NULL,
	[HR] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CheckListViewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
