
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='DepartmentReportMapping')
CREATE TABLE [dbo].[DepartmentReportMapping](
	[DepartmentReportMappingId] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentId] [int] NULL,
	[EmployeeCategoryId] [int] NULL,
	[ReportId] [int] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_DepartmentReportMapping] PRIMARY KEY CLUSTERED 
(
	[DepartmentReportMappingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


