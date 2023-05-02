USE [PMSNexus_ExitManagement]
GO
CREATE TABLE [dbo].[ITCheckList](
    [ITCheckListId]  [int] IDENTITY(1,1) NOT NULL,
	[ResignationChecklistId] [int]  NULL,
	[ApprovedBy] [int]  NULL,
	[Status] [nvarchar](100) NULL,
    [LoginDisabledId] [int] NULL,
	[LoginDisabledRemark] [nvarchar](max) NULL,
    [MailID] [int] NULL,
	[RoutedToRemark] [nvarchar](max) NULL,
    [BiometricAccessTerminationId] [int] NULL,
	[BiometricAccessTerminationRemark] [nvarchar](max) NULL,
    [SystemAssetsRecoveredId] [int] NULL,
	[SystemAssetsRecoveredRemark] [nvarchar](max) NULL,
	[DATAcardReturnedId] [int] NULL,
	[DATAcardReturnedRemark] [nvarchar](max) NULL,
	[DamageRecoveryId] [int] NULL,
	[DamageRecoveryRemark] [nvarchar](max) NULL,
	[MacAddressRemovalId] [int] NULL,
	[MacAddressRemovalRemark] [nvarchar](max) NULL,
	[DataBackUpId] [int] NULL,
	[DataBackUpRemark] [nvarchar](max) NULL,
	[UserLaptopDataSize] [nvarchar](max) NULL,
	[UserLaptopDataSizeRemark] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ITCheckListId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
