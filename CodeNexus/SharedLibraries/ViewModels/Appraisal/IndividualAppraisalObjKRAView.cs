using System;
using System.Collections.Generic;
using System.Text;


namespace SharedLibraries.ViewModels.Appraisal
{
    public class IndividualAppraisalObjKRAView
    {
        public int? EntityId { get; set; }
        public string EntityName { get; set; }
        public int ObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
        public string ObjectiveDescription { get; set; }
        public decimal ObjectiveWeightage { get; set; }
        public decimal? ObjectiveRating { get; set; }
        public decimal? ObjectiveMaxRating { get; set; }
        public int? ApprovedKeyResultCount { get; set; }
        public int? RejectedKeyResultCount { get; set; }
        public List<IndividualKRAView> IndividualKRAView { get; set; }

        public List<IndividualBenchmarkKeyResultGroup> IndividualBenchmarkKeyResultGroup { get; set; }
    }


    public class IndividualKRAView
    {

        public int VersionId { get; set; }
        public int Dept_Id { get; set; }
        public int Role_Id { get; set; }
        public int Objective_Id { get; set; }
        public int KeyResult_Id { get; set; }
        public bool? IsSelected { get; set; }
        public string KeyResult_Name { get; set; }
        public decimal? Key_Result_Weightage { get; set; }
        public int? Benchmark_Type { get; set; }
        public string Benchmark_Type_Name { get; set; }
        public int? Benchmark_Duration { get; set; }
        public int? Benchmark_Operator { get; set; }
        public string Benchmark_Operator_Name { get; set; }
        public decimal? Benchmark_Value { get; set; }
        public decimal? Benchmark_From_Value { get; set; }
        public decimal? Benchmark_To_Value { get; set; }
        public int? Updated_By { get; set; }
        public int? Benchmark_UIType { get; set; }
        public string Benchmark_UIType_Name { get; set; }
        public decimal? KEY_RESULT_ACTUAL_VALUE { get; set; }
        public decimal? KEY_RESULT_MAX_RATING { get; set; }
        public decimal? KEY_RESULT_RATING { get; set; }
        public decimal? GRP_KEYRES_ACTUAL_VALUE { get; set; }
        public decimal? INDIVIDUAL_GRPITEM_RATING { get; set; }
        public decimal? KEY_RESULTS_GROUP_RATING { get; set; }
        public decimal? KEY_RESULTS_GROUP_MAX_RATING { get; set; }
        public int? KEY_RESULT_STATUS { get; set; }
        public string KEY_RESULT_STATUS_NAME { get; set; }
        public List<DocList> DocList { get; set; }
        public List<IndividualKRABenchmark> IndividualKRABenchmark { get; set; }
        public KRAComments kraComments { get; set; }
        public bool? IS_DOCUMENT_MANDATORY { get; set; }
    }
    public class IndividualKRABenchmark
    {
        public int BenchmarkId { get; set; }
        public decimal? BenchmarkValue { get; set; }
        public decimal? RangeFrom { get; set; }
        public decimal? RangeTo { get; set; }
        public decimal BenchmarkWeightage { get; set; }
        public string BenchmarkSubjectiveName { get; set; }
        public string BenchmarkValue_Name { get; set; }
    }

    public class IndividualBenchmarkKeyResultGroup
    {
        public int VersionId { get; set; }
        public int DeptId { get; set; }
        public int RoleId { get; set; }
        public int ObjectiveId { get; set; }
        public int KeyResultGroupId { get; set; }
        public bool? IsSelected { get; set; }
        public string KeyResultGroupName { get; set; }
        public decimal? KeyResultGroupWeightage { get; set; }
        public int? MandatoryKeyResultOption { get; set; }
        public int? MandatoryKeyResultOperator { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        //public int? Benchmark_UIType { get; set; }
        //public string Benchmark_UIType_Name { get; set; }
        public int? Benchmark_Type { get; set; }
        public string Benchmark_Type_Name { get; set; }
        public List<IndividualKRAView> KeyResultDetail { get; set; }
    }
    public class DocList
    {
        public int? KEY_RESULTS_DOC_ID { get; set; }
        public string KEY_RESULTS_DOC_NAME { get; set; }
        public string DOC_TYPE { get; set; }

    }
    public class KRAComments
    {
        public string KEY_RESULTS_COMMENT { get; set; }
        public int? KEY_RESULTS_COMMENT_CREATEDBY { get; set; }
    }
}

