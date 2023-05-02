USE [PMSNexus_Accounts]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerContactDetails]') AND type in (N'U'))
ALTER TABLE [dbo].[CustomerContactDetails] DROP CONSTRAINT IF EXISTS [FK_AccountDetails]
GO
/****** Object:  Table [dbo].[CustomerContactDetails]    Script Date: 23-08-2021 19:56:23 ******/
DROP TABLE IF EXISTS [dbo].[CustomerContactDetails]
GO
/****** Object:  Table [dbo].[CustomerContactDetails]    Script Date: 23-08-2021 19:56:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerContactDetails](
	[CustomerContactDetailId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[ContactPersonFirstName] [varchar](250) NULL,
	[ContactPersonLastName] [varchar](250) NULL,
	[ContactPersonPhoneNumber] [varchar](2000) NULL,
	[ContactPersonEmailAddress] [varchar](2000) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerContactDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CustomerContactDetails]  WITH CHECK ADD  CONSTRAINT [FK_AccountDetails] FOREIGN KEY([AccountId])
REFERENCES [dbo].[AccountDetails] ([AccountId])
GO
ALTER TABLE [dbo].[CustomerContactDetails] CHECK CONSTRAINT [FK_AccountDetails]
GO
