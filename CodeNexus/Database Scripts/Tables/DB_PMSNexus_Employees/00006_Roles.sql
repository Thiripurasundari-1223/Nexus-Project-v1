IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Roles')
CREATE TABLE [dbo].[Roles](
RoleID [int] IDENTITY(1,1) NOT NULL,
RoleName [varchar](2000) NULL,
RoleShortName [varchar](2000) NULL,
RoleDescription [varchar](2000) NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
	(
		[RoleID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

