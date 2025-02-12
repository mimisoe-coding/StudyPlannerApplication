using Microsoft.AspNetCore.Components;
using StudyPlannerApplication.App.Services.Security;

namespace StudyPlannerApplication.App.Components.Pages.Course
{
    public partial class P_Course
    {
        [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
                var authState = await customAuthStateProvider.GetAuthenticationStateAsync();
                if (authState.User.Identity != null && !authState.User.Identity.IsAuthenticated)
                {
                    _nav.NavigateTo("/");
                    return;
                }

                StateHasChanged();
            }
        }
    }
}
