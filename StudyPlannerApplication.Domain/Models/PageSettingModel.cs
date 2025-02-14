namespace StudyPlannerApplication.Domain.Models;

public class PageSettingModel
{
    public PageSettingModel() { }
    public PageSettingModel(int pageNo, int pageSize)
    {
        PageNo = pageNo;
        PageSize = pageSize;
    }
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int SkipRowCount => (PageNo - 1) * PageSize;
}
