USE [PMSNexus_Leaves]
GO

/****** Object:  Table [dbo].[LeaveDuration]    Script Date: 06-09-2021 18:00:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LeaveTakenTogether](
	[LeaveTakenTogetherId] [int] IDENTITY(1,1) NOT NULL,
	[LeaveTypeId] [int] NOT NULL,
	[LeaveOrHolidayId] [int] NULL,
	[LeaveTakenType] [nvarchar] (50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LeaveTakenTogetherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


