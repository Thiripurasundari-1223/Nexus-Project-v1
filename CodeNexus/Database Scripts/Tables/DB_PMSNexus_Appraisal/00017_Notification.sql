USE [PMSNexus_Appraisal]
Go
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Notification')

CREATE TABLE Notification (
  NOTIFICATION_ID int identity(1,1) NOT NULL,
  EMPLOYEE_ID int NULL,
  NOTIFICATION_CATEGORY_ID int NULL,
  APP_CYCLE_ID int NULL,
  PAGE_ID int NULL,
  NOTIFICATION_MESSAGE nvarchar(500) NULL,
  NOTIFICATION_IMAGE_URL nvarchar(200) NULL,
  NOTIFICATION_VISITED_STATUS int NULL,
  CREATED_BY int NULL,
  CREATED_DATE datetime NULL,
  UPDATED_BY int  NULL,
  UPDATED_DATE datetime NULL,
  PRIMARY KEY (NOTIFICATION_ID)
) 
GO
