USE [PMSNexus_Employees]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='PreventAbsentNotification')
CREATE TABLE [dbo].[PreventAbsentNotification](
[PreventAbsentNotificationId] [int] IDENTITY(1,1) NOT NULL,
[EmployeeId] [int] NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
	(
		[PreventAbsentNotificationId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
 
