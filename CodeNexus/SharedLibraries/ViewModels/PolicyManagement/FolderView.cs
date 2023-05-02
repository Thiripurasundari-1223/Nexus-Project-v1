namespace SharedLibraries.ViewModels.PolicyManagement
{
    public class FolderView
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}