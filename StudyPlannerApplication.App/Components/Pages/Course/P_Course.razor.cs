namespace StudyPlannerApplication.App.Components.Pages.Course;

public partial class P_Course
{
    [Inject] private ILogger<P_Course> _logger { get; set; }
    private EnumFormType _formType = EnumFormType.List;
    private UserSessionModel _userSession = new();
    private PageSettingModel ps = new();
    private CourseRequestModel _reqModel = new();
    private Result<CourseResponseModel> _resModel = new();
    private IEnumerable<SubjectDataModel> lstSubject;
    private List<SelectListModel> lstStatus = new();
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
            await GetSubjectList();
            GetStatusTypeTypeList();
            await List(ps);
            StateHasChanged();
        }
    }

    async Task Back()
    {
        _reqModel = new CourseRequestModel();
        visible = false;
        ps = new PageSettingModel(1, 10);
        await List(ps);
        _formType = EnumFormType.List;
        StateHasChanged();
    }

    private async Task GetSubjectList()
    {
        try
        {
            _reqModel.CurrentUserId = _userSession.UserId;
            var result = await _subjectService.GetSubjectList(_reqModel.CurrentUserId);
            lstSubject = result.Data.SubjectList;
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
            _resModel = await _courseService.List(_reqModel);
            if (!_resModel.Success)
            {
                await _injectService.ErrorMessage(_resModel.Message);
                return;
            }
            count = _resModel.Data.PageSetting.TotalPageNo;
            visible = false;
            _formType = EnumFormType.List;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    private void GetStatusTypeTypeList()
    {
        lstStatus = new List<SelectListModel>
        {
            new() {Value = EnumStatusType.Pending.ToString()},
            new() {Value = EnumStatusType.Done.ToString() }
        };
    }

    private void Create()
    {
        _formType = EnumFormType.Register;
        _reqModel = new CourseRequestModel();
    }

    async Task Save()
    {
        try
        {
            if (!await CheckRequiredFields(_reqModel)) return;

            _reqModel.CurrentUserId = _userSession.UserId;
            if (_reqModel.CourseId > 0)
            {
                _resModel = await _courseService.Update(_reqModel);
            }
            else
            {
                _resModel = await _courseService.Create(_reqModel);
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

    async Task<bool> CheckRequiredFields(CourseRequestModel _reqModel)
    {
        if (string.IsNullOrEmpty(_reqModel.CourseName))
        {
            await _injectService.ErrorMessage("CourseName Field is Required.");
            return false;
        }
        if (string.IsNullOrEmpty(_reqModel.Status))
        {
            await _injectService.ErrorMessage("Status Field is Required.");
            return false;
        }
        if (string.IsNullOrEmpty(_reqModel.SubjectCode))
        {
            await _injectService.ErrorMessage("Subject Field is Required.");
            return false;
        }

        return true;
    }

    private async Task PageChanged(int i)
    {
        ps.PageNo = i;
        await List(ps);
    }

    private async Task Edit(int id)
    {
        try
        {
            _reqModel.CourseId = id;
            var result = await _courseService.Edit(id);
            if (!result.Success)
            {
                await _injectService.ErrorMessage(result.Message);
                return;
            }
            _reqModel.CourseName = result.Data.Course.CourseName;
            _reqModel.Description = result.Data.Course.Description;
            _reqModel.SubjectCode = result.Data.Course.SubjectCode;
            _reqModel.Status = result.Data.Course.Status;
            _reqModel.DueDate = result.Data.Course.DueDate;
            _formType = EnumFormType.Edit;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    private async Task Detail(int id)
    {
        try
        {
            _reqModel.CourseId = id;
            var result = await _courseService.Edit(id);
            if (!result.Success)
            {
                await _injectService.ErrorMessage(result.Message);
                return;
            }
            _reqModel.CourseName = result.Data.Course.CourseName;
            _reqModel.Description = result.Data.Course.Description;
            _reqModel.SubjectCode = result.Data.Course.SubjectCode;
            _reqModel.SubjectCode = result.Data.Course.SubjectCode;
            _reqModel.Status = result.Data.Course.Status;
            _reqModel.Status = result.Data.Course.Status;
            _reqModel.DueDate = result.Data.Course.DueDate;
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
        try
        {
            bool isConfirm = await _injectService.ConfirmMessageBox("Are you sure you want to delete");
            if (!isConfirm) return;
            _reqModel.CurrentUserId = _userSession.UserId;
            _reqModel.CourseId = id;
            var result = await _courseService.Delete(_reqModel);
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
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    private MudBlazor.Color GetStatus(string status)
    {
        return status switch
        {
            "Done" => Color.Success,
            "Pending" => Color.Warning,
            _ => Color.Default
        };
    }

}
