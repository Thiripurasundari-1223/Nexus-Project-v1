USE [PMSNexus_ExitManagement]
GO
TRUNCATE TABLE [dbo].[CheckListApprover]
GO
SET IDENTITY_INSERT [dbo].[CheckListApprover] ON 
Go
INSERT [dbo].[CheckListApprover] ([CheckListApproverId], [LevelId], [ApprovedRole], [CreatedBy], [CreatedOn]) VALUES ( 1,1, N'manager', 1, getdate())
GO
INSERT [dbo].[CheckListApprover] ([CheckListApproverId], [LevelId], [ApprovedRole], [CreatedBy], [CreatedOn]) VALUES ( 2,2, N'pmo',  1, getdate())
GO
INSERT [dbo].[CheckListApprover] ([CheckListApproverId], [LevelId], [ApprovedRole], [CreatedBy], [CreatedOn]) VALUES ( 3,3,N'it', 1, getdate())
GO
INSERT [dbo].[CheckListApprover] ([CheckListApproverId], [LevelId], [ApprovedRole], [CreatedBy], [CreatedOn]) VALUES ( 4,4, N'admin', 1, getdate())
GO
INSERT [dbo].[CheckListApprover] ([CheckListApproverId], [LevelId], [ApprovedRole], [CreatedBy], [CreatedOn]) VALUES ( 5,5, N'finance', 1, getdate())
GO
INSERT [dbo].[CheckListApprover] ([CheckListApproverId], [LevelId], [ApprovedRole], [CreatedBy], [CreatedOn]) VALUES ( 6,6, N'hr', 1, getdate())
GO
SET IDENTITY_INSERT [dbo].[CheckListApprover] OFF