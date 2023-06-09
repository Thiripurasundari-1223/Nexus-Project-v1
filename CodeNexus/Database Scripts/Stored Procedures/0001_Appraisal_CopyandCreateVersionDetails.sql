USE [PMSNexus_Appraisal]
GO
IF (OBJECT_ID('CopyandCreateVersionDetails') IS NOT NULL)
  DROP PROCEDURE [dbo].[CopyandCreateVersionDetails]
GO
/****** Object:  StoredProcedure [dbo].[CopyandCreateVersionDetails]    Script Date: 26-07-2021 22:18:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CopyandCreateVersionDetails]
            @VersionId INT,
			@CreatedBy INT
        AS
        BEGIN
            SET NOCOUNT ON;    
			DECLARE @NewVersionId INT
			DECLARE @Output INT
			declare @versionCode varchar(100);
			declare @maxVersionId int, @groupCount int;
			declare @versionTable table(id int);
			declare @benchmarkGroupTable table(id int identity(1,1), benchmarkGroupId int);
			-----------------Copy and Create Version Master-----------------

			Select top 1 @maxVersionId= VERSION_ID from [dbo].[VersionMaster] order by VERSION_ID desc;
			set @maxVersionId=@maxVersionId+1;
			if(len(@maxVersionId)<5)
			Begin
			Set @versionCode= REPLACE(STR(@maxVersionId,4),' ','0')
			End

              INSERT INTO [dbo].[VersionMaster] (VERSION_NAME,VERSION_CODE,VERSION_DESC,CREATED_BY,CREATED_DATE)
			  OUTPUT Inserted.VERSION_ID into @versionTable
              VALUES ('New Version','Version-'+convert(varchar(100),@versionCode),'New Version Description',1,GETDATE())	 	 
			  ---- Select new version Id -------
			  Select top 1 @NewVersionId = id from @versionTable;

			-----------------Copy ad create Version Department Role Mapping---------------

			  INSERT INTO [dbo].[VersionDepartmentRoleMapping] (VERSION_ID,DEPT_ID,ROLE_ID,CREATED_BY,CREATED_DATE)
			  SELECT @NewVersionId,DEPT_ID,ROLE_ID,@CreatedBy,GETDATE() FROM [dbo].[VersionDepartmentRoleMapping]
			  WHERE VERSION_ID = @VersionId

			----------------Copy and Create Version Department Role Objective---------------

			INSERT INTO [dbo].[VersionDepartmentRoleObjective] (VERSION_ID,DEPT_ID,ROLE_ID,OBJECTIVE_ID,OBJECTIVE_WEIGHTAGE,CREATED_BY,CREATED_DATE)
			SELECT @NewVersionId,DEPT_ID,ROLE_ID,OBJECTIVE_ID,OBJECTIVE_WEIGHTAGE,@CreatedBy,GETDATE() FROM [dbo].[VersionDepartmentRoleObjective]
			WHERE VERSION_ID = @VersionId

			----------------Copy and Create Version Key Results------------------------

			INSERT INTO [dbo].[VersionKeyResults] (VERSION_ID,DEPT_ID,ROLE_ID,OBJECTIVE_ID,KEY_RESULT_ID,KEY_RESULT_WEIGHTAGE,BENCHMARK_TYPE
            ,BENCHMARK_DURATION,BENCHMARK_OPERATOR,BENCHMARK_VALUE,BENCHMARK_FROM_VALUE,BENCHMARK_TO_VALUE,CREATED_BY,CREATED_DATE,UPDATED_BY,UPDATED_DATE,
			BENCHMARK_UITYPE,IS_DOCUMENT_MANDATORY)
			SELECT @NewVersionId,DEPT_ID,ROLE_ID,OBJECTIVE_ID,KEY_RESULT_ID,KEY_RESULT_WEIGHTAGE,BENCHMARK_TYPE
            ,BENCHMARK_DURATION,BENCHMARK_OPERATOR,BENCHMARK_VALUE,BENCHMARK_FROM_VALUE,BENCHMARK_TO_VALUE,@CreatedBy,GETDATE(),@CreatedBy,GETDATE(),BENCHMARK_UITYPE,IS_DOCUMENT_MANDATORY
			FROM [dbo].[VersionKeyResults] WHERE VERSION_ID = @VersionId

			----------------Copy and Create VersionBenchMarks-------------------------

			INSERT INTO [dbo].[VersionBenchMarks] (VERSION_ID,DEPT_ID,ROLE_ID,OBJECTIVE_ID,KEY_RESULT_ID,RANGE_FROM,RANGE_TO,
			BENCHMARK_VALUE,BENCHMARK_WEIGHTAGE,CREATED_BY,CREATED_DATE,BENCHMARK_SUBJECTIVE_NAME)
			SELECT @NewVersionId,DEPT_ID,ROLE_ID,OBJECTIVE_ID,KEY_RESULT_ID,RANGE_FROM,RANGE_TO,
			BENCHMARK_VALUE,BENCHMARK_WEIGHTAGE,@CreatedBy,GETDATE(),BENCHMARK_SUBJECTIVE_NAME FROM [dbo].[VersionBenchMarks]
			WHERE VERSION_ID = @VersionId;			
			
			INSERT INTO  @benchmarkGroupTable (benchmarkGroupId) SELECT KEY_RESULTS_GROUP_ID FROM [dbo].[VersionKeyResultsGroup]
			WHERE VERSION_ID = @VersionId

			Select @groupCount=count(id) from @benchmarkGroupTable;
			
			DECLARE @count INT, @oldGroupId INT,@newGroupId INT;
            SET @count = 1;

			WHILE @count<= @groupCount
			BEGIN
			   
			   delete from @versionTable;
			   Select @oldGroupId=benchmarkGroupId from @benchmarkGroupTable where id=@count;
			  
			   ----------------Copy and Create Version Key Results Group-----------------
			   INSERT INTO [dbo].[VersionKeyResultsGroup] (VERSION_ID,DEPT_ID,ROLE_ID,OBJECTIVE_ID,KEY_RESULTS_GROUP_NAME
            ,MANDATORY_KEY_RESULT_OPTIONS,KEY_RESULT_GROUP_WEIGHTAGE,CREATED_BY,CREATED_DATE)
			OUTPUT Inserted.KEY_RESULTS_GROUP_ID into @versionTable
			SELECT @NewVersionId,DEPT_ID,ROLE_ID,OBJECTIVE_ID,KEY_RESULTS_GROUP_NAME
            ,MANDATORY_KEY_RESULT_OPTIONS,KEY_RESULT_GROUP_WEIGHTAGE,@CreatedBy,GETDATE() FROM [dbo].[VersionKeyResultsGroup]
			WHERE VERSION_ID = @VersionId AND KEY_RESULTS_GROUP_ID= @oldGroupId
			
			
			----------------Copy and Create Version Key Results Group Details-----------------
			Select top 1 @newGroupId=id from @versionTable;

			INSERT INTO [dbo].[VersionKeyResultsGroupDetails] (VERSION_ID,DEPT_ID,ROLE_ID,OBJECTIVE_ID,KEY_RESULTS_GROUP_ID
            ,KEY_RESULT_ID,CREATED_BY,CREATED_DATE)
			SELECT @NewVersionId,DEPT_ID,ROLE_ID,OBJECTIVE_ID,@newGroupId
            ,KEY_RESULT_ID,@CreatedBy,GETDATE() FROM [dbo].[VersionKeyResultsGroupDetails]
			WHERE VERSION_ID = @VersionId AND KEY_RESULTS_GROUP_ID= @oldGroupId

			   SET @count = @count + 1;
			END;

			
       Select 1 as success, 'New version created successfully' as data
        END
		