USE [PMSNexus_Timesheets]
BEGIN
alter table [dbo].[TimesheetConfigurationDetails] alter column [TimesheetSubmissionTime] Time(0) NULL
END
GO