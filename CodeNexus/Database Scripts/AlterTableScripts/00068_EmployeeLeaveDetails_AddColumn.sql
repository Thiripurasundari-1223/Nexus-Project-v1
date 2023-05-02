USE [PMSNexus_Leaves]
IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeLeaveDetails' AND COLUMN_NAME = 'TotalLeave')
BEGIN
	ALTER TABLE EmployeeLeaveDetails DROP COLUMN TotalLeave
	END
	IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeeLeaveDetails' AND COLUMN_NAME = 'AdjustmentBalanceLeave' AND COLUMN_NAME = 'AdjustmentEffectiveFromDate')
	BEGIN
	ALTER TABLE EmployeeLeaveDetails ADD AdjustmentBalanceLeave DECIMAL(18, 2) NULL
	ALTER TABLE EmployeeLeaveDetails ADD AdjustmentEffectiveFromDate [datetime] NULL
END
GO
