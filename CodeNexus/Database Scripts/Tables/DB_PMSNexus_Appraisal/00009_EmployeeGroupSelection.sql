USE [PMSNexus_Appraisal]
Go
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='EmployeeGroupSelection')

CREATE TABLE EmployeeGroupSelection (
  APP_CYCLE_ID int NOT NULL,
  EMPLOYEE_ID int NOT NULL,
  OBJECTIVE_ID int NOT NULL,
  KEY_RESULTS_GROUP_ID int NOT NULL,
  KEY_RESULT_ID int NOT NULL,
  CREATED_BY int  NULL,
  CREATED_DATE datetime  NULL,
  UPDATED_BY int  NULL,
  UPDATED_DATE datetime  NULL,
  GRP_KEYRES_ACTUAL_VALUE decimal(18,2)  NULL,
  INDIVIDUAL_GRPITEM_RATING decimal(18,2)  NULL,
  INDIVIDUAL_KEYRES_STATUS int  NULL,
  IS_ADDRESSED int  NULL,
  PRIMARY KEY (APP_CYCLE_ID,EMPLOYEE_ID,OBJECTIVE_ID,KEY_RESULTS_GROUP_ID,KEY_RESULT_ID)
  )
GO