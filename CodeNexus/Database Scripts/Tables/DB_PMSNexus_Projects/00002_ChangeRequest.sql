IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ChangeRequest')
	CREATE TABLE [dbo].[ChangeRequest] (
		 [ChangeRequestId] [int] IDENTITY(1, 1) NOT NULL
		,[ChangeRequestName] [varchar](2000) NULL
		,[ProjectId] [int] NOT NULL
		,[ChangeRequestTypeId] [int] NULL
		,[CurrencyId] [int] NULL
		,[SOWAmount] Decimal(18,2) NULL
		,[ChangeRequestDuration] Decimal(18,2) NULL
		,[ChangeRequestStartDate] DateTime NULL
		,[ChangeRequestEndDate] DateTime NULL
		,[ChangeRequestDescription] [varchar](2000) NULL		
		,[ChangeRequestStatus] [varchar](2000) NULL
		,[CreatedOn] [datetime] NULL
	    ,[CreatedBy] [int] NULL
	    ,[ModifiedOn] [datetime] NULL
	    ,[ModifiedBy] [int] NULL
		,[CRStatusCode] [varchar](100) NULL
		,[CRChanges] [varchar](max) NULL
		,[FormattedChangeRequestId] [varchar](250) NULL
		,PRIMARY KEY CLUSTERED 
	(
		[ChangeRequestId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO