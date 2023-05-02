USE [PMSNexus_ExitManagement]
GO

truncate table [dbo].[EmployeeResignationDetails]
truncate table [dbo].[ReasonLeavingPosition]
truncate table [dbo].[ResignationApprovalStatus]
truncate table [dbo].[ResignationFeedbackToManagement]
truncate table [dbo].[ResignationInterview]
truncate table ResignationChecklist
truncate table ManagerChecklist
truncate table PMOChecklist
truncate table ITChecklist
truncate table AdminChecklist
truncate table FinanceChecklist
truncate table HRChecklist

update PMSNexus_Employees..Employees set DateOfRelieving=null, ResignationDate=null,[ResignationReason]=null,[ResignationStatus]=null,isresign=null
