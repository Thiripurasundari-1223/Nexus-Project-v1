IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EmployeeNationality')
    CREATE TABLE [dbo].[EmployeeNationality](
        [NationalityId] [int] IDENTITY(1,1) NOT NULL,
        [NationalityName] [varchar](50) NULL,
        [CreatedOn] [datetime] NULL,
        [CreatedBy] [int] NULL,
    PRIMARY KEY CLUSTERED
    (
        [NationalityId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
GO