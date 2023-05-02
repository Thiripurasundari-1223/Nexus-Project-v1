IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='TimesheetComments')
BEGIN
	CREATE TABLE [dbo].[TimesheetComments](
	[TimesheetCommentsId] [int] IDENTITY(1,1) NOT NULL,
	[TimesheetId] [int] NULL,
	[Comments] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[WeekTimesheetId] uniqueidentifier NULL,
PRIMARY KEY CLUSTERED 
(
	[TimesheetCommentsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO