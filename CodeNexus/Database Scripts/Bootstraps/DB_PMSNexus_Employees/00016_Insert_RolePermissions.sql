USE PMSNexus_Employees
GO
TRUNCATE TABLE [dbo].[RolePermissions]
Go
SET IDENTITY_INSERT RolePermissions ON
GO
declare @moduleId int;
declare @roleId int;
--Select moduleid from modules where ModuleName='Employees'
Select @moduleId=moduleid from modules where ModuleName='Organization'
--Select roleid from systemroles where rolename='super admin'
Select @roleId=roleid from systemroles where rolename='Super Admin'
Insert into RolePermissions (RolePermissionId,ModuleId,RoleId,FeatureId,IsEnabled,CreatedOn) VALUES
(1,@moduleId,@roleId,1,1,getdate())
Insert into RolePermissions (RolePermissionId,ModuleId,RoleId,FeatureId,IsEnabled,CreatedOn) VALUES
(2,@moduleId,@roleId,2,1,getdate())
Insert into RolePermissions (RolePermissionId,ModuleId,RoleId,FeatureId,IsEnabled,CreatedOn) VALUES
(3,@moduleId,@roleId,3,1,getdate())
Insert into RolePermissions (RolePermissionId,ModuleId,RoleId,FeatureId,IsEnabled,CreatedOn) VALUES
(4,@moduleId,@roleId,4,1,getdate())
Insert into RolePermissions (RolePermissionId,ModuleId,RoleId,FeatureId,IsEnabled,CreatedOn) VALUES
(5,@moduleId,@roleId,57,1,getdate())
GO
SET IDENTITY_INSERT RolePermissions OFF
