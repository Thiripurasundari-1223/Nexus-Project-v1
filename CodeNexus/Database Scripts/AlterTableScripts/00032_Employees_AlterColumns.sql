USE [PMSNexus_Employees]
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees' AND COLUMN_NAME = 'Mobile')
	ALTER TABLE Employees ADD Mobile VARCHAR(50) NULL
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees' AND COLUMN_NAME = 'JobTitle')
	ALTER TABLE Employees ADD JobTitle VARCHAR(500) NULL
GO