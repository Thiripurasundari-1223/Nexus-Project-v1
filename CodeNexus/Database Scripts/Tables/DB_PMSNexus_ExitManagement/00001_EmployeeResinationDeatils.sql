CREATE DATABASE PMSNexus_ExitManagement;

USE [PMSNexus_ExitManagement]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EmployeeResignationDetails')
    CREATE TABLE [dbo].[EmployeeResignationDetails] (
        [EmployeeResignationDetailsId] [int] IDENTITY(1,1) NOT NULL,
        [EmployeeId] [int] NULL,
        [EmployeeDesignationId] [int] NULL,     
		[ResidanceContactNumber] [varchar](100) NULL,
		[MobileNumber] [varchar](100) NULL,
		[PersonalEmailAddress] [varchar](250) NULL,
	    [Address] [varchar](500) NULL,
		[ResignationReasonId]  [int] NULL,  
		[ResignationStatus] [varchar](100) NULL,
        [CreatedOn] [datetime] NULL,
        [ModifiedOn] [datetime] NULL,
        [CreatedBy] [int] NULL,
        [ModifiedBy] [int] NULL,
    PRIMARY KEY CLUSTERED
    (
        [EmployeeResignationDetailsId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
GO