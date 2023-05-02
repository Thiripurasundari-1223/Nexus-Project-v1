USE [PMSNexus_Employees]
GO

IF Not Exists(Select 1 from INFORMATION_SCHEMA.COLUMNS where COLUMN_NAME='IsEnableBUAccountable' AND  TABLE_NAME='Department')
ALTER TABLE Department ADD IsEnableBUAccountable BIT 
GO

UPDATE Department SET IsEnableBUAccountable = 0 WHERE IsEnableBUAccountable IS NULL
GO

UPDATE Department SET IsEnableBUAccountable = 1 WHERE DepartmentName in('Engineering','Intelligence', 'Corporate') AND IsEnableBUAccountable != 1
GO