USE [PMSNexus_Employees]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='CompensationDetail')
	CREATE TABLE [dbo].[CompensationDetail](
		[CTCId] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeID] [int]  NULL,
		[EffectiveFromDate] [datetime] NULL,
		[MonthlyCurrentCTC] [DECIMAL](18,2) NULL,
		[MonthlyBasicPay] [DECIMAL](18,2) NULL,
		[MonthlyHRA] [DECIMAL](18,2) NULL,
		[MonthlySatutoryBonus] [DECIMAL](18,2) NULL,
		[MonthlyFBP] [DECIMAL](18,2) NULL,
		[MonthlyGrossPay] [DECIMAL](18,2) NULL,
		[MonthlyPF] [DECIMAL](18,2) NULL,
		[MonthlyESI] [DECIMAL](18,2) NULL,
		[MonthlyCBP] [DECIMAL](18,2) NULL,
		[MonthlyCBPPercentage] [DECIMAL](18,2) NULL,
		[AnnualCurrentCTC] [DECIMAL](18,2) NULL,
		[AnnualBasicPay] [DECIMAL](18,2) NULL,
		[AnnualHRA] [DECIMAL](18,2) NULL,
		[AnnualSatutoryBonus] [DECIMAL](18,2) NULL,
		[AnnualFBP] [DECIMAL](18,2) NULL,
		[AnnualGrossPay] [DECIMAL](18,2) NULL,
		[AnnualPF] [DECIMAL](18,2) NULL,
		[AnnualESI] [DECIMAL](18,2) NULL,
		[AnnualCBP] [DECIMAL](18,2) NULL,
		[AnnualCBPPercentage] [DECIMAL](18,2) NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[CTCId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
 