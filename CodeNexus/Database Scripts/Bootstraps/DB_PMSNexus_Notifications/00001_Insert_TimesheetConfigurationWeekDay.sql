USE [PMSNexus_Notifications]
GO
TRUNCATE TABLE [dbo].[TimesheetConfigurationWeekDay]
GO
SET IDENTITY_INSERT [dbo].[TimesheetConfigurationWeekDay] ON 

INSERT INTO [TimesheetConfigurationWeekDay] ([TimesheetConfigurationWeekdayId], [DayName], CreatedOn, CreatedBy) VALUES (1, N'Monday', GETDATE(), 1)
INSERT INTO [TimesheetConfigurationWeekDay] ([TimesheetConfigurationWeekdayId], [DayName], CreatedOn, CreatedBy) VALUES (2, N'Tuesday', GETDATE(), 1)
INSERT INTO [TimesheetConfigurationWeekDay] ([TimesheetConfigurationWeekdayId], [DayName], CreatedOn, CreatedBy) VALUES (3, N'Wednesday', GETDATE(), 1)
INSERT INTO [TimesheetConfigurationWeekDay] ([TimesheetConfigurationWeekdayId], [DayName], CreatedOn, CreatedBy) VALUES (4, N'Thursday', GETDATE(), 1)
INSERT INTO [TimesheetConfigurationWeekDay] ([TimesheetConfigurationWeekdayId], [DayName], CreatedOn, CreatedBy) VALUES (5, N'Friday', GETDATE(), 1)
INSERT INTO [TimesheetConfigurationWeekDay] ([TimesheetConfigurationWeekdayId], [DayName], CreatedOn, CreatedBy) VALUES (6, N'Saturday', GETDATE(), 1)
INSERT INTO [TimesheetConfigurationWeekDay] ([TimesheetConfigurationWeekdayId], [DayName], CreatedOn, CreatedBy) VALUES (7, N'Sunday', GETDATE(), 1)
SET IDENTITY_INSERT [dbo].[TimesheetConfigurationWeekDay] OFF
GO