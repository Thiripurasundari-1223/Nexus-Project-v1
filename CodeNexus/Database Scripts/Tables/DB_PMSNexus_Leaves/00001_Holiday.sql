IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Holiday')
	CREATE TABLE [dbo].[Holiday](
		[HolidayID] [int] IDENTITY(1,1) NOT NULL,
		[Year] [int] NULL,
		[HolidayName] [nvarchar](250) NULL,
		[HolidayCode] [nvarchar](100) NULL,			
		[HolidayDescription] [nvarchar](max) NULL,		
		[IsActive] [bit] Null,
		[HolidayDate] [Date] Null,
		[IsRestrictHoliday] [bit] NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedBy] [int] NULL,		
	PRIMARY KEY CLUSTERED 
	(
		[HolidayID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
