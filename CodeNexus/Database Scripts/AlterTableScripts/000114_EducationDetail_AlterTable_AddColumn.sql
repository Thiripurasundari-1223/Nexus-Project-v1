
USE [PMSNexus_Employees]
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EducationDetail')
BEGIN
		ALTER TABLE [dbo].[EducationDetail] ADD [Degree] nvarchar(100) NULL
		ALTER TABLE [dbo].[EducationDetail] ADD [Specialization] nvarchar(100)  NULL

		ALTER TABLE [dbo].[EducationDetail] drop column DegreeId
		ALTER TABLE [dbo].[EducationDetail] drop column SpecializationId
END
GO
