IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Notifications')
CREATE TABLE [dbo].[Notifications](
[NotificationId] [int] IDENTITY(1,1) NOT NULL,
[FromId] [int] NULL,
[ToId] [int] NULL,
[NotificationSubject] [VARCHAR](2000) NULL,
[NotificationBody] [VARCHAR](2000) NULL,
[MarkAsRead] [bit] NULL,
[PrimaryKeyId] [INT] NULL,
[ButtonName] [VARCHAR](200) NULL,
[SourceType] [VARCHAR](200) NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL
PRIMARY KEY CLUSTERED 
	(
		[NotificationId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO