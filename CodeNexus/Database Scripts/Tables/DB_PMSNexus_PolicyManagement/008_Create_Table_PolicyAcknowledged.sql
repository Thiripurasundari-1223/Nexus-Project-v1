USE [PMSNexus_PolicyManagement]
GO

CREATE TABLE [dbo].[PolicyAcknowledged](
	[PolicyAcknowledgedId] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NULL,
	[PolicyDocumentId] [int] NULL,
	[AcknowledgedAt] [datetime] NULL,
	[AcknowledgedBy] [int] NULL,
	[AcknowledgedStatus] [varchar](50) NULL,
 CONSTRAINT [PK_PolicyAcknowledged] PRIMARY KEY CLUSTERED 
(
	[PolicyAcknowledgedId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO