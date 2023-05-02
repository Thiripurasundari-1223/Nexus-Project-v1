USE [PMSNexus_Leaves]
GO

ALTER TABLE LeaveTypes ALTER COLUMN LeaveAccruedNoOfDays DECIMAL(18, 2)
GO

ALTER TABLE LeaveEntitlement ALTER COLUMN MaxLeaveAvailedDays DECIMAL(18, 2)
ALTER TABLE LeaveEntitlement ALTER COLUMN MaxLimitDays DECIMAL(18, 2)
ALTER TABLE LeaveEntitlement ALTER COLUMN RestrictLeaveApplicationDays DECIMAL(18, 2)
ALTER TABLE LeaveEntitlement ALTER COLUMN DocumentMandatoryDays DECIMAL(18, 2)
ALTER TABLE LeaveEntitlement ALTER COLUMN NoOfDays DECIMAL(18, 2)
GO

ALTER TABLE LeaveRestrictions ALTER COLUMN WeekendCountAfterDays DECIMAL(18, 2)
ALTER TABLE LeaveRestrictions ALTER COLUMN HolidayCountAfterDays DECIMAL(18, 2)
ALTER TABLE LeaveRestrictions ALTER COLUMN DaysInAdvance DECIMAL(18, 2)
ALTER TABLE LeaveRestrictions ALTER COLUMN AllowRequestNextDays DECIMAL(18, 2)
ALTER TABLE LeaveRestrictions ALTER COLUMN DatesAppliedAdvance DECIMAL(18, 2)
ALTER TABLE LeaveRestrictions ALTER COLUMN MinimumLeavePerApplication DECIMAL(18, 2)
ALTER TABLE LeaveRestrictions ALTER COLUMN MinimumGapTwoApplication DECIMAL(18, 2)
ALTER TABLE LeaveRestrictions ALTER COLUMN MinimumConsecutiveDays DECIMAL(18, 2)
ALTER TABLE LeaveRestrictions ALTER COLUMN MinimumNoOfApplicationsPeriod DECIMAL(18, 2)
ALTER TABLE LeaveRestrictions ALTER COLUMN EnableFileUpload DECIMAL(18, 2)
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'AllowPastDates')
	ALTER TABLE LeaveRestrictions ADD AllowPastDates BIT NULL
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'AllowFutureDates')
	ALTER TABLE LeaveRestrictions ADD AllowFutureDates BIT NULL
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'IsAllowRequestNextDays')
	ALTER TABLE LeaveRestrictions ADD IsAllowRequestNextDays BIT NULL
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='LeaveRestrictions' AND COLUMN_NAME = 'IsToBeApplied')
	ALTER TABLE LeaveRestrictions ADD IsToBeApplied BIT NULL
GO

UPDATE LeaveRestrictions SET AllowPastDates = 0, AllowFutureDates = 0, IsAllowRequestNextDays = 0, IsToBeApplied = 0
GO