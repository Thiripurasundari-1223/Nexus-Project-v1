USE [PMSNexus_ExitManagement]
GO
CREATE TABLE [dbo].[ManagerCheckList](
    [ManagerCheckListId]  [int] IDENTITY(1,1) NOT NULL,
	[ResignationChecklistId] [int]  NULL,
	[ApprovedBy] [int]  NULL,
	[Status] [nvarchar](100) NULL,
    [KnowledgeTransferId] [int] NULL,
	[KnowledgeTransferRemark] [nvarchar](max) NULL,
    [MailID] [int] NULL,
	[RoutedTo] [nvarchar](max) NULL,
	[RoutedToRemark] [nvarchar](max) NULL,
    [ProjectDocumentsReturnedId] [int] NULL,
	[ProjectDocumentsReturnedRemark] [nvarchar](max) NULL,
    [RecoverPayNoticeId][int] NULL,
	[RecoverPayNoticeRemark] [nvarchar](max) NULL,
	[RouteReporteesTo] [nvarchar](max) NULL,
	[WaivingOffNoticePeriodReason] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ManagerCheckListId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
