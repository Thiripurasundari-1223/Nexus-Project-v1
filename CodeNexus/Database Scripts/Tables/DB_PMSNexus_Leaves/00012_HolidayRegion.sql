USE [PMSNexus_Leaves]
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='HolidayLocation')
	CREATE TABLE [dbo].[HolidayLocation](
		[HolidayLocationId] [int] IDENTITY(1,1) NOT NULL,
		[HolidayId] [int] NULL,
		[LocationId] [int] NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,		
	PRIMARY KEY CLUSTERED 
	(
		[HolidayLocationId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
