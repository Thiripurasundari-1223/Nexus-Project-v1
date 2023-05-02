USE [PMSNexus_ExitManagement]
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'Address')
BEGIN
    EXEC sp_rename 'EmployeeResignationDetails.Address', 'AddressLine1';
   ALTER TABLE EmployeeResignationDetails   
   alter COLUMN AddressLine1 nvarchar(max);
END