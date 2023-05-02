USE [PMSNexus_PolicyManagement]
GO

CREATE TABLE [dbo].[Announcement](
	[AnnouncementId] [int] IDENTITY(1,1) NOT NULL,
	[Topic] [nvarchar](2000) NULL,
	[Subject] [nvarchar](4000) NULL,
	[Description] [nvarchar](max) NULL,
	[ExpiryDate] [datetime] NULL,
	[Image] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_Announcement] PRIMARY KEY CLUSTERED 
(
	[AnnouncementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO