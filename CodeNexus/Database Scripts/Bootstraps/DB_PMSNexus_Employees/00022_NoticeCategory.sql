TRUNCATE TABLE [NoticePeriodCategory]
GO

INSERT [dbo].[NoticePeriodCategory] ([CategoryName],[SubCategoryName], [NoticePeriodDays], [CreatedBy], [CreatedOn]) VALUES (N'general',N'LessThanOneYear',90,  1, GETDATE())
INSERT [dbo].[NoticePeriodCategory] ([CategoryName],[SubCategoryName], [NoticePeriodDays], [CreatedBy], [CreatedOn]) VALUES (N'general',N'MoreThanOneYear',60,  1, GETDATE())
INSERT [dbo].[NoticePeriodCategory] ([CategoryName],[SubCategoryName], [NoticePeriodDays], [CreatedBy], [CreatedOn]) VALUES (N'sfl',N'SFL',90,  1, GETDATE())
INSERT [dbo].[NoticePeriodCategory] ([CategoryName],[SubCategoryName], [NoticePeriodDays], [CreatedBy], [CreatedOn]) VALUES (N'sales_juniors',N'BeforeConfirmation',7,  1, GETDATE())
INSERT [dbo].[NoticePeriodCategory] ([CategoryName],[SubCategoryName], [NoticePeriodDays], [CreatedBy], [CreatedOn]) VALUES (N'sales_juniors',N'AfterConfirmation',15,  1, GETDATE())
INSERT [dbo].[NoticePeriodCategory] ([CategoryName],[SubCategoryName], [NoticePeriodDays], [CreatedBy], [CreatedOn]) VALUES (N'trainee',N'BeforeConfirmation',90,  1, GETDATE())
INSERT [dbo].[NoticePeriodCategory] ([CategoryName],[SubCategoryName], [NoticePeriodDays], [CreatedBy], [CreatedOn]) VALUES (N'trainee',N'AfterConfirmation',60,  1, GETDATE())


