USE [PMSNexus_Accounts]
GO

CREATE TABLE [dbo].[VersionCustomerContactDetails](
	[VersionCustomerContactDetailId] [int] IDENTITY(1,1) NOT NULL,
	[VersionId] [int] NOT NULL,
	[ContactPersonFirstName] [varchar](250) NULL,
	[ContactPersonLastName] [varchar](250) NULL,
	[ContactPersonPhoneNumber] [varchar](2000) NULL,
	[ContactPersonEmailAddress] [varchar](2000) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[DesignationName] [nvarchar](500) NULL,
	[CountryId] [int] NULL,
	[AddressDetail] [nvarchar](2000) NULL,
	[CityName] [nvarchar](500) NULL,
	[StateId] [int] NULL,
	[Postalcode] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[VersionCustomerContactDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

