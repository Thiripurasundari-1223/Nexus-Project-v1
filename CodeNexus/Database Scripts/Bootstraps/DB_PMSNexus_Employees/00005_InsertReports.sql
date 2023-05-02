USE PMSNexus_Employees
GO

TRUNCATE TABLE Reports
GO

Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('Projects','Projects','./assets/images/nexus/Projects.svg','pmsnexus/projects/homeprojects',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('Timesheets','Timesheets','./assets/images/nexus/Timesheets.svg','pmsnexus/timesheets/all-timesheets/my-timesheets',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('Contribution','Contribution','./assets/images/nexus/Contribution.svg','',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('Appraisal','My Progress Review','./assets/images/nexus/Appraisal@2x.png','pmsnexus/appraisal/appraisal-individual',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('ResourceUtilization','Resource Utilization','./assets/images/nexus/ResourceUtilization.svg','pmsnexus/projects/resourceutilization',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('TimesheetsApproval','Timesheets Approval','./assets/images/nexus/TimesheetsApproval.svg','pmsnexus/timesheets/all-timesheets/team-timesheets',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('AppraisalReview','Team Progress Review','./assets/images/nexus/appraisal-review@2x.png','pmsnexus/appraisal/appraisal-individual',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('CustomerOnboard','Customer Onboard','./assets/images/nexus/Customers@2x.png','pmsnexus/accounts/customeronboard',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('ResourceAvailability','Resource Availability','./assets/images/nexus/Resource Availability.svg','pmsnexus/projects/resourceavailability',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('Attendance','Attendance','','',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('Associates','Associates','./assets/images/nexus/Associates.svg','pmsnexus/employees/associates',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('AppraisalStatus','Appraisal Status','./assets/images/nexus/appraisal-status@2x.png','pmsnexus/appraisal/appraisal-status',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('MonthlyRevenue','Monthly Revenue','','',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('OverallRevenue','Overall Revenue','./assets/images/nexus/Overall Revenue.svg','',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('BillableHours','Billable Hours','./assets/images/nexus/Billable Hours.svg','',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('ResourceBillability','Resource Billability','./assets/images/nexus/Resource Billability.svg','pmsnexus/projects/resourcebillability',GETDATE(),1)
Insert into [dbo].[Reports] (ReportName,ReportTitle,ReportIconPath,ReportNavigationUrl,CreatedOn,CreatedBy) values ('Customers','Customers','./assets/images/nexus/Customers.svg','',GETDATE(),1)

GO