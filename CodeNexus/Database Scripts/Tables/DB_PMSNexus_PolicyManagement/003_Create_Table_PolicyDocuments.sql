USE [PMSNexus_PolicyManagement]
GO

CREATE TABLE [dbo].[PolicyDocuments](
	[PolicyDocumentId] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](250) NULL,
	[Description] [varchar](1000) NULL,
	[FolderId] [int] NULL,
	[ValidTo] [datetime] NULL,
	[Acknowledgement] [bit] NULL,
	[IsNotifyViaEmail] [bit] NULL,
	[IsNotifyViaFeeds] [bit] NULL,
	[IsEmployeeAbleToDownload] [bit] NULL,
	[IsRMAbleToDownload] [bit] NULL,
	[IsEmployeeAbleToView] [bit] NULL,
	[IsRMAbleToView] [bit] NULL,
	[FilePath] [varchar](500) NULL,
	[EmployeeId] [int] NULL,
	[ToShareWithAll] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_PolicyDocuments] PRIMARY KEY CLUSTERED 
(
	[PolicyDocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


