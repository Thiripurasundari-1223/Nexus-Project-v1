IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Modules')
CREATE TABLE [dbo].[Modules](
[ModuleId] [int] IDENTITY(1,1) NOT NULL,
[ModuleName] [varchar](500) NULL,
[ModuleDescription] [varchar](2000) NULL,
[ModuleIcon] [varchar](250) NULL,
[NavigationURL] [VARCHAR](1000),
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
	(
		[ModuleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Features')
CREATE TABLE [dbo].[Features](
[FeatureId] [int] IDENTITY(1,1) NOT NULL,
[FeatureName] [varchar](500) NULL,
[FeatureDescription] [varchar](2000) NULL,
[NavigationURL] VARCHAR(1000),
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
	(
		[FeatureId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='RolePermissions')
CREATE TABLE [dbo].[RolePermissions](
[RolePermissionId] [int] IDENTITY(1,1) NOT NULL,
[ModuleId] [int] NULL,
[RoleId] [int] NULL,
[FeatureId] [int] NULL,
[IsEnabled] [bit] NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
	(
		[RolePermissionId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='ModuleFeatureMapping')
CREATE TABLE [dbo].[ModuleFeatureMapping](
[ModuleFeatureMappingId] [int] IDENTITY(1,1) NOT NULL,
[ModuleId] [int] NULL,
[FeatureId] [int] NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [int] NULL,
[ModifiedOn] [datetime] NULL,
[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
	(
		[ModuleFeatureMappingId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
