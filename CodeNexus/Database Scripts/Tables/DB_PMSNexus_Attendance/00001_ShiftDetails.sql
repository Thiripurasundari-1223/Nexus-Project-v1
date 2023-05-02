IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'PMSNexus_Attendance')
  BEGIN
    CREATE DATABASE [PMSNexus_Attendance]
    END
    GO
       USE [PMSNexus_Attendance]
    GO
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='ShiftDetails')
	CREATE TABLE [dbo].[ShiftDetails](
		[ShiftDetailsId] [int] IDENTITY(1,1) NOT NULL,
		[ShiftName] [nvarchar](250) NULL,
		[ShiftCode] [nvarchar](100) NULL,
		[TimeFrom] [Time](0) NULL,
		[TimeTo] [Time](0) NULL,
		[ShiftDescription] [nvarchar](max) NULL,
		[EmployeeGroupId] [int] Null,
		[OverTime] [bit] Null,
		[IsActive] [bit] Null,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,
		[IsFlexyShift] [bit] Null,
	PRIMARY KEY CLUSTERED 
	(
		[ShiftDetailsId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
