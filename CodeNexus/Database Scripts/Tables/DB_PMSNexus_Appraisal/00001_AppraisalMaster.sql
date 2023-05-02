
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'PMSNexus_Appraisal')
  --BEGIN
   -- CREATE DATABASE [PMSNexus_Appraisal]
  --  END

  --  GO
       USE [PMSNexus_Appraisal]
    GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='AppraisalMaster')
	CREATE TABLE AppraisalMaster (
  APP_CYCLE_ID int NOT NULL identity(1,1),
  ENTITY_ID int NOT NULL,
  VERSION_ID int NOT NULL,
  APP_CYCLE_NAME nvarchar(500) NOT NULL,
  APP_CYCLE_DESC nvarchar(2000) NOT NULL,
  APP_CYCLE_START_DATE datetime NOT NULL,
  APP_CYCLE_END_DATE datetime NOT NULL,
  APPRAISEE_REVIEW_START_DATE datetime NOT NULL,
  APPRAISEE_REVIEW_END_DATE datetime NOT NULL,
  APPRAISER_REVIEW_START_DATE datetime NOT NULL,
  APPRAISER_REVIEW_END_DATE datetime NOT NULL,
  MGMT_REVIEW_START_DATE datetime NOT NULL,
  MGMT_REVIEW_END_DATE datetime NOT NULL,
  APPRAISAL_STATUS int NOT NULL,
  DateOfJoining DateTime Null,
  EmployeesTypeId int Null,
  DURATION_ID int Null,
  CREATED_BY int  NULL,
  CREATED_DATE datetime  NULL,
  UPDATED_BY int  NULL,
  UPDATED_DATE datetime  NULL,
  PRIMARY KEY (APP_CYCLE_ID)
) 
GO