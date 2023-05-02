USE [PMSNexus_ExitManagement]
GO
TRUNCATE TABLE [dbo].[ExitManagementEmailTemplate]
GO
INSERT [dbo].[ExitManagementEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'ResignationApprovalManager', N'@employeeName sent request for Resignation approval', N'
<html>
     <body>
	   <p>Hi @approverName,</p>  
      <p>Please find the below <b>resignation</b> details:</p>  
         <table>
               <tr  style="text-align:left";> 
                    <td>Employee Id </td> 
                    <td> : </td>
                    <td>@formattedEmployeeId</td > 
               </tr>
               <tr  style="text-align:left";> 
                    <td>Employee Name </td> 
                    <td> : </td>
                    <td>@employeeName</td > 
               </tr>
	       <tr  style="text-align:left";> 
                    <td>Manager </td>
                    <td> : </td>
                    <td>@managerName</td > 
               </tr>
	       <tr  style="text-align:left";> 
                    <td>Department </td> 
                    <td> : </td>
                    <td>@department</td > 
               </tr>
		<tr  style="text-align:left";> 
                    <td>Resignation Date </td> 
                    <td> : </td>
                    <td>@resignationDate</td > 
               </tr>
		<tr  style="text-align:left";> 
                    <td>Reason </td> 
                    <td> : </td>
                    <td>@reason</td > 
               </tr>
          </table>
       <p> Please click <a href="@baseURL/#/pmsnexus/exitmanagement/resignation-application?resignationID=1&isChecklist=false"  target="_blank">here</a> to approve/reject.</p>
       <p>Thanks & Regards,<br>
        Team HR </br>
        TVS Next
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[ExitManagementEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'ResignationApprovalHR', N'@employeeName Resignation approval request is @status', N'
<html>
     <body>
	   <p>Hi,</p>  
      <p>Please find the below <b>resignation</b> details:</p>  
         <table>
               <tr  style="text-align:left";> 
                    <td>Employee Id </td> 
                    <td> : </td>
                    <td>@formattedEmployeeId</td > 
               </tr>
               <tr  style="text-align:left";> 
                    <td>Employee Name </td> 
                    <td> : </td>
                    <td>@employeeName</td > 
               </tr>
	       <tr  style="text-align:left";> 
                    <td>Manager </td>
                    <td> : </td>
                    <td>@managerName</td > 
               </tr>
	       <tr  style="text-align:left";> 
                    <td>Department </td> 
                    <td> : </td>
                    <td>@department</td > 
               </tr>
		<tr  style="text-align:left";> 
                    <td>Resignation Date </td> 
                    <td> : </td>
                    <td>@resignationDate</td > 
               </tr>
		<tr  style="text-align:left";> 
                    <td>Reason </td> 
                    <td> : </td>
                    <td>@reason</td > 
               </tr>
          </table>
       <p> Please click <a href="@baseURL/#/pmsnexus/exitmanagement/resignation-application?resignationID=1&isChecklist=false"  target="_blank">here</a> to view.</p>
       <p>Thanks & Regards,<br>
        Team HR </br>
        TVS Next
       </p>
       </br>
       <table>
               <tr  style="text-align:left";> 
                    <td>Requested for </td> 
					<td></td>
                    <td><pre>                      </pre></td>
                    <td>@employeeName</td > 
               </tr>
               <tr  style="text-align:left";> 
                    <td style="vertical-align: top;" >@approverName''s comment </td> 
                    <td></td>
					<td><pre>                      </pre></td>
                    <td>@comments</td > 
               </tr>
       </table>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[ExitManagementEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'WithdrawalResignationApprovalManager', N'@employeeName sent request for Resignation Withdrawal approval', N'
<html>
     <body>
	   <p>Hi @approverName,</p>  
      <p>Please find the below <b>resignation withdrawal</b> details:</p>  
         <table>
               <tr  style="text-align:left";> 
                    <td>Employee Id </td> 
                    <td> : </td>
                    <td>@formattedEmployeeId</td > 
               </tr>
               <tr  style="text-align:left";> 
                    <td>Employee Name </td> 
                    <td> : </td>
                    <td>@employeeName</td > 
               </tr>
	       <tr  style="text-align:left";> 
                    <td>Manager </td>
                    <td> : </td>
                    <td>@managerName</td > 
               </tr>
	       <tr  style="text-align:left";> 
                    <td>Department </td> 
                    <td> : </td>
                    <td>@department</td > 
               </tr>
		<tr  style="text-align:left";> 
                    <td>Resignation Date </td> 
                    <td> : </td>
                    <td>@resignationDate</td > 
               </tr>
		<tr  style="text-align:left";> 
                    <td>Withdrawal Reason </td> 
                    <td> : </td>
                    <td>@reason</td > 
               </tr>
          </table>
       <p> Please click <a href="@baseURL/#/pmsnexus/exitmanagement/resignation-application?resignationID=1&isChecklist=false"  target="_blank">here</a> to approve/reject.</p>
       <p>Thanks & Regards,<br>
        Team HR </br>
        TVS Next
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[ExitManagementEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'WithdrawalResignationApprovalHR', N'@employeeName Resignation Withdrawal approval request is @status', N'
<html>
     <body>	 
	   <p>Hi,</p>  
      <p>Please find the below <b>resignation withdrawal</b> details:</p>  
         <table>
               <tr  style="text-align:left";> 
                    <td>Employee Id </td> 
                    <td> : </td>
                    <td>@formattedEmployeeId</td > 
               </tr>
               <tr  style="text-align:left";> 
                    <td>Employee Name </td> 
                    <td> : </td>
                    <td>@employeeName</td > 
               </tr>
	       <tr  style="text-align:left";> 
                    <td>Manager </td>
                    <td> : </td>
                    <td>@managerName</td > 
               </tr>
	       <tr  style="text-align:left";> 
                    <td>Department </td> 
                    <td> : </td>
                    <td>@department</td > 
               </tr>
		<tr  style="text-align:left";> 
                    <td>Resignation Date </td> 
                    <td> : </td>
                    <td>@resignationDate</td > 
               </tr>
		<tr  style="text-align:left";> 
                    <td>Withdrawal Reason </td> 
                    <td> : </td>
                    <td>@reason</td > 
               </tr>
          </table>
       <p> Please click <a href="@baseURL/#/pmsnexus/exitmanagement/resignation-application?resignationID=1&isChecklist=false"  target="_blank">here</a> to view.</p>
       <p>Thanks & Regards,<br>
        Team HR </br>
        TVS Next
       </p>
       </br>
       <table>
               <tr  style="text-align:left";> 
                    <td>Requested for </td> 
					<td></td>
                    <td><pre>                      </pre></td>
                    <td>@employeeName</td > 
               </tr>
               <tr  style="text-align:left";> 
                    <td style="vertical-align: top;">@approverName''s comment </td> 
                    <td></td>
					<td><pre>                      </pre></td>
                    <td>@comments</td > 
               </tr>
       </table>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[ExitManagementEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'ExitInterview', N'Exit Interview Submitted', N'
<html>
     <body>
	   <p>Hi,</p>  
      <p>@employeeName has filled out the Exit Interview Form.</p>  
       <p> Please click <a href="@baseURL/#/pmsnexus/exitmanagement/resignation-application?resignationID=@resignationId&isChecklist=false"  target="_blank">here</a> to view.</p>
       <p>Regards,<br>
        People Experience Team </br>
        TVS Next
       </p>
      </body>
  </html>', 1, getdate())
GO

INSERT [dbo].[ExitManagementEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'ExitCheckList', N'Exit Checklist - @employeeFullName ', N'
<html>
     <body>
	  <p>Hi,<p>
 
      <p>This is with reference to the separation request for <b>@formattedEmployeeId - @employeeName</b>. This email is being sent to you for providing your input in checklist as you are one of the checklist approvers in the workflow.<p>
 
      <p>Details as follows:<p>
 <table>
               <tr  style="text-align:left";> 
                    <td>Name </td> 
                    <td> : </td>
                    <td>@employeeName</td > 
               </tr>
               <tr  style="text-align:left";> 
                    <td>Employee Id </td> 
                    <td> : </td>
                    <td>@formattedEmployeeId</td > 
               </tr>
	       <tr  style="text-align:left";> 
                    <td>Designation </td>
                    <td> : </td>
                    <td>@designation</td > 
               </tr>
	       <tr  style="text-align:left";> 
                    <td>Joining Date </td> 
                    <td> : </td>
                    <td>@dateOfJoining</td > 
               </tr>
		<tr  style="text-align:left";> 
                    <td>Resigned Date </td> 
                    <td> : </td>
                    <td>@resignationDate</td > 
               </tr>
		<tr  style="text-align:left";> 
                    <td>Relieving Date </td> 
                    <td> : </td>
                    <td>@relievingDate</td > 
               </tr>
          </table>
 
     <p>Click <a href="@baseURL/#/pmsnexus/exitmanagement/resignation-application?resignationCheckListId=@resignationCheckListId&isChecklist=true&empId=@employeeId&role=@role"  target="_blank">here</a> to view and update the details.</p>
	 <p>Regards,<br>
        People Experience Team </br>
        TVS Next
       </p>
      </body>
  </html>', 1, getdate())
GO
INSERT [dbo].[ExitManagementEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'ExitInterviewNotification', N'Please submit Exit Interview', N'
<html>
     <body>
	   <p>Hi @employeeName,</p>  
      <p> Please submit your Exit Interview Form.</p>  
       <p> Please click <a href="@baseURL/#/pmsnexus/exitmanagement/resignation-application"  target="_blank">here</a> to submit Exit Interview.</p>
       <p>Regards,<br>
        People Experience Team </br>
        TVS Next
       </p>
      </body>
  </html>', 1, getdate())
GO
GO
INSERT [dbo].[ExitManagementEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'ExitCheckListNotification', N'Please Submit Exit Checklist', N'
<html>
     <body>
	   <p>Hi @employeeName,</p>  
      <p> Please submit your Exit Checklist Form.</p>  
       <p> Please click <a href="@baseURL/#/pmsnexus/exitmanagement/resignation-application"  target="_blank">here</a> to submit Exit Checklist.</p>
       <p>Regards,<br>
        People Experience Team </br>
        TVS Next
       </p>
      </body>
  </html>', 1, getdate())

INSERT [dbo].[ExitManagementEmailTemplate] ( [TemplateName], [Subject], [Body], [CreatedBy], [CreatedOn]) VALUES ( N'ExitCheckListCompleteNotification', N'Exit Checklist - @employeeFullName ', N'
<html>
     <body>
	  <p>Hi,<p>
 
      <p>This is with reference to the separation request for <b>@formattedEmployeeId - @employeeName</b>. This email is being sent to you for providing your input in checklist as you are one of the checklist approvers in the workflow.<p>
 
      <p>Details as follows:<p>
 <table>
               <tr  style="text-align:left";> 
                    <td>Name </td> 
                    <td> : </td>
                    <td>@employeeName</td > 
               </tr>
               <tr  style="text-align:left";> 
                    <td>Employee Id </td> 
                    <td> : </td>
                    <td>@formattedEmployeeId</td > 
               </tr>
	       <tr  style="text-align:left";> 
                    <td>Designation </td>
                    <td> : </td>
                    <td>@designation</td > 
               </tr>
	       <tr  style="text-align:left";> 
                    <td>Joining Date </td> 
                    <td> : </td>
                    <td>@dateOfJoining</td > 
               </tr>
		<tr  style="text-align:left";> 
                    <td>Resigned Date </td> 
                    <td> : </td>
                    <td>@resignationDate</td > 
               </tr>
		<tr  style="text-align:left";> 
                    <td>Relieving Date </td> 
                    <td> : </td>
                    <td>@relievingDate</td > 
               </tr>
          </table>
 
     <p>Click <a href="@baseURL/#/pmsnexus/exitmanagement/resignation-application?resignationCheckListId=@resignationCheckListId&isChecklist=true&empId=@employeeId&role=@role"  target="_blank">here</a> to view and update the details.</p>
	 <p>Regards,<br>
        People Experience Team </br>
        TVS Next
       </p>
      </body>
  </html>', 1, getdate())
  GO









