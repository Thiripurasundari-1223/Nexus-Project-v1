IF Not Exists(Select 1 from INFORMATION_SCHEMA.COLUMNS where COLUMN_NAME='IAM' AND   TABLE_NAME='RoleSetup')
Begin
Alter Table RoleSetup Add IAM int Default(0)
End
GO
Update RoleSetup SET IAM=0; 
Update RoleSetup SET IAM=1 where RoleID=(Select RoleID From Roles Where RoleName='Admin')
GO