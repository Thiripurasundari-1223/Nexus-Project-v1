IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='NoticePeriodCategory')
CREATE TABLE [dbo].[NoticePeriodCategory](
	[NoticePeriodCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](250) NULL,
	[SubCategoryName] [varchar](250) NULL,
	[NoticePeriodDays] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_NoticePeriodCategory] PRIMARY KEY CLUSTERED 
(
	[NoticePeriodCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO