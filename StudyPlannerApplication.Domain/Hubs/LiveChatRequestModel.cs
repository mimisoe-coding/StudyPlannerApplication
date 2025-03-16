namespace StudyPlannerApplication.Domain.Hubs;

public class LiveChatRequestModel
{
    public string LiveChatGroupId { get; set; } 
    public string UserName { get; set; }    
    public string Message { get; set; }
    public string ImageUrl { get; set; } = "/images/profile.jpg";
    public DateTime CreatedDate { get; set; }
}
