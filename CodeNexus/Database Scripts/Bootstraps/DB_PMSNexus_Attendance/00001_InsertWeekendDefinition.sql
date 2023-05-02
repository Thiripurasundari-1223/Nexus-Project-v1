USE [PMSNexus_Attendance]
GO
TRUNCATE TABLE [dbo].[WeekendDefinition]
GO
SET IDENTITY_INSERT [dbo].[WeekendDefinition] ON 

INSERT INTO [WeekendDefinition] ([WeekendDayId], [WeekendDayName], CreatedOn, CreatedBy) VALUES (1, N'Sunday', GETDATE(), 1)
INSERT INTO [WeekendDefinition] ([WeekendDayId], [WeekendDayName], CreatedOn, CreatedBy) VALUES (2, N'Monday', GETDATE(), 1)
INSERT INTO [WeekendDefinition] ([WeekendDayId], [WeekendDayName], CreatedOn, CreatedBy) VALUES (3, N'Tuesday', GETDATE(), 1)
INSERT INTO [WeekendDefinition] ([WeekendDayId], [WeekendDayName], CreatedOn, CreatedBy) VALUES (4, N'Wednesday', GETDATE(), 1)
INSERT INTO [WeekendDefinition] ([WeekendDayId], [WeekendDayName], CreatedOn, CreatedBy) VALUES (5, N'Thursday', GETDATE(), 1)
INSERT INTO [WeekendDefinition] ([WeekendDayId], [WeekendDayName], CreatedOn, CreatedBy) VALUES (6, N'Friday', GETDATE(), 1)
INSERT INTO [WeekendDefinition] ([WeekendDayId], [WeekendDayName], CreatedOn, CreatedBy) VALUES (7, N'Saturday', GETDATE(), 1)
SET IDENTITY_INSERT [dbo].[WeekendDefinition] OFF
GO