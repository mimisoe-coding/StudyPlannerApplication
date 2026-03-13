using System;

namespace StudyPlannerApplication.App.Services
{
    public class PageHeaderService
    {
        public string Title { get; private set; } = "Dashboard";
        public string Subtitle { get; private set; } = string.Empty;
        public string Color { get; private set; } = "#3B82F6"; // Default blue
        public string Icon { get; private set; } = string.Empty;

        public event Action? OnChange;

        public void SetHeader(string title, string subtitle = "", string color = "#3B82F6", string icon = "")
        {
            Title = title;
            Subtitle = subtitle;
            Color = color;
            Icon = icon;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
