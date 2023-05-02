IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='SupportingDocuments')
	CREATE TABLE [dbo].[SupportingDocuments](
		[DocumentId] [int] IDENTITY(1,1) NOT NULL,
		[SourceId] [int] NULL,
		[SourceType] [varchar](100) NULL,
		[DocumentCategory] [varchar](100) NULL,
		[IsApproved] [bit] NULL,
		[DocumentPath] [varchar](max) NULL,
		[DocumentName] [varchar](1000) NULL,
		[DocumentType] [varchar](100) NULL,
		[DocumentSize] [decimal](18,2) NULL,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedOn] [datetime] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[DocumentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO