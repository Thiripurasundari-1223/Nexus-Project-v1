USE [PMSNexus_Leaves]
GO

/****** Object:  Table [dbo].[Leaves]    Script Date: 07-05-2021 12:25:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Leaves](
	[LeaveId] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NULL,
	[LeaveTypeId] [int] NULL,
	[FromDate] [date] NULL,
	[ToDate] [date] NULL,
	[NoOfDays] [DECIMAL](18,2) NULL,
	[Reason] [nvarchar](max) NULL,
	[Status] [nvarchar](50) NULL,
	[Feedback] [nvarchar](max) NULL,
	[LeaveRejectionReasonId] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL, 
	[ManagerId] [int] NULL,
	[ApproveRejectBy]  int NULL,
	[ApproveRejectOn]  datetime NULL,
	[ApproveRejectName]  nvarchar(250) NULL,
 CONSTRAINT [PK_Leaves] PRIMARY KEY CLUSTERED 
(
	[LeaveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


