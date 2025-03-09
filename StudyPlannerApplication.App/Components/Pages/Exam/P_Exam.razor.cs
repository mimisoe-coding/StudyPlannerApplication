using StudyPlannerApplication.Domain.Models;

namespace StudyPlannerApplication.App.Components.Pages.Exam;

public partial class P_Exam
{
    [Inject] private ILogger<P_Exam> _logger { get; set; }
    private EnumFormType _formType = EnumFormType.List;
    private ExamRequestModel _reqModel = new();
    private Result<ExamResponseModel> _resModel = new();
    private UserSessionModel _userSession = new();
    private NotificationResponseModel _notiData = new();
    private PageSettingModel ps = new();
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

    private void GetStatusTypeTypeList()
    {
        lstStatus = new List<SelectListModel>
        {
            new() {Value = EnumStatusType.Pending.ToString()},
            new() {Value = EnumStatusType.Done.ToString() }
        };
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

    private void Create()
    {
        _formType = EnumFormType.Register;
        _reqModel = new ExamRequestModel();
        StateHasChanged();
    }

    async Task List(PageSettingModel ps)
    {
        try
        {
            _reqModel.PageSetting = ps;
            _reqModel.CurrentUserId = _userSession.UserId;
            _resModel = await _examService.List(_reqModel);
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

    async Task Save()
    {
        try
        {
            if (!await CheckRequiredFields(_reqModel)) return;

            _reqModel.CurrentUserId = _userSession.UserId;
            if (_reqModel.ExamId > 0)
            {
                _resModel = await _examService.Update(_reqModel);
            }
            else
            {
                _resModel = await _examService.Create(_reqModel);
            }

            if (!_resModel.Success)
            {
                await _injectService.ErrorMessage(_resModel.Message);
                return;
            }
            await _injectService.SuccessMessage(_resModel.Message);
            await Notification();
            ps = new PageSettingModel(1, 10);

            await List(ps);
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    async Task Back()
    {
        _reqModel = new ExamRequestModel();
        visible = false;
        ps = new PageSettingModel(1, 10);
        await List(ps);
        _formType = EnumFormType.List;
        StateHasChanged();
    }

    async Task<bool> CheckRequiredFields(ExamRequestModel _reqModel)
    {
        if (string.IsNullOrEmpty(_reqModel.SubjectCode))
        {
            await _injectService.ErrorMessage("Subject Field is Required.");
            return false;
        }
        if (_reqModel.Duration.Hour == 0 && _reqModel.Duration.Minute == 0)
        {
            await _injectService.ErrorMessage("Duration Field is Required.");
            return false;
        }
        if (string.IsNullOrEmpty(_reqModel.Status))
        {
            await _injectService.ErrorMessage("Status Field is Required.");
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
            _reqModel.ExamId = id;
            var result = await _examService.Edit(id);
            if (!result.Success)
            {
                await _injectService.ErrorMessage(result.Message);
                return;
            }
            _reqModel.Description = result.Data.ExamData.Description;
            _reqModel.SubjectCode = result.Data.ExamData.SubjectCode;
            _reqModel.Status = result.Data.ExamData.Status;
            _reqModel.DueDate = result.Data.ExamData.DueDate;
            _reqModel.Duration = result.Data.ExamData.Duration;
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
            _reqModel.ExamId = id;
            var result = await _examService.Edit(id);
            if (!result.Success)
            {
                await _injectService.ErrorMessage(result.Message);
                return;
            }
            _reqModel.Description = result.Data.ExamData.Description;
            _reqModel.SubjectCode = result.Data.ExamData.SubjectCode;
            _reqModel.Status = result.Data.ExamData.Status;
            _reqModel.DueDate = result.Data.ExamData.DueDate;
            _reqModel.Duration = result.Data.ExamData.Duration;
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
            _reqModel.ExamId = id;
            var data = await _examService.Delete(_reqModel);
            if (!data.Success)
            {
                await _injectService.ErrorMessage(data.Message);
                return;
            }
            await _injectService.SuccessMessage(data.Message);
            await Notification();
            ps = new();
            await List(ps);
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    async Task Notification()
    {
        NotificationRequestModel reqModel = new NotificationRequestModel
        {
            CurrentUserId = _userSession.UserId
        };
        _notiData = await _notificationService.GetAllNotification(reqModel);
        if (!_notiData.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(_notiData.Response.Message);
            return;
        }

        _notificationStateContainer.NotificationCount = _notiData.NotiList.Count;
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
