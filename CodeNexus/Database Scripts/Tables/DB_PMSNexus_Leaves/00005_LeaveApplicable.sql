IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='LeaveApplicable')
	CREATE TABLE [dbo].[LeaveApplicable](
		[LeaveApplicableId] [int] IDENTITY(1,1) NOT NULL,
		[Gender_Male] [bit] NULL,
		[Gender_Female] [bit] NULL,
		[Gender_Others] [bit] NULL,
		[MaritalStatus_Single] [bit] NULL,
		[MaritalStatus_Married] [bit] NULL,
		[EmployeeTypeId][int] NULL,
		[ProbationStatus] [int] NULL,
		[LeaveTypeId] [int] Null,
 		[CreatedOn] [datetime] NULL,
		[CreatedBy] [int] NULL,
		[ModifiedOn] [datetime] NULL,
		[ModifiedBy] [int] NULL,
		[Gender_Male_Exception] [bit] NULL,
		[Gender_Female_Exception] [bit] NULL,
		[Gender_Others_Exception] [bit] NULL,
		[MaritalStatus_Single_Exception] [bit] NULL,
		[MaritalStatus_Married_Exception] [bit] NULL,
		[Type] [nvarchar] (50) NULL,
	PRIMARY KEY CLUSTERED 
		(
			[LeaveApplicableId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	GO