USE [PMSNexus_Leaves]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LeaveDuration]') AND type in (N'U'))
ALTER TABLE [dbo].[LeaveDuration] DROP CONSTRAINT IF EXISTS [FK_AppConstants]
GO
/****** Object:  Table [dbo].[LeaveDuration]    Script Date: 24-08-2021 13:31:46 ******/
DROP TABLE IF EXISTS [dbo].[LeaveDuration]
GO
/****** Object:  Table [dbo].[LeaveDuration]    Script Date: 24-08-2021 13:31:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeaveDuration](
	[LeaveDurationId] [int] IDENTITY(1,1) NOT NULL,
	[DurationId] [int] NOT NULL,
	[LeaveTypeId] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LeaveDurationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO