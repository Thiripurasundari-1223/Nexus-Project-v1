using System;
using System.Collections.Generic;
using System.Text;
using SharedLibraries.Models.Appraisal;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class AppraisalCycleMasterData
    {
        public int? APP_CYCLE_ID { get; set; }
        public string APP_CYCLE_NAME { get; set; }
        public bool? ISACTIVE_APPRAISAL { get; set; }
        public DateTime? APPRAISEE_REVIEW_START_DATE { get; set; }
        public DateTime? APPRAISEE_REVIEW_END_DATE { get; set; }
        public DateTime? APPRAISER_REVIEW_START_DATE { get; set; }
        public DateTime? APPRAISER_REVIEW_END_DATE { get; set; }
        public DateTime? MGMT_REVIEW_START_DATE { get; set; }
        public DateTime? MGMT_REVIEW_END_DATE { get; set; }
        public int? EMPLOYEE_DEPARTMENT_ID { get; set; }
        public int? EMPLOYEE_ROLE_ID { get; set; }
    }


    public class IndividualAppraisalMasterData
    {

        public int? APP_CYCLE_ID { get; set; }
        public string APP_CYCLE_NAME { get; set; }
        public int? VERSION_ID { get; set; }
        public int? ENTITY_ID { get; set; }
        public int? DEPT_ID { get; set; }
        public int? ROLE_ID { get; set; }
        //public int? OBJECTIVE_ID { get; set; }
        //public int? KEY_RESULT_ID { get; set; }
        //public string APP_CYCLE_NAME { get; set; }
        //public DateTime? APP_CYCLE_START_DATE { get; set; }
        //public DateTime? APP_CYCLE_END_DATE { get; set; }
        //public string OBJECTIVE_NAME { get; set; }
        //public string KEY_RESULT_NAME { get; set; }
        //public decimal? KEY_RESULT_WEIGHTAGE { get; set; }
        //public int? BENCHMARK_TYPE { get; set; }
        //public int? BENCHMARK_DURATION { get; set; }
        //public int? BENCHMARK_OPERATOR { get; set; }
        //public decimal? BENCHMARK_VALUE { get; set; }
        //public decimal? BENCHMARK_FROM_VALUE { get; set; }
        //public decimal? BENCHMARK_TO_VALUE { get; set; }
        //public List<ObjectiveMaster> ObjectiveMaster { get; set; }

    }

    public class  IndividualCommanData
    {
        public int? APP_CYCLE_ID { get; set; }
        public int? VERSION_ID { get; set; }
        public int? ENTITY_ID { get; set; }
        public int? DEPT_ID { get; set; }
        public int? ROLE_ID { get; set; }

    }

    public class IndividualMasterData
    {
        public IndividualCommanData IndividualCommanData { get; set; }
        public List<KeyResultMaster> KeyResultMasters { get; set; }
        public List<ObjectiveMaster> ObjectiveMaster { get; set; }
        
    }
    public class IndividualAppraisalCycleMasterData
    {
        public List<IndividualAppraisalMasterData> IndividualAppraisalMasterData { get; set; }
        public List<IndividualAppraisalObjectiveMasterData> IndividualAppraisalObjectiveMasterData { get; set; }
        public List<IndividualAppraisalKraMasterData> IndividualAppraisalKraMasterData { get; set; }

    }
    public class IndividualAppraisalObjectiveMasterData
    {
        public int? OBJECTIVE_ID { get; set; }
        public string OBJECTIVE_NAME { get; set; }
    }
    public class IndividualAppraisalKraMasterData
    {
        public int? OBJECTIVE_ID { get; set; }
        public int? KEY_RESULT_ID { get; set; }
        public string KEY_RESULT_NAME { get; set; }
        public decimal? KEY_RESULT_WEIGHTAGE { get; set; }
        public int? BENCHMARK_TYPE { get; set; }
        public int? BENCHMARK_DURATION { get; set; }
        public int? BENCHMARK_OPERATOR { get; set; }
        public decimal? BENCHMARK_VALUE { get; set; }
        public decimal? BENCHMARK_FROM_VALUE { get; set; }
        public decimal? BENCHMARK_TO_VALUE { get; set; }
    }
}
