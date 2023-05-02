USE PMSNexus_Appraisal
GO
declare @AppConstantTypeId int
select @AppConstantTypeId=APP_CONSTANT_TYPE_ID from [dbo].[AppConstantType] where APP_CONSTANT_TYPE_DESC='Type';

select @AppConstantTypeId=APP_CONSTANT_TYPE_ID from [dbo].[AppConstantType] where APP_CONSTANT_TYPE_DESC='Appraisal Status';
INSERT INTO [dbo].[AppConstants] (APP_CONSTANT_TYPE_ID,APP_CONSTANT_TYPE_VALUE,CREATED_BY,CREATED_DATE,UPDATED_BY,UPDATED_DATE)
VALUES (@AppConstantTypeId,'Self-Appraisal in Progress-Feedback',1,GETDATE(),1,GETDATE())
INSERT INTO [dbo].[AppConstants] (APP_CONSTANT_TYPE_ID,APP_CONSTANT_TYPE_VALUE,CREATED_BY,CREATED_DATE,UPDATED_BY,UPDATED_DATE)
VALUES (@AppConstantTypeId,'Self-Appraisal Completed-Feedback',1,GETDATE(),1,GETDATE())
GO