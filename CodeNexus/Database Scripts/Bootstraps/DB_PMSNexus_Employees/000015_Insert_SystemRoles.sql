USE PMSNexus_Employees
Go
TRUNCATE TABLE [dbo].[SystemRoles]
GO
SET IDENTITY_INSERT [dbo].[SystemRoles] ON;
INSERT [dbo].[SystemRoles] ([RoleId],[RoleName], [RoleShortName], [RoleDescription], [CreatedBy], [CreatedOn]) VALUES (1,N'Director', N'Director', N'Director',1,GETDATE())
INSERT [dbo].[SystemRoles] ([RoleId],[RoleName], [RoleShortName], [RoleDescription], [CreatedBy], [CreatedOn]) VALUES (2,N'BU Head', N'BU Head', N'BU Head', 1, GETDATE())
INSERT [dbo].[SystemRoles] ([RoleId],[RoleName], [RoleShortName], [RoleDescription], [CreatedBy], [CreatedOn]) VALUES (3,N'Finance', N'Finance', N'Finance', 1, GETDATE())
INSERT [dbo].[SystemRoles] ([RoleId],[RoleName], [RoleShortName], [RoleDescription], [CreatedBy], [CreatedOn]) VALUES (4,N'HR BU Head', N'HR BU Head', N'HR BU Head', 1, GETDATE())
INSERT [dbo].[SystemRoles] ([RoleId],[RoleName], [RoleShortName], [RoleDescription], [CreatedBy], [CreatedOn]) VALUES (5,N'HR', N'HR', N'HR', 1, GETDATE())
INSERT [dbo].[SystemRoles] ([RoleId],[RoleName], [RoleShortName], [RoleDescription], [CreatedBy], [CreatedOn]) VALUES (6,N'IT', N'IT', N'IT', 1, GETDATE())
INSERT [dbo].[SystemRoles] ([RoleId],[RoleName], [RoleShortName], [RoleDescription], [CreatedBy], [CreatedOn]) VALUES (7,N'Manager', N'Manager', N'Manager', 1, GETDATE())
INSERT [dbo].[SystemRoles] ([RoleId],[RoleName], [RoleShortName], [RoleDescription], [CreatedBy], [CreatedOn]) VALUES (8,N'PMO', N'PMO', N'PMO', 1, GETDATE())
INSERT [dbo].[SystemRoles] ([RoleId],[RoleName], [RoleShortName], [RoleDescription], [CreatedBy], [CreatedOn]) VALUES (9,N'Super Admin', N'Super Admin', N'Super Admin', 1, GETDATE())
INSERT [dbo].[SystemRoles] ([RoleId],[RoleName], [RoleShortName], [RoleDescription], [CreatedBy], [CreatedOn]) VALUES (10,N'Team Incharge', N'Team Incharge', N'Team Incharge', 1, GETDATE())
INSERT [dbo].[SystemRoles] ([RoleId],[RoleName], [RoleShortName], [RoleDescription], [CreatedBy], [CreatedOn]) VALUES (11,N'Team member', N'Team member', N'Team member', 1, GETDATE())
SET IDENTITY_INSERT [dbo].[SystemRoles] OFF;
GO