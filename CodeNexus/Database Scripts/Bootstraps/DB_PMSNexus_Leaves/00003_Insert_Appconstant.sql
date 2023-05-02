USE [PMSNexus_Leaves]
GO
TRUNCATE TABLE [dbo].[AppConstants]
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'LeaveDuration', N'Full Day', N'FullDay', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES (N'LeaveDuration', N'Half Day', N'HalfDay', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES (N'ReportConfiguration', N'Actual leave balance', N'ActualLeaveBalance', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'ReportConfiguration', N'Detailed leave balance', N'DetailedLeaveBalance', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'LeaveType', N'Paid', N'Paid', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'LeaveType', N'Unpaid', N'Unpaid', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn])  VALUES ( N'LeaveType', N'Restricted Holiday', N'RestrictedHoliday', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'LeaveAccured', N'Monthly', N'monthly', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'LeaveAccured', N'HalfYearly', N'halfYearly', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn])  VALUES (N'LeaveAccured', N'Quarterly', N'quarterly', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn])  VALUES ( N'LeaveAccured', N'Yearly', N'yearly', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'LeaveAccured', N'One-time', N'onetime', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'AllowRequestPeriod', N'Weekly', N'weekly', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'AllowRequestPeriod', N'Monthly', N'monthly', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'AllowRequestPeriod', N'HalfYearly', N'halfYearly', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'AllowRequestPeriod', N'Quarterly', N'quarterly', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn])  VALUES (N'AllowRequestPeriod', N'Yearly', N'yearly', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'SpecificEmployeeLeave', N'DOJ', N'DOJ', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'SpecificEmployeeLeave', N'DOB', N'DOB', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'SpecificEmployeeLeave', N'Wedding anniversary', N'WeddingAnniversary', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'CarryForwardLeave', N'No. of days', N'NoOfDays', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'CarryForwardLeave', N'All', N'All', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'CarryForwardLeave', N'None', N'None', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'ReimbursementLeave', N'No. of days', N'NoOfDays', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'ReimbursementLeave', N'All', N'All', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'ReimbursementLeave', N'None', N'None', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'BalanceBasedOn', N'Fixed entitlement', N'FixedEntitlement', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'BalanceBasedOn', N'Leave grant', N'LeaveGrant', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'GrantLeaveRequestPeriod', N'Month', N'Month', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'GrantLeaveRequestPeriod', N'Year', N'Year', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'GrantLeaveApproval', N'Reporting To', N'ReportingTo', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'GrantLeaveApproval', N'Department BUHead', N'DepartmentBUHead', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'GrantLeaveApproval', N'HR BUHead', N'HRBUHead', 1, getdate())
GO
INSERT [dbo].[AppConstants] ( [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( N'GrantLeaveApproval', N'Others', N'Others', 1, getdate())
GO
