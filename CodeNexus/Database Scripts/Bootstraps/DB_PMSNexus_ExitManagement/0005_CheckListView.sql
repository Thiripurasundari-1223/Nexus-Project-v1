USE [PMSNexus_ExitManagement]
GO
TRUNCATE TABLE [dbo].[CheckListView]
GO
SET IDENTITY_INSERT [dbo].[CheckListView] ON 
Go
INSERT [dbo].[CheckListView] ([CheckListViewId],[ApproverRole],[Manager],[PMO],[IT],[Finance],[Admin],[HR],[CreatedBy],[CreatedOn]) VALUES ( 1,N'manager',1,0,0,0,0,0, 1, getdate())
GO
INSERT [dbo].[CheckListView] ([CheckListViewId],[ApproverRole],[Manager],[PMO],[IT],[Finance],[Admin],[HR],[CreatedBy],[CreatedOn]) VALUES ( 2,N'pmo',1,1,0,0,0,0, 1, getdate())
GO
INSERT [dbo].[CheckListView] ([CheckListViewId],[ApproverRole],[Manager],[PMO],[IT],[Finance],[Admin],[HR],[CreatedBy],[CreatedOn]) VALUES ( 3,N'it',1,0,1,0,0,0, 1, getdate())
GO
INSERT [dbo].[CheckListView] ([CheckListViewId],[ApproverRole],[Manager],[PMO],[IT],[Finance],[Admin],[HR],[CreatedBy],[CreatedOn]) VALUES ( 4,N'admin',0,0,1,0,1,0, 1, getdate())
GO
INSERT [dbo].[CheckListView] ([CheckListViewId],[ApproverRole],[Manager],[PMO],[IT],[Finance],[Admin],[HR],[CreatedBy],[CreatedOn]) VALUES ( 5,N'finance',1,1,1,1,1,0, 1, getdate())
GO
INSERT [dbo].[CheckListView] ([CheckListViewId],[ApproverRole],[Manager],[PMO],[IT],[Finance],[Admin],[HR],[CreatedBy],[CreatedOn]) VALUES ( 6,N'hr',1,1,1,1,1,1, 1, getdate())
GO
INSERT [dbo].[CheckListView] ([CheckListViewId],[ApproverRole],[Manager],[PMO],[IT],[Finance],[Admin],[HR],[CreatedBy],[CreatedOn]) VALUES ( 7,N'superadmin',1,1,1,1,1,1, 1, getdate())
GO
SET IDENTITY_INSERT [dbo].[CheckListView] OFF