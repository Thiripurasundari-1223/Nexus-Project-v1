USE [PMSNexus_Employees]
GO

TRUNCATE TABLE [dbo].[EmployeeLocation]
GO
SET IDENTITY_INSERT [dbo].[EmployeeLocation] ON 

INSERT into [dbo].[EmployeeLocation] ([LocationId],[Location], [CreatedOn], [CreatedBy]) VALUES (1,'Chennai', GETDATE(), 1)
INSERT into [dbo].[EmployeeLocation] ([LocationId],[Location], [CreatedOn], [CreatedBy]) VALUES (2,'Bangalore',  GETDATE(), 1)
INSERT into [dbo].[EmployeeLocation] ([LocationId],[Location], [CreatedOn], [CreatedBy]) VALUES (3,'Mumbai',  GETDATE(), 1)
INSERT into [dbo].[EmployeeLocation] ([LocationId],[Location], [CreatedOn], [CreatedBy]) VALUES (4,'New Jersey',  GETDATE(), 1)
SET IDENTITY_INSERT [dbo].[EmployeeLocation] OFF
GO

