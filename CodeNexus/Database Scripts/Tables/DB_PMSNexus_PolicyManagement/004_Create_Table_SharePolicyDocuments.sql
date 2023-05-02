USE [PMSNexus_PolicyManagement]
GO

CREATE TABLE [dbo].[SharePolicyDocuments](
	[SharePolicyDocumentId] [int] IDENTITY(1,1) NOT NULL,
	[PolicyDocumentId] [int] NULL,
	[DepartmentId] [int] NULL,
	[LocationId] [int] NULL,
	[RoleId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_SharePolicyDocument] PRIMARY KEY CLUSTERED 
(
	[SharePolicyDocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO