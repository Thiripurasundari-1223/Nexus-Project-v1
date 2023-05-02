USE [PMSNexus_ExitManagement]
GO
CREATE TABLE [dbo].[AdminCheckList](
    [AdminCheckListId]  [int] IDENTITY(1,1) NOT NULL,
	[ResignationChecklistId] [int]  NULL,
	[ApprovedBy] [int]  NULL,
	[Status] [nvarchar](100) NULL,
    [IdentityCardId] [int] NULL,
	[IdentityCardRemark] [nvarchar](max) NULL,
    [CabinKeysID] [int] NULL,
	[CabinKeysRemark] [nvarchar](max) NULL,
    [TravelCardId] [int] NULL,
	[TravelCardRemark] [nvarchar](max) NULL,
    [BusinessCardsId] [int] NULL,
	[BusinessCardsRemark] [nvarchar](max) NULL,
	[LibraryBooksId] [int] NULL,
	[LibraryBooksRemark] [nvarchar](max) NULL,
	[CompanyMobileId] [int] NULL,
	[CompanyMobileRemark] [nvarchar](max) NULL,
	[OtherRecovery] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminCheckListId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
