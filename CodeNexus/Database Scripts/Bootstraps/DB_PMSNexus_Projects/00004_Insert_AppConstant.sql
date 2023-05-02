USE [PMSNexus_Projects]
GO
TRUNCATE TABLE [dbo].[AppConstants]
GO
SET IDENTITY_INSERT [dbo].[AppConstants] ON 
Go
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 1,N'BillableType', N'Flexible', N'Flexible', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 2,N'BillableType', N'FlexibleLimit', N'Flexible-Limit', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 3,N'BillableType', N'Fixed', N'Fixed', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 4,N'BillableFrequency', N'Hours', N'Hours', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 5,N'BillableFrequency', N'Days', N'Days', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 6,N'BillableFrequency', N'Months', N'Months', 1, getdate())
GO

SET IDENTITY_INSERT [dbo].[AppConstants] OFF

