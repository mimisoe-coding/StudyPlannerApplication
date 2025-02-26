namespace StudyPlannerApplication.Domain.Features.Dashboard;

public class DashboardRequestModel:BaseRequestModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
