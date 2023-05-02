USE [PMSNexus_Employees]
GO

TRUNCATE TABLE [dbo].[ProbationStatus]
GO

INSERT [dbo].[ProbationStatus] ([ProbationStatusName], [CreatedBy], [CreatedOn]) VALUES (N'Probation', 1, GETDATE())
INSERT [dbo].[ProbationStatus] ([ProbationStatusName], [CreatedBy], [CreatedOn]) VALUES (N'Probation Extended', 1, GETDATE())
INSERT [dbo].[ProbationStatus] ([ProbationStatusName], [CreatedBy], [CreatedOn]) VALUES (N'Confirmed', 1, GETDATE())

