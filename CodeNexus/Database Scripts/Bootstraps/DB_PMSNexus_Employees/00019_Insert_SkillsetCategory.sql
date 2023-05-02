TRUNCATE TABLE [SkillsetCategory]
GO

INSERT [dbo].[SkillsetCategory] ([SkillsetCategoryName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (N'Frontend',  1, GETDATE(),  1, GETDATE())
INSERT [dbo].[SkillsetCategory] ([SkillsetCategoryName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (N'Backend',  1, GETDATE(),  1, GETDATE())
INSERT [dbo].[SkillsetCategory] ([SkillsetCategoryName], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (N'Database', 1, GETDATE(),  1, GETDATE())
