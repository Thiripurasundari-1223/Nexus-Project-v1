USE [PMSNexus_Leaves]
GO

/****** Object:  Table [dbo].[LeaveDuration]    Script Date: 09-09-2021 00:10:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProRateMonthDetails](
	[ProRateMonthDetailId] [int] IDENTITY(1,1) NOT NULL,
	[LeaveTypeId] [int] NOT NULL,
	[Fromday][varchar] (100) NULL,
	[Today][varchar] (100) NULL,
	[Count][decimal] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProRateMonthDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


