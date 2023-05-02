
USE [PMSNexus_Notifications]
GO
TRUNCATE TABLE [dbo].[Status]
GO
SET IDENTITY_INSERT [dbo].[Status] ON 
GO
INSERT [dbo].[Status] ([StatusId], [StatusCode], [StatusName]) VALUES (1, N'ACT', N'Active')
GO
INSERT [dbo].[Status] ([StatusId], [StatusCode], [StatusName]) VALUES (2, N'INA', N'In Active')
GO
INSERT [dbo].[Status] ([StatusId], [StatusCode], [StatusName]) VALUES (3, N'HLD', N'Hold')
GO
SET IDENTITY_INSERT [dbo].[Status] OFF
GO
