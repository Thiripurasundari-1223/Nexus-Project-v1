USE [PMSNexus_Employees]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EmployeeDocument')
	CREATE TABLE [dbo].[EmployeeDocument](
		[EmployeeDocumentId] [int] IDENTITY(1,1) NOT NULL,
		[EmployeeID] [int]  NULL,
		[SourceId] [int] NULL,
		[SourceType][varchar](50) NULL,
		[DocumentType] [varchar](50) NULL,
        [DocumentName] [varchar](50) NULL,
		[DocumentPath] [varchar](max) NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[EmployeeDocumentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
 

