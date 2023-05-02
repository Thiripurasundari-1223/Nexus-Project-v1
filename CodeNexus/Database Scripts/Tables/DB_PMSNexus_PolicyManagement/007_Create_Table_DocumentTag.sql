USE [PMSNexus_PolicyManagement]
GO

CREATE TABLE [dbo].[DocumentTag](
	[DocumentTagId] [int] IDENTITY(1,1) NOT NULL,
	[TagName] [varchar](1000) NULL,
	[PlaceHolderName] [varchar](1000) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_DocumentTag] PRIMARY KEY CLUSTERED 
(
	[DocumentTagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO