USE [PMSNexus_ExitManagement]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='ReasonLeavingPosition')
CREATE TABLE [dbo].[ReasonLeavingPosition](
	[ReasonLeavingPositionId] [int] IDENTITY(1,1) NOT NULL,
	[ResignationInterviewId] [int] NULL,
	[LeavingPositionId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL
	PRIMARY KEY CLUSTERED
(
	[ReasonLeavingPositionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO