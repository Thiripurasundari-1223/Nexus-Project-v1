USE [PMSNexus_ExitManagement]
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeResignationDetails' AND COLUMN_NAME = 'EmployeeDesignationId')
BEGIN
    EXEC sp_rename 'EmployeeResignationDetails.EmployeeDesignationId', 'EmployeeDesignation';
   ALTER TABLE EmployeeResignationDetails   
   alter COLUMN EmployeeDesignation nvarchar(max);
END