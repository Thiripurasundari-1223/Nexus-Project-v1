USE [PMSNexus_ExitManagement]
GO
CREATE TABLE [dbo].[ResignationInterview](
	[ResignationInterviewId] [int] IDENTITY(1,1) NOT NULL,
	[ResignationDetailsId] [int] NULL,
	[EmployeeID] [int] NULL,
	[OverallView] [nvarchar](max) NULL,
	[ReasonOfRelievingPositionId] [int] NULL,
	[ReasonOfRelieving] [nvarchar](max) NULL,
	[EventTriggeredForAlternativeJob] [nvarchar](max) NULL,
	[ShareProspectiveEmployer] [nvarchar](max) NULL,
	[AttractedProspectiveEmployer] [nvarchar](max) NULL,
	[UnresolvedIssues] [nvarchar](max) NULL,
	[EnjoyDislike] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ResignationInterviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO