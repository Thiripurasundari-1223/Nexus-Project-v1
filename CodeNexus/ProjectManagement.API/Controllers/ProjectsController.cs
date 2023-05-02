using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectManagement.DAL.Services;
using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using SharedLibraries.Models.Projects;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Appraisal;
using SharedLibraries.ViewModels.Home;
using SharedLibraries.ViewModels.Projects;
using SharedLibraries.ViewModels.Reports;
using System;
using System.Collections.Generic;

namespace ProjectManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly PMServices _pmServices;
        public ProjectsController(PMServices pMServices)
        {
            this._pmServices = pMServices;
        }


        #region Code Commented
        [HttpGet("GetEmptyMethod")]
        public IActionResult Get()
        {
            return Ok(new
            {
                StatusCode = "SUCCESS",
                Data = "Projects API - GET Method"
            });
        }

        #endregion


        /*  #region Project Details
  #region Bulk Insert Project
  [HttpPost]
  [Route("BulkInsertProject")]
  public IActionResult BulkInsertProject(SharedLibraries.ViewModels.Projects.ImportExcelView importExcelView)
  {
      try
      {
          var result = _pmServices.BulkInsertProject(importExcelView);
          return Ok(
              new
              {
                  StatusCode = "Success",
                  StatusText = "Inserted Successfully",
                  Data = result
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/BulkInserProject");
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = ex.Message,
              Data = 0
          });
      }
  }
        #endregion

        #region
        #region Insert And Update
        [HttpPost]
  [Route("InsertUpdateProject")]
  public IActionResult InsertAndUpdateProject(ProjectDetailView pProjectDetails)
  {
      try
      {
          if (_pmServices.ProjectNameDuplication(pProjectDetails))
          {
              return Ok(new
              {
                  StatusCode = "FAILURE",
                  StatusText = "Project Name is already exists. Please change your project name and try again.",
                  Data = 0
              });
          }
          else
          {
              int ProjectId = _pmServices.InsertandUpdateProject(pProjectDetails).Result;
              if (ProjectId > 0)
              {
                  return Ok(new
                  {
                      StatusCode = "SUCCESS",
                      StatusText = (pProjectDetails.IsDraft == true ? "Project drafted successfully." : "Project approval request sent successfully."),
                      Data = ProjectId
                  });
              }
              else
              {
                  return Ok(new
                  {
                      StatusCode = "FAILURE",
                      StatusText = "Unexpected error occurred. Try again.",
                      Data = 0
                  });
              }
          }
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/InsertAndUpdateProject", JsonConvert.SerializeObject(pProjectDetails));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = 0
          });
      }
  }


  #region Delete Project
  [HttpGet]
  [Route("DeleteProject")]
  public IActionResult DeleteProject(int pProjectId)
  {
      try
      {
          if (_pmServices.DeleteProject(pProjectId))
          {
              return Ok(new { StatusCode = "SUCCESS", StatusText = "Project details deleted successfully.", Data = true });
          }
          else
          {
              return Ok(new
              {
                  StatusCode = "FAILURE",
                  StatusText = "Unexpected error occurred. Try again.",
                  Data = false
              });
          }
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/DeleteProject", Convert.ToString(pProjectId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = false
          });
      }
  }
  #endregion

  #region Get Project Detail By Id
  [HttpGet]
  [Route("GetProjectDetailById")]
  public IActionResult GetProjectDetailById(int pProjectID)
  {
      ProjectsViewModel Projects = new ProjectsViewModel();
      try
      {
          Projects.ProjectDetails = _pmServices.GetProjectDetailById(pProjectID);
          Projects.ProjectDetailsCommentsList = _pmServices.GetProjectCommentsByProjectId(pProjectID);
          Projects.ChangeRequestList = _pmServices.GetChangeRequestDetailByProjectId(pProjectID);
          return Ok(new
          {
              StatusCode = "SUCCESS",
              StatusText = string.Empty,
              Data = Projects
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectDetailById", Convert.ToString(pProjectID));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = Projects
          });
      }
  }
  #endregion

  #region Get Project's Master Data
  [HttpGet]
  [Route("GetProjectsMasterData")]
  public IActionResult GetProjectsMasterData()
  {
      ProjectMasterData projectMasterData = new ProjectMasterData();
      try
      {
          projectMasterData.ListOfAllocations = _pmServices.GetAllAllocation();
          projectMasterData.ListOfCurrencyTypes = _pmServices.GetAllCurrencyType();
          projectMasterData.ListOfProjectTypes = _pmServices.GetAllProjectType();
          projectMasterData.ListOfRateFrequencys = _pmServices.GetAllRateFrequency();
          //projectMasterData.ListOfRequiredSkillSets = _pmServices.GetAllRequiredSkillSet();
          return Ok(new
          {
              StatusCode = "SUCCESS",
              StatusText = string.Empty,
              Data = projectMasterData
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectsMasterData");
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = projectMasterData
          });
      }
  }
  #endregion

  #region Get All Project Details By ResourceId
  [HttpPost]
  [Route("GetProjectDetailsByResourceId")]
  public IActionResult GetProjectDetailsByResourceId(ProjectCustomerEmployeeList listResourceId)
  {
      List<ProjectListView> projectListViews = new List<ProjectListView>();
      try
      {
          projectListViews = _pmServices.GetProjectDetailsByResourceId(listResourceId);
          return Ok(new
          {
              StatusCode = "SUCCESS",
              Data = projectListViews
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectDetailsByResourceId", JsonConvert.SerializeObject(listResourceId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = projectListViews
          });
      }
  }
  #endregion

  #region Get All Drafted Project Details By ResourceId
  [HttpGet]
  [Route("GetDraftedProjectDetailsByResourceId")]
  public IActionResult GetDraftedProjectDetailsByResourceId(int pResourceId)
  {
      List<ProjectListView> projectListViews = new List<ProjectListView>();
      try
      {
          projectListViews = _pmServices.GetDraftedProjectDetailsByResourceId(pResourceId);
          return Ok(new
          {
              StatusCode = "SUCCESS",
              Data = projectListViews
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetDraftedProjectDetailsByResourceId", Convert.ToString(pResourceId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = projectListViews
          });
      }
  }
  #endregion

  #region Approve or Reject the Project
  [HttpPost]
  [Route("ApproveOrRejectProject")]
  public IActionResult ApproveOrRejectProjectByProjectId(ApproveOrRejectProject pApproveOrRejectProject)
  {
      try
      {
          if (_pmServices.ApproveOrRejectProjectByProjectId(pApproveOrRejectProject).Result)
          {
              if (pApproveOrRejectProject.ProjectStatus == "Action Required")
                  return Ok(new
                  {
                      StatusCode = "SUCCESS",
                      StatusText = "Project change request(s) are saved successfully.",
                      Data = pApproveOrRejectProject.ProjectId
                  });
              else
                  return Ok(new
                  {
                      StatusCode = "SUCCESS",
                      StatusText = "Project details are approved successfully.",
                      Data = pApproveOrRejectProject.ProjectId
                  });
          }
          else
          {
              return Ok(new
              {
                  StatusCode = "FAILURE",
                  StatusText = "Unexpected error occurred. Try again.",
                  Data = 0
              });
          }
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/ApproveOrRejectProjectByProjectId", JsonConvert.SerializeObject(pApproveOrRejectProject));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = 0
          });
      }
  }
  #endregion

  #region Assign Project SPOC For Project
  [HttpPost]
  [Route("AssignProjectSPOCForProject")]
  public IActionResult AssignProjectSPOCForProject(UpdateProjectSPOC pUpdateProjectSPOC)
  {
      try
      {
          if (_pmServices.AssignProjectSPOCForProject(pUpdateProjectSPOC).Result)
          {
              return Ok(new
              {
                  StatusCode = "SUCCESS",
                  StatusText = "Project SPOC assigned successfully.",
                  Data = pUpdateProjectSPOC.ProjectId
              });
          }
          else
          {
              return Ok(new
              {
                  StatusCode = "FAILURE",
                  StatusText = "Unexpected error occurred. Try again.",
                  Data = 0
              });
          }
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/AssignProjectSPOCForProject", JsonConvert.SerializeObject(pUpdateProjectSPOC));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = 0
          });
      }
  }
  #endregion

  #region Assign Logo For Project
  [HttpPost]
  [Route("AssignLogoForProject")]
  public IActionResult AssignLogoForProject(UpdateProjectLogo pUpdateProjectLogo)
  {
      try
      {
          if (_pmServices.AssignLogoForProject(pUpdateProjectLogo).Result)
          {
              return Ok(new
              {
                  StatusCode = "SUCCESS",
                  StatusText = "Project Logo assigned successfully.",
                  Data = pUpdateProjectLogo.ProjectId
              });
          }
          else
          {
              return Ok(new
              {
                  StatusCode = "FAILURE",
                  StatusText = "Unexpected error occurred. Try again.",
                  Data = 0
              });
          }
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/AssignLogoForProject", JsonConvert.SerializeObject(pUpdateProjectLogo));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = 0
          });
      }
  }
  #endregion
  #endregion

 

  #region Delete Change Request Detail
  [HttpDelete]
  [Route("DeleteChangeRequestDetail")]
  public IActionResult DeleteChangeRequestDetail(int pChangeRequestID)
  {
      try
      {
          if (_pmServices.DeleteChangeRequestDetail(pChangeRequestID).Result)
          {
              return Ok(new { StatusCode = "SUCCESS", StatusText = "Change request deleted successfully.", Data = true });
          }
          else
          {
              return Ok(new { StatusCode = "FAILURE", StatusText = "Unexpected error occurred. Try again.", Data = false });
          }
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/DeleteChangeRequestDetail", Convert.ToString(pChangeRequestID));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = false
          });
      }
  }
  #endregion

  #region Approve or Reject the Change Request
  [HttpPost]
  [Route("ApproveOrRejectChangeRequest")]
  public IActionResult ApproveOrRejectChangeRequest(ApproveOrRejectChangeRequest pApproveOrRejectCR)
  {
      try
      {
          if (_pmServices.ApproveOrRejectChangeRequestById(pApproveOrRejectCR).Result)
          {
              if (pApproveOrRejectCR.ChangeRequestStatus == "Action Required")
                  return Ok(new { StatusCode = "SUCCESS", StatusText = "Change request(s) are saved successfully.", Data = pApproveOrRejectCR.ChangeRequestId });
              else
                  return Ok(new { StatusCode = "SUCCESS", StatusText = "Change request details are approved successfully.", Data = pApproveOrRejectCR.ChangeRequestId });
          }
          else
          {
              return Ok(new { StatusCode = "FAILURE", StatusText = "Unexpected error occurred. Try again.", Data = 0 });
          }
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/ApproveOrRejectChangeRequest", JsonConvert.SerializeObject(pApproveOrRejectCR));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = 0
          });
      }
  }
  #endregion


  #region Get Change Request Detail By Id
  [HttpGet]
  [Route("GetChangeRequestDetailById")]
  public IActionResult GetChangeRequestDetailById(int pCRID)
  {
      CRViewModel CRView = new CRViewModel();
      try
      {
          CRView.CRDetails = _pmServices.GetChangeRequestDetailById(pCRID);
          CRView.CRCommentsList = _pmServices.GetChangeRequestCommentsById(pCRID);
          return Ok(new { StatusCode = "SUCCESS", StatusText = string.Empty, Data = CRView });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetChangeRequestDetailById", Convert.ToString(pCRID));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = CRView
          });
      }
  }
  #endregion

  #region Get Change Request Detail By Project Id
  [HttpGet]
  [Route("GetChangeRequestDetailByProjectId")]
  public IActionResult GetChangeRequestDetailByProjectId(int pProjectID)
  {
      try
      {
          List<ChangeRequestView> changeRequests = _pmServices.GetChangeRequestDetailByProjectId(pProjectID);
          return Ok(new { StatusCode = "SUCCESS", changeRequests });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetChangeRequestDetailByProjectId", Convert.ToString(pProjectID));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again."
          });
      }
  }
  #endregion

  #region Get all change request type
  [HttpGet]
  [Route("GetAllChangeRequestType")]
  public IActionResult GetAllChangeRequestType()
  {
      List<ChangeRequestType> changeRequestTypes = new List<ChangeRequestType>();
      try
      {
          changeRequestTypes = _pmServices.GetAllChangeRequestType();
          return Ok(new { StatusCode = "SUCCESS", Data = changeRequestTypes });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetAllChangeRequestType");
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = changeRequestTypes
          });
      }
  }
  #endregion

  #region Get All Active Projects
  [HttpGet]
  [Route("GetAllActiveProjects")]
  public IActionResult GetAllActiveProjects()
  {
      List<ProjectListView> projectListViews = new List<ProjectListView>();
      try
      {
          projectListViews = _pmServices.GetAllActiveProjects();
          return Ok(new { StatusCode = "SUCCESS", StatusText = string.Empty, Data = projectListViews });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetAllActiveProjects");
          return Ok(new { StatusCode = "FAILURE", StatusText = ex.Message, Data = projectListViews });
      }
  }
  #endregion
  #endregion

  #region Resource Allocations


  #region Add Associates For Additional Resource
  [HttpPost]
  [Route("AddAssociatesForAdditionalresource")]
  public IActionResult AddAssociatesForAdditionalresource(ResourceAllocationList pResourceAllocation)
  {
      try
      {
          string result = _pmServices.AdditionalResourceAllocationDuplication(pResourceAllocation);
          if (result !="")
          {
              return Ok(new
              {
                  StatusCode = "FAILURE",
                  StatusText = result,
                  Data = 0
              });
          }
          else
          {
              if (_pmServices.AddAssociatesForAdditionalresource(pResourceAllocation).Result)
              {
                  return Ok(new
                  {
                      StatusCode = "SUCCESS",
                      StatusText = "Additional associate(s) are added successfully.",
                      Data = pResourceAllocation.ProjectId
                  });
              }
              else
              {
                  return Ok(new
                  {
                      StatusCode = "FAILURE",
                      StatusText = "Unexpected error occurred. Try again.",
                      Data = 0
                  });
              }
          }
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/AddAssociatesForAdditionalresource", JsonConvert.SerializeObject(pResourceAllocation));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = 0
          });
      }
  }
  #endregion


  #region Assign Associates For Resource Allocation
  [HttpPost]
  [Route("AssignAssociatesForResourceAllocation")]
  public IActionResult AssignAssociatesForResourceAllocation(UpdateResourceAllocation pUpdateResourceAllocation)
  {
      try
      {
          string data = _pmServices.ResourceAllocationDuplication(pUpdateResourceAllocation);
          if (data !="")
          {
              return Ok(new
              {
                  StatusCode = "FAILURE",
                  StatusText = data,
                  Data = 0
              });
          }
          else
          {
              if (_pmServices.AssignAssociatesForResourceAllocation(pUpdateResourceAllocation).Result)
              {
                  return Ok(new
                  {
                      StatusCode = "SUCCESS",
                      StatusText = "Resource allocation(s) are updated successfully.",
                      Data = pUpdateResourceAllocation.ProjectId
                  });
              }
              else
              {
                  return Ok(new { StatusCode = "FAILURE", StatusText = "Unexpected error occurred. Try again.", Data = 0 });
              }
          }
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/AssignAssociatesForResourceAllocation", JsonConvert.SerializeObject(pUpdateResourceAllocation));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = 0
          });
      }
  }
  #endregion

  #region Remove Associates
  [HttpPost]
  [Route("RemoveAssociates")]
  public IActionResult RemoveAssociates(UpdateResourceAllocation pUpdateResourceAllocation)
  {
      try
      {
          if (_pmServices.RemoveAssociates(pUpdateResourceAllocation).Result)
          {
              return Ok(new
              {
                  StatusCode = "SUCCESS",
                  StatusText = "Associate(s) removed successfully.",
                  Data = pUpdateResourceAllocation.ProjectId
              });
          }
          else
          {
              return Ok(new { StatusCode = "FAILURE", StatusText = "Unexpected error occurred. Try again.", Data = 0 });
          }
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/RemoveAssociates", JsonConvert.SerializeObject(pUpdateResourceAllocation));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = 0
          });
      }
  }
  #endregion

  #region Delete Resource Allocation
  [HttpDelete]
  [Route("DeleteResourceAllocation")]
  public IActionResult DeleteResourceAllocation(int ResourceAllocationId)
  {
      try
      {
          if (_pmServices.DeleteResourceAllocation(ResourceAllocationId).Result)
          {
              return Ok(new { StatusCode = "SUCCESS", StatusText = "", Data = true });
          }
          else
          {
              return Ok(new { StatusCode = "FAILURE", StatusText = "Unexpected error occurred. Try again.", Data = false });
          }
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/DeleteResourceAllocation", Convert.ToString(ResourceAllocationId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = false
          });
      }
  }
  #endregion        
  #endregion

  #region Get project details by account id
  [HttpPost]
  [Route("GetProjectDetailByaccountId")]
  public IActionResult GetProjectDetailByaccountId(List<int?> lstAccountId)
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetProjectDetailByAccountId(lstAccountId),
                  Resource = _pmServices.GetResourceAllocationByAccountId(lstAccountId)
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectDetailByaccountId", JsonConvert.SerializeObject(lstAccountId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new List<ProjectDetails>(),
              ResourceCount = new List<ResourceAllocation>()
          });
      }
  }
  #endregion

  #region Get resource project list
  [HttpGet]
  [Route("GetResourceProjectList")]
  public IActionResult GetResourceProjectList(int ResourceId = 0)
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetResourceProjectList(ResourceId)
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetResourceProjectList", Convert.ToString(ResourceId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new List<ResourceProjectList>()
          });
      }
  }
  #endregion

  #region Get Reporting Person TeamMember
  [HttpGet]
  [Route("GetReportingPersonTeamMember")]
  public IActionResult GetReportingPersonTeamMember(int resourceId, DateTime? weekStartDay)
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetReportingPersonTeamMember(resourceId, weekStartDay)
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetReportingPersonTeamMember", " ResorceId- " + resourceId.ToString() + " WeekStartDay- " + weekStartDay.ToString());
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new List<TeamMemberDetails>()
          });
      }
  }
  #endregion

  #region Get project timesheet
  [HttpGet]
  [Route("GetProjectTimesheet")]
  public IActionResult GetProjectTimesheet(int resourceId)
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetProjectTimesheet(resourceId)
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectTimesheet", Convert.ToString(resourceId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new ProjectTimesheet()
          });
      }
  }
  #endregion

  #region Get project spoc by project id
  [HttpPost]
  [Route("GetProjectSPOCByProjectId")]
  public IActionResult GetProjectSPOCByProjectId(List<int> lstProjectId)
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetProjectSPOCByProjectId(lstProjectId)
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectSPOCByProjectId", JsonConvert.SerializeObject(lstProjectId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new List<ProjectSPOC>()
          });
      }
  }
  #endregion

  #region check team members by resource id
  [HttpGet]
  [Route("CheckTeamMembersByResourceId")]
  public IActionResult CheckTeamMembersByResourceId(int resourceId)
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.CheckTeamMembersByResourceId(resourceId)
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/CheckTeamMembersByResourceId", Convert.ToString(resourceId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = false
          });
      }
  }
  #endregion

  #region Get all project details
  [HttpGet]
  [Route("GetAllProjectsDetails")]
  public IActionResult GetAllProjectsDetails()
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetAllProjectsDetails()
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetAllProjectsDetails");
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new List<ProjectDetails>()
          });
      }
  }
  #endregion

  #region Get resource project report
  [HttpGet]
  [Route("GetResourceProjectData")]
  public IActionResult GetResourceProjectData()
  {
      try
      {

          ResourceReportMasterData ResourceReportMasterData = new ResourceReportMasterData()
          {
              ProjectDetails = _pmServices.GetAllProjectsDetails(),
              ResourceAllocation = _pmServices.GetAllResourceAllocation(),
              Allocation = _pmServices.GetAllAllocation()
              //RequiredSkillSets = _pmServices.GetAllRequiredSkillSet()

          };
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = ResourceReportMasterData
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetResourceProjectData");
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new ResourceReportMasterData()
          });
      }
  }
  #endregion

  #region Get project name by id
  [HttpPost]
  [Route("GetProjectNameById")]
  public IActionResult GetProjectNameById(List<int> lstProjectId)
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetProjectNameById(lstProjectId)
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectNameById", JsonConvert.SerializeObject(lstProjectId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new List<ProjectNames>()
          });
      }
  }
  #endregion

  #region Get project by id
  [HttpGet]
  [Route("GetProjectById")]
  public IActionResult GetProjectById(int pProjectID)
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetProjectById(pProjectID)
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectById", Convert.ToString(pProjectID));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new ProjectDetails()
          });
      }
  }
  #endregion

  #region Get Change Request by id
  [HttpGet]
  [Route("GetChangeRequestById")]
  public IActionResult GetChangeRequestById(int pCRID)
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetChangeRequestById(pCRID)
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetChangeRequestById", Convert.ToString(pCRID));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new ChangeRequest()
          });
      }
  }
  #endregion

  #region Get resource utilisation Data
  [HttpGet]
  [Route("GetResourceUtilisationData")]
  public IActionResult GetResourceUtilisationData()
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetResourceUtilisationData()
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetResourceUtilisationData");
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new List<ReportData>()
          });
      }
  }
  #endregion 

  #region Get project details by account id
  [HttpGet]
  [Route("GetProjectDetailsByAccountId")]
  public IActionResult GetProjectDetailsByAccountId(int pAccountId)
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetProjectDetailsByAccountId(pAccountId)
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectDetailsByAccountId", Convert.ToString(pAccountId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new List<ProjectDetailView>()
          });
      }
  }
  #endregion 

  #region Get project allocated active resource list
  [HttpGet]
  [Route("GetProjectAllocatedResourceList")]
  public IActionResult GetProjectAllocatedResourceList()
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetProjectAllocatedResourceList()
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectAllocatedResourceList");
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new List<int>()
          });
      }
  }
  #endregion

  #region Get Project Report Master Data
  [HttpGet]
  [Route("GetProjectReportMasterData")]
  public IActionResult GetProjectReportMasterData()
  {
      try
      {
          ProjectReportMasterData data = new ProjectReportMasterData();
          data.ProjectDetails = _pmServices.GetAllProjectsDetails();
          data.ProjectType = _pmServices.GetAllProjectType();
          data.ResourceAllocation = _pmServices.GetAllResourceAllocation();
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = data
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectReportMasterData");
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new ProjectReportMasterData()
          });
      }
  }
  #endregion

  #region Get Contribution Home Report
  [HttpGet]
  [Route("GetContributionHomeReport")]
  public IActionResult GetContributionHomeReport(int employeeId)
  {
      try
      {
          return Ok(
              new
              {
                  StatusCode = "SUCCESS",
                  StatusText = string.Empty,
                  Data = _pmServices.GetContributionHomeReport(employeeId)
              });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetContributionHomeReport");
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = 0
          });
      }
  }
  #endregion

  #region Get Project Detils By Resource Id
  [HttpGet]
  [Route("GetProjectsByResourceId")]
  public IActionResult GetProjectsByResourceId(int pResourceId)
  {
      List<ProjectNames> projectListViews = new List<ProjectNames>();
      try
      {
          projectListViews = _pmServices.GetProjectsByResourceId(pResourceId);
          return Ok(new
          {
              StatusCode = "SUCCESS",
              Data = projectListViews
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectsByResourceId", JsonConvert.SerializeObject(pResourceId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = projectListViews
          });
      }
  }
  #endregion

  #region Get Project & Employee Detils By Resource Id
  [HttpGet]
  [Route("GetProjectsAndEmpByResourceId")]
  public IActionResult GetProjectsAndEmpByResourceId(int pResourceId)
  {
      List<EmployeeProjectNames> projectListViews = new List<EmployeeProjectNames>();
      try
      {
          projectListViews = _pmServices.GetProjectsAndEmpByResourceId(pResourceId);
          return Ok(new
          {
              StatusCode = "SUCCESS",
              Data = projectListViews
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectsAndEmpByResourceId", JsonConvert.SerializeObject(pResourceId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = projectListViews
          });
      }
  }
  #endregion 

  #region Get Project Detils By Resource Id
  [HttpGet]
  [Route("GetActiveProjectDetailsByResourceId")]
  public IActionResult GetActiveProjectDetailsByResourceId(int resourceId)
  {
      try
      {
          return Ok(new
          {
              StatusCode = "SUCCESS",
              Data = _pmServices.GetActiveProjectDetailsByResourceId(resourceId)
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetActiveProjectDetailsByResourceId", JsonConvert.SerializeObject(resourceId));
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = new List<ProjectDetails>()
          });
      }
  }
  #endregion

  #region Get Projects By Resource Id
  [HttpGet]
  [Route("GetProjectsByEmployeeId")]
  public IActionResult GetProjectsByEmployeeId(int pResourceId)
  {
      List<EmployeeProjectNames> projectListViews = new List<EmployeeProjectNames>();
      try
      {
          projectListViews = _pmServices.GetProjectsByEmployeeId(pResourceId);
          return Ok(new
          {
              StatusCode = "SUCCESS",
              Data = projectListViews
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectsByEmployeeId", "ResorceId" + pResourceId.ToString());
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = projectListViews
          });
      }
  }
  #endregion

  #region Get Resource Availability Details By Resource Id
  [HttpGet]
  [Route("GetResourceAvailabilityEmployeeDetails")]
  public IActionResult GetResourceAvailabilityEmployeeDetails(int pResourceId)
  {
      List<ResourceAvailability> projectListViews = new List<ResourceAvailability>();
      try
      {
          projectListViews = _pmServices.GetResourceAvailabilityEmployeeDetails(pResourceId);
          return Ok(new
          {
              StatusCode = "SUCCESS",
              Data = projectListViews
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetResourceAvailabilityEmployeeDetails", "ResorceId" + pResourceId.ToString());
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = projectListViews
          });
      }
  }
  #endregion

  #region Get Resource Billability Home Report
  [HttpGet]
  [Route("GetResourceBillabilityHomeReport")]
  public IActionResult GetResourceBillabilityHomeReport()
  {
      HomeReportData resourceData = new HomeReportData();
      try
      {
          resourceData = _pmServices.GetResourceBillabilityHomeReport();
          return Ok(new
          {
              StatusCode = "SUCCESS",
              Data = resourceData
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetResourceBillabilityHomeReport");
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = resourceData
          });
      }
  }
  #endregion

  #region Get Resource Availability Home Report
  [HttpGet]
  [Route("GetResourceAvailabilityHomeReport")]
  public IActionResult GetResourceAvailabilityHomeReport()
  {
      HomeReportData resourceList = new HomeReportData();
      try
      {
          resourceList = _pmServices.GetResourceAvailabilityHomeReport();
          return Ok(new
          {
              StatusCode = "SUCCESS",
              Data = resourceList
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetResourceAvailabilityHomeReport");
          return Ok(new
          {
              StatusCode = "FAILURE",
              StatusText = "Unexpected error occurred. Try again.",
              Data = resourceList
          });
      }
  }
  #endregion

  #region Remove Customer Logo
  [HttpPost]
  [Route("RemoveProjectLogo")]
  public IActionResult RemoveProjectLogo(RemoveProjectLogo removeProjectLogo)
  {
      try
      {
          if (_pmServices.RemoveProjectLogo(removeProjectLogo.ProjectId).Result)
          {
              return Ok(new
              {
                  StatusCode = "SUCCESS",
                  StatusText = "Project Logo removed successfully."
              });
          }
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/RemoveProjectLogo", JsonConvert.SerializeObject(removeProjectLogo));
      }
      return Ok(new
      {
          StatusCode = "FAILURE",
          StatusText = "Unexpected error occurred. Try again."
      });
  }
  #endregion

  #region Get account id by buhead id
  [HttpGet]
  [Route("GetAccountIdByBUHeadId")]
  public IActionResult GetAccountIdByBUHeadId(int resourceId)
  {
      try
      {
          return Ok(new
          {
              StatusCode = "SUCCESS",
              StatusText = "Project Logo removed successfully.",
              Data = _pmServices.GetAccountIdByBUHeadId(resourceId)
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetAccountIdByBUHeadId", JsonConvert.SerializeObject(resourceId));
      }
      return Ok(new
      {
          StatusCode = "FAILURE",
          StatusText = "Unexpected error occurred. Try again.",
          Data=new List<int?>()
      });
  }
  #endregion

 
        #region Get account id by buhead id
  [HttpPost]
  [Route("GetEmployeeProjectListById")]
  public IActionResult GetEmployeeProjectListById(AppraisalWorkDayFilterView appraisalWorkDayFilterView)
  {
      try
      {
          return Ok(new
          {
              StatusCode = "SUCCESS",
              Data = _pmServices.GetEmployeeProjectListById(appraisalWorkDayFilterView)
          });
      }
      catch (Exception ex)
      {
          LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetEmployeeProjectListById", JsonConvert.SerializeObject(appraisalWorkDayFilterView));
      }
      return Ok(new
      {
          StatusCode = "FAILURE",
          StatusText = "Unexpected error occurred. Try again.",
          Data = new List<EmployeeProjectNames>()
      });
  }
        #endregion

        #endregion*/




        /*   #region Get List of  Customer Conatct details from Customer module.
           [HttpGet]
           [Route("GetCustomerContactDetails")]
           public IActionResult GetCustomerContactDetails()
           {
               List<CustomerContactDetails> CustomerContactDetailsList = new List<CustomerContactDetails>();
               try
               {
                   CustomerContactDetailsList = _pmServices.GetCustomerContactDetails();
                   return Ok(new
                   {
                       StatusCode = "SUCCESS",
                       Data = CustomerContactDetailsList
                   });
               }
               catch (Exception ex)
               {
                   LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectsByAccountManagerId", "ResourceId" + CustomerContactDetailsList.ToString());
                   return Ok(new
                   {
                       StatusCode = "FAILURE",
                       StatusText = "Unexpected error occurred. Try again.",
                       Data = CustomerContactDetailsList
                   }) ;

               }
           }
           #endregion*/



        /*   #region Create New Project 
        [HttpPost]
        [Route("createProject/")]
        public IActionResult CreateProjectDetails(ProjectDetails pProjectDetails)
        {
           ProjectDetails projectDetails = new ProjectDetails();
            try
            {
                projectDetails = _pmServices.CreateProjectDetails(pProjectDetails);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = projectDetails
                }) ;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectsByAccountManagerId", "ResourceId" + Id.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again.",
                    Data = projectDetails
                });

            }
        }
        #endregion*/

        /*  #region Show All Iteration
     [HttpGet]
     [Route("ShowAllIteration")]
     public IActionResult ShowAllIteration(int noOfIterations, int projectId)
     {
         try
         {
             List<FixedIteration> fixedIteration = _pmServices.ShowAllIteration(noOfIterations, projectId);
             if (fixedIteration?.Count > 0)
             {
                 return Ok(new
                 {
                     StatusCode = "SUCCESS",
                     StatusText = "Change request saved successfully.",
                     Data = fixedIteration
                 });
             }
             else
             {
                 return Ok(new
                 {
                     StatusCode = "FAILURE",
                     StatusText = "Unexpected error occurred. Try again.",
                     Data = fixedIteration
                 }) ;
             }
         }
         catch (Exception ex)
         {
             LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/ShowAllIteration", JsonConvert.SerializeObject(projectId));
             return Ok(new
             {
                 StatusCode = "FAILURE",
                 StatusText = "Unexpected error occurred. Try again.",
                 Data = 0
             });
         }
     }
     #endregion*/

        /*
        #region Get List of all the projects for Account Manager Id
        [HttpGet]
        [Route("GetProjectsByAccountManagerId/{Id}")]
        public IActionResult GetProjectsByAccountManagerId(int Id)
        {
            List<ProjectDetails> projectListViews = new List<ProjectDetails>();
            try
            {
                projectListViews = _pmServices.GetProjectsByAccountManagerId(Id);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = projectListViews
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectsByAccountManagerId", "ResourceId" + Id.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again.",
                    Data = projectListViews
                });

            }
        }
        #endregion*/


        #region Get List of project details by EngineeringLeadId
        [HttpGet]
        [Route("GetProjectsByEngineeringLeadId/{employeeId}")]
        public IActionResult GetProjectsByEngineeringLeadId(int employeeId)
        {
            List<ProjectDetails> projectListViews = new List<ProjectDetails>();
            try
            {
                projectListViews = _pmServices.GetProjectsByEngineeringLeadId(employeeId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = projectListViews
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/GetProjectsByProjectleadId", "ResourceId" + employeeId.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again.",
                    Data = projectListViews
                });
            }
        }
        #endregion




        #region Insert or Update Change Request Detail
        [HttpPost]
        [Route("InsertOrUpdateChangeRequestDetail")]
        public IActionResult InsertOrUpdateChangeRequestDetail(ChangeRequestView pChangeRequestDetails)
        {
            try
            {
                int ChangeRequestId = _pmServices.InsertOrUpdateChangeRequestDetail(pChangeRequestDetails).Result;
                if (ChangeRequestId > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Change request saved successfully.",
                        Data = ChangeRequestId
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = ChangeRequestId = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/InsertOrUpdateChangeRequestDetail", JsonConvert.SerializeObject(pChangeRequestDetails));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again.",
                    Data = 0
                });
            }
        }
        #endregion



       
        #region Insert And Update
        [HttpPost]
        [Route("InsertUpdateProject")]
        public IActionResult InsertAndUpdateProject(ProjectDetailView pProjectDetails)
        {
            try
            {
                if ((_pmServices.ProjectNameDuplication(pProjectDetails) && (pProjectDetails.ProjectId) == 0))
                {
                    return Ok(new
                   {
                        StatusCode = "FAILURE",
                        StatusText = "Project Name is already exists. Please change your project name and try again.",
                        Data = 0
                    });
                }
                else
                {
                    int ProjectId = _pmServices.InsertandUpdateProject(pProjectDetails).Result;
                    if (ProjectId > 0)
                    {
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = (pProjectDetails.IsDraft == true ? "Project drafted successfully." : "Project approval request sent successfully."),
                            Data = ProjectId
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Unexpected error occurred. Try again.",
                            Data = 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/InsertAndUpdateProject", JsonConvert.SerializeObject(pProjectDetails));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again.",
                    Data = 0
                });
            }
        }
        #endregion




        #region Create Iterations
        [HttpPost]
        [Route("CreateIterations")]
        public IActionResult CreateIterations(int noOfIterations, int projectId)
        {
            try
            {
                List<FixedIteration> fixedIteration = _pmServices.CreateIterations(noOfIterations, projectId);
                if (fixedIteration?.Count > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Change request saved successfully.",
                        Data = fixedIteration
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = fixedIteration
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/CreateIterations", JsonConvert.SerializeObject(projectId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again.",
                    Data = 0
                });
            }
        }
        #endregion


        #region Approve or Reject the Project by Finance Head 
        [HttpPost]
        [Route("ApproveOrRejectProject")]
        public IActionResult ApproveOrRejectProjectByFinanceHead(ApproveOrRejectProject pApproveOrRejectProject)
        {
            try
            {
                if (_pmServices.ApproveOrRejectProjectByFinanceHead(pApproveOrRejectProject).Result)
                {
                    // if rejected else approved
                    if (pApproveOrRejectProject.ProjectStatus == "Action Required")
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = "Project change request(s) are saved successfully.",
                            Data = pApproveOrRejectProject.ProjectId
                        });
                    else

                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = "Project details are approved successfully.",
                            Data = pApproveOrRejectProject.ProjectId
                        });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Projects", "Projects/ApproveOrRejectProject", JsonConvert.SerializeObject(pApproveOrRejectProject));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again.",
                    Data = 0
                });
            }
        }
        #endregion


    }



}


    

