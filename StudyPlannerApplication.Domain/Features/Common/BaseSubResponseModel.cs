namespace StudyPlannerApplication.Domain.Features.Common;

public class ResponseModel
{
    
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
}

public static class SubResponseModel
{
    public static ResponseModel GetResponseMsg(string message, bool isSuccess)
    {
        return new ResponseModel()
        {
            Message = message,
            IsSuccess = isSuccess   
        };
    }
}
