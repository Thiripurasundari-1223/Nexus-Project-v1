USE [PMSNexus_Employees]
Go
Declare @ModuleId int,@DepartmentId int, @RoleId int,@EmployeeCategoryId int,@EmployeeTypeId int,@LocationId int;
Select @RoleId=RoleId from [dbo].[Roles] where RoleName='Admin';
Select @DepartmentId= DepartmentId from [dbo].[Department]  where DepartmentName='Engineering';
Select @EmployeeTypeId= EmployeesTypeId from [dbo].[EmployeesType]  where EmployeesType='Permanent';
Select @LocationId= LocationId from [dbo].[EmployeeLocation]  where [Location]='Chennai';
Select @EmployeeCategoryId=EmployeeCategoryId from [dbo].[EmployeeCategory] where DepartmentId=@DepartmentId AND EmployeeCategoryName ='Individual';
Select @ModuleId=ModuleId from [dbo].[Modules] where modulename='Employees';

INSERT INTO Employees(FirstName, LastName, EmailAddress, EmployeeTypeId, DepartmentId, RoleId, IsActive, CreatedOn, CreatedBy, LocationId, EmployeeCategoryId)
VALUES('Admin', 'Nexus', 'srvnexusadmin@tvsnext.io', @EmployeeTypeId, @DepartmentId, @RoleId, 1, GETDATE(), 1, @LocationId, @EmployeeCategoryId)

Insert into [dbo].[RolePermissions]
([ModuleId],[RoleId],[FeatureId],[IsEnabled],[CreatedOn],[CreatedBy])
Select @ModuleId,@RoleId,FeatureId,1,getdate(),1 from features where FeatureName in ('Create','Edit','View','EmployeeSync')
