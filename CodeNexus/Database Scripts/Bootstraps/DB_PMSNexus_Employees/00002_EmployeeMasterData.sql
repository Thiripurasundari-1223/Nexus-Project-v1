USE PMSNexus_Employees
GO

TRUNCATE TABLE [dbo].[EmployeesType]
GO
SET IDENTITY_INSERT [dbo].[EmployeesType] ON 

INSERT [dbo].[EmployeesType] ([EmployeesTypeId], [EmployeesType], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (1, N'Permanent', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[EmployeesType] ([EmployeesTypeId], [EmployeesType], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (2, N'Contract', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
--INSERT [dbo].[EmployeesType] ([EmployeesTypeId], [EmployeesType], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (3, N'Probation', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[EmployeesType] ([EmployeesTypeId], [EmployeesType], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (4, N'Intern', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[EmployeesType] ([EmployeesTypeId], [EmployeesType], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (5, N'One time Engagement', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[EmployeesType] ([EmployeesTypeId], [EmployeesType], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (6, N'3rd Party Contract', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[EmployeesType] ([EmployeesTypeId], [EmployeesType], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (7, N'Director', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[EmployeesType] ([EmployeesTypeId], [EmployeesType], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (8, N'US Permanent', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[EmployeesType] ([EmployeesTypeId], [EmployeesType], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (9, N'US Contract', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
SET IDENTITY_INSERT [dbo].[EmployeesType] OFF
GO
TRUNCATE TABLE [dbo].[Skillset]
Go
SET IDENTITY_INSERT [dbo].[Skillset] ON 

INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (1, N'Java', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (2, N'.Net', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (3, N'Angular', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (4, N'JavaScript', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (5, N'Python', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (6, N'C/C++', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (7, N'Kotlin', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (8, N'PHP', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (9, N'React', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (10, N'NodeJS', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (11, N'Ruby on Rails', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (12, N'GO', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (13, N'R Language', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (14, N'SQL Server', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (15, N'My SQL', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (16, N'Postgres', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (17, N'MongoDB', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (18, N'HTML/CSS', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (19, N'Perl', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (20, N'Swift', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[Skillset] ([SkillsetId], [Skillset], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (21, N'QA(Testing)', CAST(N'2020-11-20T00:00:00.000' AS DateTime), CAST(N'2020-11-20T00:00:00.000' AS DateTime), 1, 1)
SET IDENTITY_INSERT [dbo].[Skillset] OFF
GO
TRUNCATE TABLE [dbo].[Department]
Go
SET IDENTITY_INSERT [dbo].[Department] ON 

INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (1, N'Human Resources', N'HR', N'Human Resources', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (2, N'Human Resources - Recruitment', N'HRRECRUIT', N'Human Resources - Recruitment', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (3, N'US - Consulting', N'USCONS', N'US - Consulting', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (4, N'Engineering', N'ENGG', N'Engineering', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (5, N'SFL', N'SFL', N'SFL', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (6, N'US Sales', N'USSAL', N'US Sales', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (7, N'India Sales', N'INDSAL', N'India Sales', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (8, N'Marketing', N'MKTING', N'Marketing', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (9, N'Consulting', N'CONS', N'Consulting', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (10, N'Finance', N'FIN', N'Finance', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (11, N'IT', N'IT', N'IT', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (12, N'Corporate', N'CORP', N'Corporate', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Department] ([DepartmentId], [DepartmentName], [DepartmentShortName], [DepartmentDescription], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (13, N'Administration', N'ADMN', N'Administration', CAST(N'2020-11-20T13:11:12.303' AS DateTime), NULL, 1, NULL)
SET IDENTITY_INSERT [dbo].[Department] OFF
GO

