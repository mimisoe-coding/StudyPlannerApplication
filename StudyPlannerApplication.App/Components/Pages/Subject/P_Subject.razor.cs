using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using StudyPlannerApplication.Domain.Features.Subject;

namespace StudyPlannerApplication.App.Components.Pages.Subject;

public partial class P_Subject
{
    private EnumFormType _formType = EnumFormType.List;
    private SubjectRequestModel _reqModel = new();
    private UserSessionModel _userSession = new();
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
            _userSession = await customAuthStateProvider.GetUserData();
            StateHasChanged();
        }
    }

    private void Create()
    {
        _formType = EnumFormType.Register;
        _reqModel = new SubjectRequestModel();
    }

    async Task Back()
    {
        _formType = EnumFormType.List;
        _reqModel = new SubjectRequestModel();
        await List();
        StateHasChanged();
    }

    async Task Save()
    {
        if (!await CheckRequiredFields(_reqModel)) return;
        _reqModel.CurrentUserId = _userSession.UserId;
        var model = await _subjectService.Create(_reqModel);
        if (!model.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(model.Response.Message);
            return;
        }
        await _injectService.SuccessMessage(model.Response.Message);
        await List();   
    }

    async Task List()
    {
        _formType=EnumFormType.List;
    }

    async Task<bool> CheckRequiredFields(SubjectRequestModel _reqModel)
    {
        if (string.IsNullOrEmpty(_reqModel.SubjectName))
        {
            await _injectService.ErrorMessage("SubjectName Field is Required.");
            return false;
        }
        return true;
    }
}
