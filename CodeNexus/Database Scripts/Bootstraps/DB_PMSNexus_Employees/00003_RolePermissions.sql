USE PMSNexus_Employees
GO

TRUNCATE TABLE [dbo].[Modules]
GO
SET IDENTITY_INSERT [dbo].[Modules] ON 
Go
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES ( 1,N'Home',N'Home and Dashboard', N'home_work', N'/pmsnexus/homedetail',N'Digital transformation is the integration of digital technology into all areas of a business, fundamentally changing how you operate and deliver value to customers. It also a cultural change that requires organizations to continually challenge the status quo, experiment, and get comfortable with failure.', N'Digital transformation is the integration of digital technology into all areas of a business, fundamentally changing how you operate and deliver value to customers. It also a cultural change that requires organizations to continually challenge the status quo, experiment, and get comfortable with failure.', 1,1 , 1, getdate())
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES ( 2,N'Customers',N'Customer on boarding', N'home_work', N'/pmsnexus/accounts/myaccounts',N'Customers are onboarded in the system as and when there is a signed SOW.',N'Customers are onboarded in the system as and when there is a signed SOW.', 1,1, 1, getdate())
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES ( 3,N'Projects',N'Project management', N'home_work', N'/pmsnexus/projects/myprojects',N'Projects are created in the system and resources are assigned against each project',N'Projects are created in the system and resources are assigned against each project', 1,1, 1, getdate())
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES ( 4,N'Reports', N'Reports and Dashboard', N'home_work', N'/pmsnexus/reports/reports',N'Summary and quick Insights of the works being done by the individual, team, department, or the organization.',N'Summary and quick Insights of the works being done by the individual, team, department, or the organization.', 1,1, 1, getdate())
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES ( 5,N'Leaves', N'Leave management', N'event', N'/pmsnexus/leaves/leave-individual',N'Leave Management we can manage the employee leaves.',N'Leave Management we can manage the employee leaves.', 1,1, 1, getdate())
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES ( 6,N'Attendance', N'Attendance management', N'event_upcoming', N'/pmsnexus/attendance/attendance-individual',N'Attendance Management we can manage the employee attendance',N'Attendance Management we can manage the employee attendance', 1,1, 1, getdate())
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES ( 7,N'Timesheets',  N'Timesheet management', N'home_work', N'/pmsnexus/timesheets/all-timesheets/my-timesheets',N'Timesheet Management we can manage the employee Timesheet.',N'Timesheet Management we can manage the employee Timesheet.',1,1, 1, getdate())
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES ( 8,N'Organization',  N'User management', N'other_houses', N'/pmsnexus/employees/employee-details',N'User Management we can manage the employee role permissions and user details.',N'User Management we can manage the employee role permissions and user details.',1,1, 1, getdate())
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES ( 9,N'Appraisal', N'Appraisal', N'military_tech', N'/pmsnexus/appraisal/appraisal-card',N'Appraisal.',N'Appraisal.', 1,1, 1, getdate())
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES ( 10,N'ChangeRequest', N'Change Request', N'other_houses', N' ',N'Change Request are created in the system and resources are assigned against each project',N'Change Request are created in the system and resources are assigned against each project', 1,0, 1, getdate())
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES ( 11,N'OrganisationFiles', N'Organisation Files', N'other_houses', N'/pmsnexus/organisationfiles/organizationfiles-details',N'Oraganisation files are managed',N'Oraganisation files are managed', 1,1, 1, getdate())
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES
( 12,N'Exit Management', N'Exit Management', N'other_houses', N'/pmsnexus/exitmanagement/resignationdetails',N'Employee Exit Management',N'Employee Exit Management', 1,1, 1, getdate())
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES
( 13,N'Workday', N'Workday', N'other_houses', N'/pmsnexus/workday',N'Employee Workday',N'Employee Workday', 1,1, 1, getdate())
INSERT [dbo].[Modules] ([ModuleId], [ModuleName], [ModuleDescription], [ModuleIcon],[NavigationURL],[ModuleShortDescription],[ModuleFullDescription],[IsActive],[IsMenu], [CreatedBy], [CreatedOn]) VALUES
( 14,N'Policies & Documents', N'Policies & Documents', N'description', N'/policiesdocuments/policies',N'Policy Management',N'Policy Management', 1,1, 1, getdate())
SET IDENTITY_INSERT [dbo].[Modules] OFF
Go
TRUNCATE TABLE [Features]
GO
SET IDENTITY_INSERT Features ON
GO
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(1,'Create', 'Creating new entry', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(2,'Edit', 'Editing the existing records', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(3,'Delete', 'Deleting the existing records', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(4,'View', 'Viewing the existing records', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(5,'Approve', 'Approve the request', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(6,'Reject', 'Reject or Requst changes the request', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(7,'SPOC', 'Assign the SPOC', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(8,'ResourceAllocation', 'Assign the resouce for existing project', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(9,'CustomerStatus', 'Status report for all Customers', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(10,'ProjectStatus', 'Status report for all projects', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(11,'ResourceBillabilty', 'Report for billability status of all resources ', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(12,'ProjectWiseResourceUtilization', 'Report for Project wise billability status of resources', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(13,'ResourceWiseUtilization', 'Report for Resource Allocation', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(14,'SkillsetWiseResources', 'Report for number of resources based on skillset', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(15,'SkilsetWiseBench', 'Report for resources in bench based on skillset ', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(16,'ProjectWiseTimesheetPlannedVsActual', 'Report for project wise planned vs actual Timesheet', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(17,'ResourceWiseWeeklyTimesheetStatus', 'Timesheet status report for resources on week basis ', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(18,'ResourceWiseTimesheetPlannedVsActual', 'Report for resource wise planned vs actual Timesheet', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(19,'TimesheetConfiguration', 'Configuration for timesheet ', getdate(), 1,'/pmsnexus/timesheets/all-timesheets/timesheet-configuration')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(20,'LeaveConfiguration', 'Configuration for Leave', getdate(), 1,'/pmsnexus/leaves/leave-configuration')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(21,'Holiday', 'Creating Holidays', getdate(), 1,'/pmsnexus/leaves/holiday-details')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(22,'LeaveAdjustment', 'Leave Adjustment', getdate(), 1,'/pmsnexus/leaves/leave-adjustment')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(23,'LeaveIndvidual', 'Individual Leaves', getdate(), 1,'/pmsnexus/leaves/leave-individual')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(24,'LeaveTeam', 'Team Leaves', getdate(), 1,'/pmsnexus/leaves/leave-team')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(25,'AdminTeamLeave', 'Admin Team Leaves', getdate(), 1,'/pmsnexus/leaves/leave-admin')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(26,'AddShift', 'Add New Shift(Attendance)', getdate(), 1,'/pmsnexus/attendance/shiftdetails')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(27,'AttendanceTeam', 'Attendance Details', getdate(), 1,'/pmsnexus/attendance/details')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(28,'AdminAttendanceDetails', 'Attendance Details', getdate(), 1,'/pmsnexus/attendance/details-admin')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(29,'AttendanceIndividual', 'Attendance Individual', getdate(), 1,'/pmsnexus/attendance/attendance-individual')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(30,'Entity', 'Entity', getdate(), 1,'/pmsnexus/appraisal/appraisal-card')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(31,'Objective', 'Objective', getdate(), 1,'/pmsnexus/appraisal/appraisal-card')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(32,'KRA', 'KRA', getdate(), 1,'/pmsnexus/appraisal/appraisal-card')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(33,'Versions', 'Versions', getdate(), 1,'/pmsnexus/appraisal/appraisal-card')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(34,'AppraisalCycle', 'Appraisal Cycle', getdate(), 1,'/pmsnexus/appraisal/appraisal-card')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(35,'EmployeeSync', 'Employee Sync from Azure AD', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(36,'AppraisalIndividual', 'Appraisal Individual Employee View', getdate(), 1,'/pmsnexus/appraisal/appraisal-card')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(37,'AdminLeaveIndvidual', 'Admin Individual Leaves', getdate(), 1,'/pmsnexus/leaves/adminleave-individual')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(38,'AdminAttendanceIndividual', 'Admin Attendance Individual', getdate(), 1,'/pmsnexus/attendance/Adminattendance-individual')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(39,'AttendanceLog', 'Check-in and Check-out Attendance', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(40,'Reports', 'Reports', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(41,'Announcement', 'Upcoming Holiday view', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(42,'Leaves', 'Leave Cards', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(43,'SocialFeeds', 'Social Feeds', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(44,'Nexpedia', 'Nexpedia', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(45,'AbsentSetting', 'Absent Configuration', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(46,'IndividualResignation', 'Individual Resignation', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(47,'TeamResignation', 'Team Resignation', getdate(), 1)
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy)
VALUES(48,'AdminResignation', 'Admin Resignation', getdate(), 1)

INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(49,'DepartmentMaster', 'Department Master', getdate(), 1,'/pmsnexus/employees/department')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(50,'DesignationMaster', 'Designation Master', getdate(), 1,'/pmsnexus/employees/designation')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(51,'BirthdayView', 'Birthday View', getdate(), 1,'/pmsnexus/employees/birthdays')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(52,'WorkAnniversaryView', 'Work Anniversary View', getdate(), 1,'/pmsnexus/employees/work-anniversary')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(53,'SkillMatrix', 'Skill Matrix', getdate(), 1,'/pmsnexus/employees/skill-matrix')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(54,'OrganizationChart', 'Organization Chart', getdate(), 1,'/pmsnexus/employees/org-chart')

INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(55,'MyApprovals', 'My Approvals', getdate(), 1,'/pmsnexus/employees/my-approval')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(56,'MyRequests', 'My Requests', getdate(), 1,'/pmsnexus/employees/my-request')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(57,'RoleAccess', 'Role Access', getdate(), 1,'/pmsnexus/employees/role-permissions')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(58,'EmployeeDetails', 'Employee Detail', getdate(), 1,'/pmsnexus/employees/employee-details')

INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(59,'IndividualWorkday', 'Individual Workday', getdate(), 1,'/pmsnexus/workday')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(60,'TeamLeave', 'Team Leave', getdate(), 1,'/pmsnexus/workday')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(61,'TeamAttendance', 'Team Attendance', getdate(), 1,'/pmsnexus/workday')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(62,'TeamTimesheet', 'Team Timesheet', getdate(), 1,'/pmsnexus/workday')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(63,'TeamWeek', 'Team Week', getdate(), 1,'/pmsnexus/workday')

INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(64,'AdminPolicy', 'Admin Policies', getdate(), 1,'/policiesdocuments/policies')
INSERT INTO Features (FeatureId,FeatureName, FeatureDescription, CreatedOn, CreatedBy,NavigationURL)
VALUES(65,'IndividualPolicy', 'Individual Policy', getdate(), 1,'/policiesdocuments/policies')
GO
SET IDENTITY_INSERT Features OFF
GO
TRUNCATE TABLE [ModuleFeatureMapping]
GO

declare @ModuleId int;
declare @FeatureId int

--Home
set @ModuleId=0 set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'Home';
Select @FeatureId=FeatureId from Features where FeatureName= 'AttendanceLog';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Reports';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Announcement';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Leaves';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'SocialFeeds';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Nexpedia';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)


--Customers
set @ModuleId=0 set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'Customers';
Select @FeatureId=FeatureId from Features where FeatureName= 'Create';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Edit';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Delete';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'View';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Approve';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)

--Projects
set @ModuleId=0 set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'Projects';
Select @FeatureId=FeatureId from Features where FeatureName= 'Create';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Edit';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Delete';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'View';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Approve';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'SPOC';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'ResourceAllocation';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)

--Timesheets
set @ModuleId=0 set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'Timesheets';
Select @FeatureId=FeatureId from Features where FeatureName= 'Create';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Edit';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Delete';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'View';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Approve';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'TimesheetConfiguration';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)

--Reports
set @ModuleId=0;
set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'Reports';
Select @FeatureId=FeatureId from Features where FeatureName= 'CustomerStatus';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'ProjectStatus'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'ResourceBillabilty'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'ProjectWiseResourceUtilization'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'ResourceWiseUtilization'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'SkillsetWiseResources'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'SkilsetWiseBench'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'ProjectWiseTimesheetPlannedVsActual'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'ResourceWiseWeeklyTimesheetStatus'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'ResourceWiseTimesheetPlannedVsActual'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)


--Employees
set @ModuleId=0;
set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'Organization';
Select @FeatureId=FeatureId from Features where FeatureName= 'Create';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Edit';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Delete';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'View';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Approve';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'EmployeeSync'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)

Select @FeatureId=FeatureId from Features where FeatureName= 'DepartmentMaster';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'DesignationMaster';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'BirthdayView';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'WorkAnniversaryView'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)

Select @FeatureId=FeatureId from Features where FeatureName= 'SkillMatrix'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)


Select @FeatureId=FeatureId from Features where FeatureName= 'OrganizationChart'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)

Select @FeatureId=FeatureId from Features where FeatureName= 'MyApprovals'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'MyRequests'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'RoleAccess'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'EmployeeDetails'
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
--Change Request
set @ModuleId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'ChangeRequest';
Select @FeatureId=FeatureId from Features where FeatureName= 'Create';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Edit';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Delete';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'View';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Approve';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)

--Leaves
set @ModuleId=0;
set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'Leaves';
Select @FeatureId=FeatureId from Features where FeatureName= 'LeaveConfiguration';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Holiday';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'LeaveAdjustment';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'LeaveIndvidual';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'AdminLeaveIndvidual';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'LeaveTeam';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'AdminTeamLeave';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)

--Attendance
set @ModuleId=0;
set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'Attendance';
Select @FeatureId=FeatureId from Features where FeatureName= 'AddShift';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'AttendanceTeam';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'AdminAttendanceDetails';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'AttendanceIndividual';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'AdminAttendanceIndividual';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'AbsentSetting';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)

--Appraisal
set @ModuleId=0;
set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'Appraisal';
Select @FeatureId=FeatureId from Features where FeatureName= 'Entity';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Objective';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'KRA';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Versions';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'AppraisalCycle';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'AppraisalIndividual';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
--Select @FeatureId=FeatureId from Features where FeatureName= 'AppraisalManager';
--INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
--VALUES(@ModuleId, @FeatureId, getdate(), 1)
GO


--OrganisationFiles
declare @ModuleId int;
declare @FeatureId int
set @ModuleId=0 set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'OrganisationFiles';
Select @FeatureId=FeatureId from Features where FeatureName= 'Create';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Edit';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'Delete';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'View';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)

--ExitManagement
--declare @ModuleId int;
--declare @FeatureId int
set @ModuleId=0 set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'Exit Management';
Select @FeatureId=FeatureId from Features where FeatureName= 'IndividualResignation';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'TeamResignation';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'AdminResignation';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)

--Workday
set @ModuleId=0;
set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'Workday';
Select @FeatureId=FeatureId from Features where FeatureName= 'IndividualWorkday';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'TeamLeave';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'TeamAttendance';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'TeamTimesheet';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'TeamWeek';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)

--Policy
set @ModuleId=0;
set @FeatureId=0;
Select @ModuleId=ModuleId from Modules where ModuleName= 'Policies & Documents';
Select @FeatureId=FeatureId from Features where FeatureName= 'AdminPolicy';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)
Select @FeatureId=FeatureId from Features where FeatureName= 'IndividualPolicy';
INSERT INTO ModuleFeatureMapping (ModuleId, FeatureId, CreatedOn, CreatedBy)
VALUES(@ModuleId, @FeatureId, getdate(), 1)




