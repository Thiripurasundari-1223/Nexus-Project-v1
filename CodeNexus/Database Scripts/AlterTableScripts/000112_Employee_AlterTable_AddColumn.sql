
USE [PMSNexus_Employees]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Employees' AND COLUMN_NAME = 'ContractEndDate')
BEGIN
	ALTER TABLE [dbo].[Employees] ADD [ContractEndDate] [datetime] NULL
	ALTER TABLE [dbo].[Employees] ADD [CurrentWorkLocationId] [int] NULL
	ALTER TABLE [dbo].[Employees] ADD [CurrentWorkPlaceId] [int] NULL
	ALTER TABLE [dbo].[Employees] ADD [SpecialAbilityId] [int] NULL
	ALTER TABLE [dbo].[Employees] ADD [SpecialAbilityRemark] [varchar](max) NULL
	ALTER TABLE [dbo].[Employees] ADD [DesignationEffectiveFrom] [datetime] NULL
	ALTER TABLE [dbo].[Employees] ADD [ActualConfirmationDate] [datetime] NULL
	ALTER TABLE [dbo].[Employees] ADD [MergerDate] [datetime] NULL
	ALTER TABLE [dbo].[Employees] ADD [ProbationExtension] [datetime] NULL
	ALTER TABLE [dbo].[Employees] ADD [TotalExperience] [decimal](18,2) NULL
	ALTER TABLE [dbo].[Employees] ADD [PreviousExperience] [decimal](18,2) NULL
	ALTER TABLE [dbo].[Employees] ADD [RetirementDate] [datetime] NULL
	ALTER TABLE [dbo].[Employees] ADD [Entity] [int] NULL
	ALTER TABLE [dbo].[Employees] ADD [SourceOfHireId] [int] NULL
	ALTER TABLE [dbo].[Employees] ADD [ProfilePicture] [nvarchar](max) NULL
END
GO
