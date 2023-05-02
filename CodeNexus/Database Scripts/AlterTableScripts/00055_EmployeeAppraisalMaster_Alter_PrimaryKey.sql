
USE [PMSNexus_Appraisal]
GO
DECLARE @table NVARCHAR(512), @sql NVARCHAR(MAX);

SELECT @table = N'dbo.EmployeeAppraisalMaster';

SELECT @sql = 'ALTER TABLE ' + @table 
    + ' DROP CONSTRAINT ' + name + ';'
    FROM sys.key_constraints
    WHERE [type] = 'PK'
    AND [parent_object_id] = OBJECT_ID(@table);

EXEC sp_executeSQL @sql;

Go

alter table dbo.EmployeeAppraisalMaster
add primary key ([APP_CYCLE_ID],[EMPLOYEE_ID])
