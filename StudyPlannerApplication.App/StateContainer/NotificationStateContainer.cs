namespace StudyPlannerApplication.App.StateContainer;

public class NotificationStateContainer
{
    private int _notificationCount = 0;

    public int NotificationCount
    {
        get => _notificationCount;
        set
        {
            _notificationCount = value;
            NotifyStateChanged();

        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
