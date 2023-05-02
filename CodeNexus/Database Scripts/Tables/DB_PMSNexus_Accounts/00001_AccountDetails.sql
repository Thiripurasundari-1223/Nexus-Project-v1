IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AccountDetails')
CREATE TABLE [dbo].[AccountDetails](
AccountId [int] IDENTITY(1,1) NOT NULL,
AccountName [varchar](2000) NULL,
AccountDescription [varchar](2000) NULL,
AccountManagerId [int] NULL,
AccountTypeId [int] NULL,
AccountLocation [varchar](2000) NULL,
OfficeAddress [varchar](2000) NULL,
CountryId [int] NULL,
StateId [int] NULL,
PostalCode [varchar](50) NULL,
City [varchar](250) NULL,
WebSite [varchar](2000) NULL,
BillingCycleFrequenyId [int] NULL,
PANNumber [varchar](2000) NULL,
TANNumber [varchar](2000) NULL,
GSTNumber [varchar](2000) NULL,
CompanyRegistrationNumber [varchar](2000) NULL,
DirectorFirstName [varchar](250) NULL, 
DirectorLastName [varchar](250) NULL,
DirectorPhoneNumber [varchar](2000) NULL,
TaxcertificateOfTheRespectiveCounty [varchar](2000) NULL,
EntityRegistrationDocuments [varchar](2000) NULL,
Documents [varchar](2000) NULL,
AccountStatus [varchar](2000) NULL,
AccountApprovedDate DateTime NULL,
AdditionalComments [varchar](2000) NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
[IsDraft] [bit] NULL,
[FinanceManagerId] [int] NULL,
[AccountStatusCode] [varchar](100) NULL,
[AccountChanges] [varchar](max) NULL,
[FormattedAccountId] [varchar] (250) NULL,
PRIMARY KEY CLUSTERED 
	(
		[AccountId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO