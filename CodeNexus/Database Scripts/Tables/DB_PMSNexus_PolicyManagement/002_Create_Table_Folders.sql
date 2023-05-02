USE [PMSNexus_PolicyManagement]
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Folders' and xtype='U')
CREATE TABLE [dbo].[Folders](
	[FolderId] [int] IDENTITY(1,1) NOT NULL,
	[FolderName] [varchar](250) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_Folders] PRIMARY KEY CLUSTERED 
(
	[FolderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO