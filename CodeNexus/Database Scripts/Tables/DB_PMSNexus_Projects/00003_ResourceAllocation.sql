IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ResourceAllocation')
		CREATE TABLE [dbo].[ResourceAllocation] (
			[ResourceAllocationId] [int] Identity(1, 1) Not Null,
			[EmployeeId] Int NULL,
			[ProjectId] Int NULL,
			[ChangeRequestId] int NULL,
			[RequiredSkillSetId] int NULL,
			[SkillRate] Decimal(18,2) NULL,
			[RateFrequencyId] int NULL,
			[AllocationId] int NULL,
			[StartDate] [datetime] NULL,
			[EndDate] [datetime] NULL,
			[PlannedHours] Decimal(18,2) NULL,
			[Contribution] Decimal(18,2) NULL,
			[IsBillable] bit NULL,
            [IsSPOC] bit NULL,
            [IsAdditionalResource] bit NULL,
            [IsActive] bit NULL,
			[Experience] Decimal(18,2) NULL,
			[CreatedOn] [datetime] NULL,
			[CreatedBy] [int] NULL,
			[ModifiedOn] [datetime] NULL,
			[ModifiedBy] [int] NULL,
			[DeliverySupervisorId] [int] NULL
			,PRIMARY KEY CLUSTERED ([ResourceAllocationId] ASC) WITH (
						PAD_INDEX = OFF
						,STATISTICS_NORECOMPUTE = OFF
						,IGNORE_DUP_KEY = OFF
						,ALLOW_ROW_LOCKS = ON
						,ALLOW_PAGE_LOCKS = ON
						) ON [PRIMARY]
					) ON [PRIMARY]
GO