USE [PMSNexus_Accounts]
GO
TRUNCATE TABLE [dbo].[AppConstants]
GO
SET IDENTITY_INSERT [dbo].[AppConstants] ON 
Go
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 1,N'CustomerStatus', N'Active', N'ACT', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 2,N'CustomerStatus', N'In Active', N'INA', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 3,N'CustomerStatus', N'Hold', N'HLD', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 4,N'CustomerFlowStatus', N'Pending Approval', N'PA', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 5,N'CustomerFlowStatus', N'Action Required', N'AR', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 6,N'CustomerFlowStatus', N'Active', N'AC', 1, getdate())


SET IDENTITY_INSERT [dbo].[AppConstants] OFF

