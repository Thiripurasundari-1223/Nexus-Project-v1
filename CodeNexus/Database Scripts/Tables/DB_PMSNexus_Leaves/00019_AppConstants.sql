USE [PMSNexus_Leaves]
GO
/****** Object:  Table [dbo].[AppConstants]    Script Date: 24-08-2021 13:30:51 ******/
DROP TABLE IF EXISTS [dbo].[AppConstants]
GO
/****** Object:  Table [dbo].[AppConstants]    Script Date: 24-08-2021 13:30:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppConstants](
	[AppConstantId] [int] IDENTITY(1,1) NOT NULL,
	[AppConstantType] [varchar](250) NULL,
	[DisplayName] [varchar](500) NULL,
	[AppConstantValue] [varchar](1000) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[AppConstantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
