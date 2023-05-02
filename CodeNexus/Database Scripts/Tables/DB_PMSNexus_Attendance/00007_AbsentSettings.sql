USE [PMSNexus_Attendance]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AbsentSetting')
	CREATE TABLE [dbo].[AbsentSetting](
		[AbsentSettingId] [int] IDENTITY(1,1) NOT NULL,
		[Gender_Male_Applicable] [bit] NULL,
		[Gender_Female_Applicable] [bit] NULL,
		[Gender_Others_Applicable] [bit] NULL,
		[MaritalStatus_Single_Applicable] [bit] NULL,
		[MaritalStatus_Married_Applicable] [bit] NULL,
		[Gender_Male_Exception] [bit] NULL,
		[Gender_Female_Exception] [bit] NULL,
		[Gender_Others_Exception] [bit] NULL,
		[MaritalStatus_Single_Exception] [bit] NULL,
		[MaritalStatus_Married_Exception] [bit] NULL,
		[Type] [nvarchar] (100) NULL,
 		[CreatedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedOn] [datetime] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
		(
			[AbsentSettingId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	GO


	/*Department*/
	USE [PMSNexus_Attendance]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AbsentDepartment')
	CREATE TABLE [dbo].[AbsentDepartment](
		[AbsentDepartmentId] [int] IDENTITY(1,1) NOT NULL,
		[AbsentSettingId] [int] NULL,
		[AbsentApplicableDepartmentId] [int] NULL,
		[AbsentExceptionDepartmentId] [int] NULL,
		[Type] [nvarchar] (100) NULL,			
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[AbsentDepartmentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

/*Designation*/

USE [PMSNexus_Attendance]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AbsentDesignation')
	CREATE TABLE [dbo].[AbsentDesignation](
		[AbsentDesignationId] [int] IDENTITY(1,1) NOT NULL,
		[AbsentSettingId] [int] NULL,
		[AbsentApplicableDesignationId] [int] NULL,
		[AbsentExceptionDesignationId] [int] NULL,
		[Type] [nvarchar] (100) NULL,			
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[AbsentDesignationId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

/*Location*/

USE [PMSNexus_Attendance]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AbsentLocation')
	CREATE TABLE [dbo].[AbsentLocation](
		[AbsentLocationId] [int] IDENTITY(1,1) NOT NULL,
		[AbsentSettingId] [int] NULL,
		[AbsentApplicableLocationId] [int] NULL,
		[AbsentExceptionLocationId] [int] NULL,
		[Type] [nvarchar] (100) NULL,			
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[AbsentLocationId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

/*Role*/

USE [PMSNexus_Attendance]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AbsentRole')
	CREATE TABLE [dbo].[AbsentRole](
		[AbsentRoleId] [int] IDENTITY(1,1) NOT NULL,
		[AbsentSettingId] [int] NULL,
		[AbsentApplicableRoleId] [int] NULL,
		[AbsentExceptionRoleId] [int] NULL,
		[Type] [nvarchar] (100) NULL,			
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[AbsentRoleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

/*EmployeeType*/

USE [PMSNexus_Attendance]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AbsentEmployeeType')
    CREATE TABLE [dbo].[AbsentEmployeeType] (
        [AbsentEmployeeTypeId] [int] IDENTITY(1,1) NOT NULL,
        [AbsentSettingId] [int] NULL,
        [AbsentApplicableEmployeeTypeId] [int] NULL,           
		[AbsentExceptionEmployeeTypeId] [int] NULL,    
		[Type] [nvarchar] (100) NULL,			
        [CreatedOn] [datetime] NULL,
        [ModifiedOn] [datetime] NULL,
        [CreatedBy] [int] NULL,
        [ModifiedBy] [int] NULL,
    PRIMARY KEY CLUSTERED
    (
        [AbsentEmployeeTypeId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
GO

/*Probationstatus*/

USE [PMSNexus_Attendance]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AbsentProbationStatus')
    CREATE TABLE [dbo].[AbsentProbationStatus] (
        [AbsentProbationStatusId] [int] IDENTITY(1,1) NOT NULL,
        [AbsentSettingId] [int] NULL,
        [AbsentApplicableProbationStatusId] [int] NULL,   
		[AbsentExceptionProbationStatusId] [int] NULL,
		[Type] [nvarchar] (100) NULL,        
        [CreatedOn] [datetime] NULL,
        [ModifiedOn] [datetime] NULL,
        [CreatedBy] [int] NULL,
        [ModifiedBy] [int] NULL,	
    PRIMARY KEY CLUSTERED
    (
        [AbsentProbationStatusId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
GO

/*employee */

USE [PMSNexus_Attendance]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AbsentEmployee')
    CREATE TABLE [dbo].[AbsentEmployee] (
        [AbsentEmployeeId] [int] IDENTITY(1,1) NOT NULL,
        [AbsentSettingId] [int] NULL,
        [AbsentApplicableEmployeeId] [int] NULL, 
		[AbsentExceptionEmployeeId] [int] NULL,
		[Type] [nvarchar] (100) NULL,          
        [CreatedOn] [datetime] NULL,
        [ModifiedOn] [datetime] NULL,
        [CreatedBy] [int] NULL,
        [ModifiedBy] [int] NULL,		
    PRIMARY KEY CLUSTERED
    (
        [AbsentEmployeeId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
GO

/*Restrictions*/

USE [PMSNexus_Attendance]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AbsentRestrictions')
    CREATE TABLE [dbo].[AbsentRestrictions] (
        [AbsentRestrictionId] [int] IDENTITY(1,1) NOT NULL,
        [AbsentSettingId] [int] NULL,
        [WeekendsBetweenAttendacePeriod] [bit] NULL, 
		[HolidaysBetweenAttendancePeriod] [bit] NULL,
		[CreatedOn] [datetime] NULL,
        [ModifiedOn] [datetime] NULL,
        [CreatedBy] [int] NULL,
        [ModifiedBy] [int] NULL,		
    PRIMARY KEY CLUSTERED
    (
        [AbsentRestrictionId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
GO