

TRUNCATE TABLE [dbo].[LeaveMaxLimitAction]
GO

Insert into [dbo].[LeaveMaxLimitAction] (LeaveMaxLimitActionName,CreatedOn,CreatedBy) values ('Carry forward balance to Next year',GETDATE(),1)
Insert into [dbo].[LeaveMaxLimitAction] (LeaveMaxLimitActionName,CreatedOn,CreatedBy) values ('Cary forward max limitand lapse remaining',GETDATE(),1)
Insert into [dbo].[LeaveMaxLimitAction] (LeaveMaxLimitActionName,CreatedOn,CreatedBy) values ('Cary forward max limit and reimbursement remaining',GETDATE(),1)
Insert into [dbo].[LeaveMaxLimitAction] (LeaveMaxLimitActionName,CreatedOn,CreatedBy) values ('Reimburse all at the end of the year',GETDATE(),1)
Insert into [dbo].[LeaveMaxLimitAction] (LeaveMaxLimitActionName,CreatedOn,CreatedBy) values ('Reimburse max limit and lapse remaining',GETDATE(),1)
Insert into [dbo].[LeaveMaxLimitAction] (LeaveMaxLimitActionName,CreatedOn,CreatedBy) values ('Reimburse max limit and carry forward remaining',GETDATE(),1)
Insert into [dbo].[LeaveMaxLimitAction] (LeaveMaxLimitActionName,CreatedOn,CreatedBy) values ('Lapse all at the end of the year',GETDATE(),1)
GO