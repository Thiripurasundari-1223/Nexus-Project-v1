USE PMSNexus_Employees
GO

TRUNCATE TABLE EmployeeCategory
GO

declare @DepartmentId int;

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Engineering';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','Engineering Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','Engineering Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Engineering BU Head' ,getdate(), 1)

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'US Sales';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','US Sales Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','US Sales Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','US Sales BU Head' ,getdate(), 1)

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'India Sales';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','India Sales Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','India Sales Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','India Sales BU Head' ,getdate(), 1)


set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Consulting';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','Consulting Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','Consulting Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Consulting BU Head' ,getdate(), 1)


set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Marketing';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','Marketing Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','Marketing Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Marketing BU Head' ,getdate(), 1)


set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Finance';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','Finance Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','Finance Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Finance BU Head' ,getdate(), 1)


set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'IT';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','IT Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','IT Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','IT BU Head' ,getdate(), 1)


set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Human Resources';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Ops Individual','Human Resources OpsIndividual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Ops Manager','Human Resources OpsManager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Human Resources BU Head' ,getdate(), 1)

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Human Resources - Recruitment';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Talent Acquisition Individual','Human Resources - Recruitment Talent Acquisition Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Talent Acquisition Manager','Human Resources - Recruitment Talent Acquisition Manager' ,getdate(), 1)

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Corporate';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'COO','COO' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'CEO','COO' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Corporate BU Head',getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Director','Director',getdate(), 1)


set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Admin';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Admin','Admin' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','Admin Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Super Admin','Super Admin' ,getdate(), 1)

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Intelligence';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','Intelligence Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','Intelligence Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Intelligence BU Head' ,getdate(), 1)

set @DepartmentId=0; 
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'People Experience';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','People Experience Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','People Experience Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','People Experience BU Head' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Super Admin','Super Admin' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'HR BU Head','HR BU Head' ,getdate(), 1)

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Recruitment';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','Recruitment Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','Recruitment Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Recruitment BU Head' ,getdate(), 1)


set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'SFL';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','SFL Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','SFL Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','SFL BU Head' ,getdate(), 1)

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'US Consulting';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','US Consulting Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','US Consulting Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','US Consulting BU Head' ,getdate(), 1)

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Client Delivery';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','Client Delivery Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','Client Delivery Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Client Delivery BU Head' ,getdate(), 1)

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Experiences';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','Experiences Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','Experiences Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Experiences BU Head' ,getdate(), 1)

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Talent Acquisition';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','Talent Acquisition Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','Talent Acquisition Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Talent Acquisition BU Head' ,getdate(), 1)

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Sales - US';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','Sales - US Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','Sales - US Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Sales - US BU Head' ,getdate(), 1)

set @DepartmentId=0;
Select @DepartmentId=DepartmentId from Department where DepartmentName= 'Sales - APAC';
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Individual','Sales - APAC Individual' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'Manager','Sales - APAC Manager' ,getdate(), 1)
INSERT INTO EmployeeCategory (DepartmentId,EmployeeCategoryName, Description ,CreatedOn, CreatedBy)
VALUES(@DepartmentId,'BU Head','Sales - APAC BU Head' ,getdate(), 1)







