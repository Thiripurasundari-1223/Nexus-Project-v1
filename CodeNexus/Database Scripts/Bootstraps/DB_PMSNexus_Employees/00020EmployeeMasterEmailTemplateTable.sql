USE [PMSNexus_Employees]
GO
TRUNCATE TABLE [dbo].[EmployeeMasterEmailTemplate]
GO
INSERT [dbo].[EmployeeMasterEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'RetirementNotification', N'Retirement Notification @employeeName', N'
<html>
     <body>
	   <p>Hi Team,</p>  
      <p>This is to notify that <b> @employeeName </b> is getting retired on <b> @retirementDate.</b></p>  
         
       <p>Kindly do the needful.</p>
       <p>Regards,<br>
        People Experience</br>
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[EmployeeMasterEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'Deactivation', N'Deactivation @employeeName', N'
<html>
     <body>
	   <p>Hi Team,</p>  
       <p>This is to notify that <b> @employeeName </b> has been <b> deactivated </b> in Nexus.</p>  
       <p>Regards,<br>
        People Experience</br>
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[EmployeeMasterEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'ChangeRequest', N'@employeeName Change of details', N'
<html>
     <body>
	   <p>Hi Team,</p>  
       <p>This is to notify that <b> @employeeName </b> has changed the <b> @changeRequestCategory </b> in Nexus. Please find the details.</p>
	   <table>
	   <tr  style= "text-align:center";> 
                         <td><b> Field Name</b></td>
                         <td><b> Previous Detail </b></td >
                         <td><b> Current Detail </b></td > 
                         </tr>
						 @textBody </table>
       <p> @link</p>
       <p>Regards,<br>
        People Experience</br>
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[EmployeeMasterEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'EmployeeDetailsChange', N'@employeeName Change of details', N'
<html>
     <body>
	   <p>Hi Team,</p>  
       <p>This is to notify that <b> @employeeName </b> has changed the <b> @detail </b> in Nexus. Please find the details.</p>
	   <p>Previous Details</p>
	   <p>Current Details</p>
       <p> @link </p>
       <p>Regards,<br>
        People Experience</br>
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[EmployeeMasterEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'RequestWithdrawn', N'@employeeName Change of detail request withdrawn', N'
<html>
     <body>
	   <p>Hi @employeeName,</p>  
       <p>This is to notify that <b> @employeeName </b> has change of <b> @detail </b> request has been withdrawn.</p>
       <p>Regards,<br>
        People Experience</br>
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[EmployeeMasterEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'ApproveOrReject', N'Change of @detail @Status', N'
<html>
     <body>
	   <p>Hi @employeeName,</p>  
       <p>This is to notify that your change request for <b> @detail </b> in Nexus has been <b> @Status.</b> </p>
	     @rejectedRemark
	   <p>Please click <a href="@baseURL/#/pmsnexus/employees/my-request" target="_blank"> here </a> to view details.</p>
	   <p>For any queries or support, please feel free to contact <a href="mailto:peoplexp@tvsnext.io">peoplexp@tvsnext.io</a></p>
       <p>Regards,<br>
        People Experience</br>
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[EmployeeMasterEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'ReportingManagerChange', N'Change of Reporting Manager', N'
<html>
     <body>
	   <p>Hi @employeeName,</p>  
       <p>This is to notify that your Reporting Manager has been changed from <b> @reportingManagerPrev </b> to <b> @reportingManagerCurrent. </b></p>
	   <p>Hi PMO/IT,</p>
	   <p>Kindly update your records and do the needful.</p>
       <p>Regards,<br>
        People Experience</br>
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[EmployeeMasterEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'DesignationChange', N'Change of Designation', N'
<html>
     <body>
	   <p>Hi @employeeName,</p>  
       <p>This is to notify that your Designation has been changed from <b> @designationPrev </b> to <b>@designationCurrent</b> on Nexus.</p>
       <p>Regards,<br>
        People Experience</br>
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[EmployeeMasterEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'BaseWorkLocationChange', N'Change of Base Work Location', N'
<html>
     <body>
	   <p>Hi @employeeName,</p>  
       <p>This is to notify that your Base Work Location has been changed from <b>@baseWorkLocationPrev</b> to <b>@baseWorkLocationCurrent</b> on Nexus.</p>
       <p>Regards,<br>
        People Experience</br>
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[EmployeeMasterEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'ProbationStatusChange', N'Change of Probation Status', N'
<html>
     <body>
	   <p>Hi @employeeName,</p>  
       <p>This is to notify that you have successfully completed your probation period.</p>
       <p>Regards,<br>
        People Experience</br>
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[EmployeeMasterEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'ContractClosureNotification', N'@employeeName Contract Closure Notification', N'
<html>
     <body>
	   <p>Hi @reportingManager,</p>  
       <p>This is to notify that <b>@employeeName</b> contract will end on <b>@contractEndDate.</b></p>
	   <p>Request you to send an email to the People Experience on the contract extension. If not, the contract will be closed on the above mentioned date.</p>
       <p>Regards,<br>
        People Experience</br>
       </p>
      </body>
  </html>', 1, getdate())
  GO