TRUNCATE TABLE DepartmentReportMapping
Go
SET IDENTITY_INSERT DepartmentReportMapping OFF
GO
declare @DepartmentId int,@EmployeeCategoryId int, @ReportId int
select @DepartmentId=DepartmentId from Department where DepartmentName='Engineering'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Contribution'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceUtilization'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceUtilization'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
-- IT --
select @DepartmentId=DepartmentId from Department where DepartmentName='IT'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

-- Marketing --
select @DepartmentId=DepartmentId from Department where DepartmentName='Marketing'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

-- Finance --
select @DepartmentId=DepartmentId from Department where DepartmentName='Finance'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceBillability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='MonthlyRevenue'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceBillability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='MonthlyRevenue'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceBillability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='MonthlyRevenue'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='OverallRevenue'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

-- HR Human Resources--
select @DepartmentId=DepartmentId from Department where DepartmentName='Human Resources'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Ops Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Attendance'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Associates'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Ops Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Attendance'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Associates'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Attendance'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Associates'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalStatus'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

-- HR-Recruitment --
select @DepartmentId=DepartmentId from Department where DepartmentName='Human Resources - Recruitment'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Talent Acquisition Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Talent Acquisition Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
-- US Sales --
select @DepartmentId=DepartmentId from Department where DepartmentName='US Sales'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='CustomerOnboard'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceAvailability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='CustomerOnboard'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceAvailability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='CustomerOnboard'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceUtilization'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceAvailability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

-- Indian Sales --
select @DepartmentId=DepartmentId from Department where DepartmentName='India Sales'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='CustomerOnboard'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceAvailability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='CustomerOnboard'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceAvailability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='CustomerOnboard'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceUtilization'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceAvailability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

-- US Consulting --
select @DepartmentId=DepartmentId from Department where DepartmentName='US Consulting'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Customers'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Customers'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Customers'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

-- Consulting --
select @DepartmentId=DepartmentId from Department where DepartmentName='Consulting'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Customers'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Customers'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Customers'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

-- Corperate --
select @DepartmentId=DepartmentId from Department where DepartmentName='Corporate'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='COO' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='ResourceUtilization'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='BillableHours'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceBillability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='OverallRevenue'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Customers'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='CEO' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='ResourceUtilization'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='BillableHours'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceBillability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='OverallRevenue'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Customers'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Director' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='ResourceUtilization'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='BillableHours'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceBillability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='OverallRevenue'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Customers'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='ResourceUtilization'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='BillableHours'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='ResourceBillability'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='OverallRevenue'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Customers'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalStatus'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
-- Admin --
select @DepartmentId=DepartmentId from Department where DepartmentName='Admin'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Admin' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalStatus'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Super Admin' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalStatus'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))


--Intelligence --
select @DepartmentId=DepartmentId from Department where DepartmentName='Intelligence'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))


--People Experience --
select @DepartmentId=DepartmentId from Department where DepartmentName='People Experience'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalStatus'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='HR BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalStatus'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Super Admin' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalStatus'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))


--Recruitment --
select @DepartmentId=DepartmentId from Department where DepartmentName='Recruitment'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

--SFL --
select @DepartmentId=DepartmentId from Department where DepartmentName='SFL'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Projects'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

--Client Delivery--
select @DepartmentId=DepartmentId from Department where DepartmentName='Client Delivery'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

--Experiences--
select @DepartmentId=DepartmentId from Department where DepartmentName='Experiences'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))


--Talent Acquisition--
select @DepartmentId=DepartmentId from Department where DepartmentName='Talent Acquisition'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

--Sales - US--
select @DepartmentId=DepartmentId from Department where DepartmentName='Sales - US'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

--Sales - APAC--
select @DepartmentId=DepartmentId from Department where DepartmentName='Sales - APAC'
select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Individual' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='Manager' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

select @EmployeeCategoryId=EmployeeCategoryId from EmployeeCategory where EmployeeCategoryName='BU Head' and DepartmentId=@DepartmentId
select @ReportId=ReportId from Reports where ReportName='AppraisalReview'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='TimesheetsApproval'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Timesheets'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))
select @ReportId=ReportId from Reports where ReportName='Appraisal'
INSERT [dbo].[DepartmentReportMapping] ([DepartmentId], [EmployeeCategoryId], [ReportId], [CreatedBy], [ModifiedOn]) VALUES (@DepartmentId, @EmployeeCategoryId, @ReportId, 1, CAST(N'2021-04-21T18:37:09.840' AS DateTime))

Go
SET IDENTITY_INSERT [DepartmentReportMapping] OFF
GO