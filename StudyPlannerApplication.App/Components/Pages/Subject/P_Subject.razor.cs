namespace StudyPlannerApplication.App.Components.Pages.Subject;

public partial class P_Subject
{
    private EnumFormType _formType = EnumFormType.List;
    private SubjectRequestModel _reqModel = new();
    private UserSessionModel _userSession = new();
    private Result<SubjectResponseModel> _resModel = new();
    private PageSettingModel ps = new();
    private int count;
    bool visible = false;

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
            await List(ps);
            StateHasChanged();
        }
    }

    private void Create()
    {
        _formType = EnumFormType.Register;
        _reqModel = new SubjectRequestModel();
    }

    private async Task Edit(int id)
    {
        _reqModel.SubjectId = id;
        var result = await _subjectService.Edit(id);
        if (!result.Success)
        {
            await _injectService.ErrorMessage(result.Message);
            return;
        }
        _reqModel.SubjectName = result.Data.Subject.SubjectName;
        _reqModel.Description = result.Data.Subject.Description;
        _formType = EnumFormType.Edit;
    }

    private async Task Detail(int id)
    {
        try
        {
            _reqModel.SubjectId = id;
            var result = await _subjectService.Edit(id);
            if (!result.Success)
            {
                await _injectService.ErrorMessage(result.Message);
                return;
            }
            _reqModel.SubjectName = result.Data.Subject.SubjectName;
            _reqModel.Description = result.Data.Subject.Description;
            visible = true;
            _formType = EnumFormType.Detail;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    private async Task Delete(int id)
    {
        bool isConfirm = await _injectService.ConfirmMessageBox("Are you sure you want to delete");
        if (!isConfirm) return;
        _reqModel.CurrentUserId = _userSession.UserId;
        _reqModel.SubjectId = id;
        var result = await _subjectService.Delete(_reqModel);
        if (!result.Success)
        {
            await _injectService.ErrorMessage(result.Message);
            return;
        }
        await _injectService.SuccessMessage(result.Message);
        ps = new();
        await List(ps);
        StateHasChanged();
    }

    async Task Back()
    {
        _reqModel = new SubjectRequestModel();
        visible = false;
        ps = new PageSettingModel(1, 10);
        await List(ps);
        _formType = EnumFormType.List;
        StateHasChanged();
    }

    async Task Save()
    {
        try
        {
            if (!await CheckRequiredFields(_reqModel)) return;

            _reqModel.CurrentUserId = _userSession.UserId;
            if (_reqModel.SubjectId > 0)
            {
                _resModel = await _subjectService.Update(_reqModel);
            }
            else
            {
                _resModel = await _subjectService.Create(_reqModel);
            }

            if (!_resModel.Success)
            {
                await _injectService.ErrorMessage(_resModel.Message);
                return;
            }
            await _injectService.SuccessMessage(_resModel.Message);
            ps = new PageSettingModel(1, 10);
            await List(ps);
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    async Task List(PageSettingModel ps)
    {
        try
        {
            _reqModel.PageSetting = ps;
            _reqModel.CurrentUserId = _userSession.UserId;
            _resModel = await _subjectService.List(_reqModel);
            if (!_resModel.Success)
            {
                await _injectService.ErrorMessage(_resModel.Message);
                return;
            }
            count = _resModel.Data.PageSetting.TotalPageNo;
            _formType = EnumFormType.List;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    async Task<bool> CheckRequiredFields(SubjectRequestModel _reqModel)
    {
        if (string.IsNullOrEmpty(_reqModel.SubjectName))
        {
            await _injectService.ErrorMessage("SubjectName Field is Required.");
            return false;
        }
        if (_reqModel.SubjectName.Length < 3)
        {
            await _injectService.ErrorMessage("SubjectName must contain at least 3 characters.");
            return false;
        }
        return true;
    }

    private async Task PageChanged(int i)
    {
        ps.PageNo = i;
        await List(ps);
    }
}
