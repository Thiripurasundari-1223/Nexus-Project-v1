USE [PMSNexus_ExitManagement]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='ResignationReason')
CREATE TABLE [dbo].[ResignationReason](
	[ResignationReasonId] [int] IDENTITY(1,1) NOT NULL,
	[ResignationReasonName] [varchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	PRIMARY KEY CLUSTERED
(
	[ResignationReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO