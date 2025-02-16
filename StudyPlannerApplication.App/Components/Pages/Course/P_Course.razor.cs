namespace StudyPlannerApplication.App.Components.Pages.Course;

public partial class P_Course
{
    private EnumFormType _formType = EnumFormType.List;
    private UserSessionModel _userSession = new();
    private PageSettingModel ps = new();
    private CourseRequestModel _reqModel = new();
    private CourseResponseModel _resModel = new();
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
            //await List(ps);
            StateHasChanged();
        }
    }

    async Task Back()
    {
        _reqModel = new CourseRequestModel();
        visible = false;
        ps = new PageSettingModel(1, 10);
        //await List(ps);
        _formType = EnumFormType.List;
        StateHasChanged();
    }

    private async Task GetSubjectList()
    {
        _reqModel.CurrentUserId = _userSession.UserId;
        var result = await _subjectService.GetSubjectList(_reqModel.CurrentUserId);
        lstSubject = result.SubjectList;
    }

    async Task List(PageSettingModel ps)
    {
        _reqModel.PageSetting = ps;
        _reqModel.CurrentUserId = _userSession.UserId;
        _resModel = await _courseService.List(_reqModel);
        if (!_resModel.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(_resModel.Response.Message);
            return;
        }
        count = _resModel.PageSetting.TotalPageNo;
        _formType = EnumFormType.List;
        StateHasChanged();
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
        if (!await CheckRequiredFields(_reqModel)) return;

        _reqModel.CurrentUserId = _userSession.UserId;
        if (_reqModel.CourseId > 0)
        {
            //_resModel = await _courseService.Update(_reqModel);
        }
        else
        {
            _resModel = await _courseService.Create(_reqModel);
        }

        if (!_resModel.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(_resModel.Response.Message);
            return;
        }
        await _injectService.SuccessMessage(_resModel.Response.Message);
        ps = new PageSettingModel(1, 10);
        await List(ps);
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
}
