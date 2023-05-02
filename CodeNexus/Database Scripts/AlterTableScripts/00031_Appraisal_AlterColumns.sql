
use [PMSNexus_Appraisal]

alter table [dbo].[AppConstants] alter column [CREATED_BY] Int
alter table [dbo].[AppConstants] alter column [UPDATED_BY] Int

alter table [dbo].[AppConstantType] alter column [CREATED_BY] Int
alter table [dbo].[AppConstantType] alter column [UPDATED_BY] Int

alter table [dbo].[AppraisalMaster] alter column [CREATED_BY] Int
alter table [dbo].[AppraisalMaster] alter column [UPDATED_BY] Int

alter table [dbo].[ContinuousFeedBack] alter column [CREATED_BY] Int
alter table [dbo].[ContinuousFeedBack] alter column [UPDATED_BY] Int

alter table [dbo].[ConversationTags] alter column [CREATED_BY] Int
alter table [dbo].[ConversationTags] alter column [UPDATED_BY] Int

alter table [dbo].[EmployeeAppraisalConversation] alter column [CREATED_BY] Int
alter table [dbo].[EmployeeAppraisalConversation] alter column [UPDATED_BY] Int

alter table [dbo].[EmployeeAppraisalMaster] alter column [CREATED_BY] Int
alter table [dbo].[EmployeeAppraisalMaster] alter column [UPDATED_BY] Int

alter table [dbo].[EmployeeGroupRating] alter column [CREATED_BY] Int
alter table [dbo].[EmployeeGroupRating] alter column [UPDATED_BY] Int

alter table [dbo].[EmployeeGroupSelection] alter column [CREATED_BY] Int
alter table [dbo].[EmployeeGroupSelection] alter column [UPDATED_BY] Int

alter table [dbo].[EmployeeKeyResultConversation] alter column [CREATED_BY] Int
alter table [dbo].[EmployeeKeyResultConversation] alter column [UPDATED_BY] Int

alter table [dbo].[EmployeeKeyResultConversationBackup] alter column [CREATED_BY] Int
alter table [dbo].[EmployeeKeyResultConversationBackup] alter column [UPDATED_BY] Int

alter table [dbo].[EmployeeKeyResultRating] alter column [CREATED_BY] Int
alter table [dbo].[EmployeeKeyResultRating] alter column [UPDATED_BY] Int

alter table [dbo].[EmployeeObjectiveRating] alter column [CREATED_BY] Int
alter table [dbo].[EmployeeObjectiveRating] alter column [UPDATED_BY] Int

alter table [dbo].[EntityMaster] alter column [CREATED_BY] Int
alter table [dbo].[EntityMaster] alter column [UPDATED_BY] Int

alter table [dbo].[KeyResultMaster] alter column [CREATED_BY] Int
alter table [dbo].[KeyResultMaster] alter column [UPDATED_BY] Int

alter table [dbo].[Notification] alter column [CREATED_BY] Int
alter table [dbo].[Notification] alter column [UPDATED_BY] Int

alter table [dbo].[ObjectiveMaster] alter column [CREATED_BY] Int
alter table [dbo].[ObjectiveMaster] alter column [UPDATED_BY] Int

alter table [dbo].[SDSTokenDTLS] alter column [SCD_CREATED_BY] Int
alter table [dbo].[SDSTokenDTLS] alter column [SCD_UPDATED_BY] Int

alter table [dbo].[VersionBenchMarks] alter column [CREATED_BY] Int
alter table [dbo].[VersionBenchMarks] alter column [UPDATED_BY] Int

alter table [dbo].[VersionDepartmentRoleMapping] alter column [CREATED_BY] Int
alter table [dbo].[VersionDepartmentRoleMapping] alter column [UPDATED_BY] Int

alter table [dbo].[VersionDepartmentRoleObjective] alter column [CREATED_BY] Int
alter table [dbo].[VersionDepartmentRoleObjective] alter column [UPDATED_BY] Int

alter table [dbo].[VersionDepartments] alter column [CREATED_BY] Int
alter table [dbo].[VersionDepartments] alter column [UPDATED_BY] Int

alter table [dbo].[VersionKeyResults] alter column [CREATED_BY] Int
alter table [dbo].[VersionKeyResults] alter column [UPDATED_BY] Int

alter table [dbo].[VersionKeyResultsGroup] alter column [CREATED_BY] Int
alter table [dbo].[VersionKeyResultsGroup] alter column [UPDATED_BY] Int

alter table [dbo].[VersionKeyResultsGroupDetails] alter column [CREATED_BY] Int
alter table [dbo].[VersionKeyResultsGroupDetails] alter column [UPDATED_BY] Int

alter table [dbo].[VersionMaster] add VERSION_CODE Nvarchar(100)

alter table [dbo].[VersionMaster] alter column [CREATED_BY] Int
alter table [dbo].[VersionMaster] alter column [UPDATED_BY] Int

alter table [dbo].[VersionObjective] alter column [CREATED_BY] Int
alter table [dbo].[VersionObjective] alter column [UPDATED_BY] Int