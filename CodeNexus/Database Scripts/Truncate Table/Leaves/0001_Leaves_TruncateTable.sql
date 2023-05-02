use [PMSNexus_Leaves]
/*
1)need to remove supporting documents table in notification
2)Delete supporting documents physical file
3)Delete already mapped shift

truncate table LeaveTypes
truncate table LeaveApplicable
truncate table LeaveDepartment
truncate table LeaveDesignation
truncate table LeaveLocation
truncate table LeaveRole
truncate table EmployeeLeaveDetails
truncate table LeaveCarryForward
truncate table LeaveEntitlement
truncate table LeaveRestrictions
truncate table EmployeeApplicableLeave
truncate table Leaves
truncate table ProRateMonthDetails
truncate table EmployeeGrantLeaveApproval
truncate table GrantLeaveApproval
truncate table SpecificEmployeeDetailLeave
truncate table LeaveTakenTogether
truncate table LeaveGrantRequestDetails
truncate table LeaveGrantDocumentDetails
truncate table AppliedLeaveDetails
truncate table LeaveAdjustmentDetails
truncate table Holiday
truncate table HolidayDepartment
truncate table HolidayLocation
truncate table HolidayShift

delete [pmsnexus_notifications].[supportingdocuments] where sourcetype='leaves'


*/

