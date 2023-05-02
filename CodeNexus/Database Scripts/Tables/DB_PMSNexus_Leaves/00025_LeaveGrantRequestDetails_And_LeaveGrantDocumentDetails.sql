USE [PMSNexus_Leaves]
GO
/****** Object:  Table [dbo].[LeaveGrantDocumentDetails]    Script Date: 07-10-2021 23:27:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeaveGrantDocumentDetails](
	[LeaveGrantDocumentDetailId] [int] IDENTITY(1,1) NOT NULL,
	[LeaveGrantDetailId] [int] NOT NULL,
	[DocumentName] [nvarchar](500) NULL,
	[DocumentPath] [nvarchar](2000) NULL,
	[DocumentType] [nvarchar](500) NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LeaveGrantRequestDetails]    Script Date: 07-10-2021 23:27:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeaveGrantRequestDetails](
	[LeaveGrantDetailId] [int] IDENTITY(1,1) NOT NULL,
	[LeaveTypeId] [int] NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[NumberOfDay] [decimal](18, 2) NULL,
	[BalanceDay] [decimal](18, 2) NULL
	[Reason] [nvarchar](1000) NULL,
	[EffectiveFromDate] [datetime] NULL,
	[EffectiveToDate] [datetime] NULL,
	[Status] [nvarchar](500) NULL,
	[RejectionReason] [nvarchar](4000) NULL,
	[IsActive] [bit] NULL,
	[IsLeaveAdjustment] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL
) ON [PRIMARY]
GO

