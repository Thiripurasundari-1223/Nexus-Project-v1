using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Appraisal
{
    public class AppraisalMaster
    {
        [Key]
        public int APP_CYCLE_ID { get; set; }
        public int? ENTITY_ID { get; set; }
        public int? VERSION_ID { get; set; }
        public string APP_CYCLE_NAME { get; set; }
        public string APP_CYCLE_DESC { get; set; }
        public DateTime? APP_CYCLE_START_DATE { get; set; }
        public DateTime? APP_CYCLE_END_DATE { get; set; }
        public DateTime? APPRAISEE_REVIEW_START_DATE { get; set; }
        public DateTime? APPRAISEE_REVIEW_END_DATE { get; set; }
        public DateTime? APPRAISER_REVIEW_START_DATE { get; set; }
        public DateTime? APPRAISER_REVIEW_END_DATE { get; set; }
        public DateTime? MGMT_REVIEW_START_DATE { get; set; }
        public DateTime? MGMT_REVIEW_END_DATE { get; set; }
        public int? APPRAISAL_STATUS { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int? EmployeesTypeId { get; set; }
        public int? DURATION_ID { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
    public class AppraisalMasterView
    {
        [Key]
        public int? APP_CYCLE_ID { get; set; }
        public int? ENTITY_ID { get; set; }
        public string ENTITY_NAME { get; set; }
        public int? VERSION_ID { get; set; }
        public string VERSION_NAME { get; set; }
        public string APP_CYCLE_NAME { get; set; }
        public string APP_CYCLE_DESC { get; set; }
        public DateTime? APP_CYCLE_START_DATE { get; set; }
        public DateTime? APP_CYCLE_END_DATE { get; set; }
        public DateTime? APPRAISEE_REVIEW_START_DATE { get; set; }
        public DateTime? APPRAISEE_REVIEW_END_DATE { get; set; }
        public DateTime? APPRAISER_REVIEW_START_DATE { get; set; }
        public DateTime? APPRAISER_REVIEW_END_DATE { get; set; }
        public DateTime? MGMT_REVIEW_START_DATE { get; set; }
        public DateTime? MGMT_REVIEW_END_DATE { get; set; }
        public bool APPRAISAL_STATUS { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int? EmployeesTypeId { get; set; }
        public int? DURATION_ID { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
}