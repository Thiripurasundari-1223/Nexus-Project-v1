USE [PMSNexus_PolicyManagement]
GO

CREATE TABLE [dbo].[RequestedDocuments](
	[RequestedDocumentId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentTypeId] [int] NULL,
	[Reason] [varchar](1000) NULL,
	[Status] [varchar](50) NULL,
	[OtherDocumentType] [varchar](1000) NULL,
	[ApprovedOrRejectedOn] [datetime] NULL,
	[ApprovedOrRejectedBy] [int] NULL,
	[RejectedReason] [varchar](500) NULL,
	[PolicyDocumentId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_RequestedDocuments] PRIMARY KEY CLUSTERED 
(
	[RequestedDocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO