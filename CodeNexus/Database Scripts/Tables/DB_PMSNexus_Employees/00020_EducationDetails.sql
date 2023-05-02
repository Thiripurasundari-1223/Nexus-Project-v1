USE [PMSNexus_Employees]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EducationDetail')
	CREATE TABLE [dbo].[EducationDetail](
		[EducationDetailId] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeId] [int]  NULL,
		[EducationTypeId] [int] NULL,
		[InstitutionName] [varchar](50) NULL,
		[BoardId] [int] NULL,
		[Degree] [varchar](100) NULL,
		[Specialization] [varchar](100) NULL,
		[CertificateName] [varchar](50) NULL,
		[YearOfCompletion] [datetime] NULL,
		[MarkPercentage] [DECIMAL](18,2) NULL,
		[ExpiryYear] [int] NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[EducationDetailId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
 