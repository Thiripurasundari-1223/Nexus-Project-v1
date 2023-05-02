
USE [PMSNexus_Employees]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='EmployeesPersonalInfo' AND COLUMN_NAME = 'IsJoiningBonus')
BEGIN
		ALTER TABLE [dbo].[EmployeesPersonalInfo] ADD [IsJoiningBonus] [bit] NULL
		ALTER TABLE [dbo].[EmployeesPersonalInfo] ADD [JoiningBonusAmmount] [DECIMAL](18,2) NULL
		ALTER TABLE [dbo].[EmployeesPersonalInfo] ADD [JoiningBonusCondition] [varchar] (max) NULL
		ALTER TABLE [dbo].[EmployeesPersonalInfo] ADD [AccountHolderName] [varchar] (max) NULL
		ALTER TABLE [dbo].[EmployeesPersonalInfo] ADD [BankName] [varchar] (max) NULL
		ALTER TABLE [dbo].[EmployeesPersonalInfo] ADD [IFSCCode] [varchar](50) NULL
		ALTER TABLE [dbo].[EmployeesPersonalInfo] ADD [AccountNumber] [DECIMAL](20,2)  NULL
		
		ALTER TABLE [dbo].[CompensationDetail] drop column IsJoiningBonus
		ALTER TABLE [dbo].[CompensationDetail] drop column JoiningBonusAmmount
		ALTER TABLE [dbo].[CompensationDetail] drop column JoiningBonusCondition
		ALTER TABLE [dbo].[CompensationDetail] drop column AccountHolderName
		ALTER TABLE [dbo].[CompensationDetail] drop column BankName
		ALTER TABLE [dbo].[CompensationDetail] drop column IFSCCode
		ALTER TABLE [dbo].[CompensationDetail] drop column AccountNumber
END
GO
