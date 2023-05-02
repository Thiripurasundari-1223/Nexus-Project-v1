IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='SkillsetHistory')
CREATE TABLE [dbo].[SkillsetHistory](
	[SkillsetHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[SkillsetId] [int] NULL,
	[OldValue] [varchar](250) NULL,
	[NewValue] [varchar](250) NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[Category]  [varchar](250) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_SkillsetHistory] PRIMARY KEY CLUSTERED 
(
	[SkillsetHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


