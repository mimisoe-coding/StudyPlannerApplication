namespace StudyPlannerApplication.App.Services
{
    public interface IInjectService
    {
        Task SuccessMessage(string message);
        Task ErrorMessage(string message);
        Task<bool> ConfirmMessageBox(string message);
    }
}
