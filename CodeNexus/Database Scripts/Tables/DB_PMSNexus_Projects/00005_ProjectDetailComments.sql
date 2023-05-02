IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='ProjectDetailComments')
CREATE TABLE [dbo].[ProjectDetailComments](
[ProjectDetailCommentId] [int] IDENTITY(1,1) NOT NULL,
[ProjectDetailId] int NULL,
[ChangeRequestId] int NULL,
[Comments] [varchar](2000) NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL
PRIMARY KEY CLUSTERED 
	(
		[ProjectDetailCommentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO