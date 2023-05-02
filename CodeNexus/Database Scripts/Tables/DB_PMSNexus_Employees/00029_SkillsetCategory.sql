IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='SkillsetCategory')
CREATE TABLE [dbo].[SkillsetCategory](
	[SkillsetCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[SkillsetCategoryName] [varchar](250) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_SkillsetCategory] PRIMARY KEY CLUSTERED 
(
	[SkillsetCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


