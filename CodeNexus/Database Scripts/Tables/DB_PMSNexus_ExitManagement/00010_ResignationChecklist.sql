USE [PMSNexus_ExitManagement]
GO
CREATE TABLE [dbo].[ResignationChecklist](
	[ResignationChecklistId] [int] IDENTITY(1,1) NOT NULL,
	[ResignationDetailsId] [int] NULL,
	[EmployeeID] [int] NULL,
	[ManagerId] [int] NULL,
	[IsAgreeCheckList] [bit]  NULL,
	[ManagerStatus]  [nvarchar](100) NULL,
	[PMOStatus]  [nvarchar](100) NULL,
	[ITStatus]  [nvarchar](100) NULL,
	[AdminStatus]  [nvarchar](100) NULL,
	[FinanceStatus]  [nvarchar](100) NULL,
	[HRStatus]  [nvarchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ResignationChecklistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO