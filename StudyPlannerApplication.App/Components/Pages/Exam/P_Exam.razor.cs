using StudyPlannerApplication.Domain.Features.Course;
using StudyPlannerApplication.Domain.Features.Exam;
using StudyPlannerApplication.Domain.Features.Notification;
using StudyPlannerApplication.Domain.Features.Subject;

namespace StudyPlannerApplication.App.Components.Pages.Exam;

public partial class P_Exam
{
    private EnumFormType _formType = EnumFormType.List;
    private ExamRequestModel _reqModel = new();
    private ExamResponseModel _resModel = new();
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
        _reqModel.CurrentUserId = _userSession.UserId;
        var result = await _subjectService.GetSubjectList(_reqModel.CurrentUserId);
        lstSubject = result.SubjectList;
    }

    private void Create()
    {
        _formType = EnumFormType.Register;
        _reqModel = new ExamRequestModel();
        StateHasChanged();
    }

    async Task List(PageSettingModel ps)
    {
        _reqModel.PageSetting = ps;
        _reqModel.CurrentUserId = _userSession.UserId;
        _resModel = await _examService.List(_reqModel);
        if (!_resModel.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(_resModel.Response.Message);
            return;
        }
        count = _resModel.PageSetting.TotalPageNo;
        visible = false;
        _formType = EnumFormType.List;
        StateHasChanged();
    }

    async Task Save()
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

        if (!_resModel.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(_resModel.Response.Message);
            return;
        }
        await _injectService.SuccessMessage(_resModel.Response.Message);
        await Notification();
        ps = new PageSettingModel(1, 10);

        await List(ps);
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
        _reqModel.ExamId = id;
        var data = await _examService.Edit(id);
        if (!data.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(data.Response.Message);
            return;
        }
        _reqModel.Description = data.ExamData.Description;
        _reqModel.SubjectCode = data.ExamData.SubjectCode;
        _reqModel.Status = data.ExamData.Status;
        _reqModel.DueDate = data.ExamData.DueDate;
        _reqModel.Duration = data.ExamData.Duration;
        _formType = EnumFormType.Edit;
    }

    private async Task Detail(int id)
    {
        _reqModel.ExamId = id;
        var data = await _examService.Edit(id);
        if (!data.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(data.Response.Message);
            return;
        }
        _reqModel.Description = data.ExamData.Description;
        _reqModel.SubjectCode = data.ExamData.SubjectCode;
        _reqModel.Status = data.ExamData.Status;
        _reqModel.DueDate = data.ExamData.DueDate;
        _reqModel.Duration = data.ExamData.Duration;
        visible = true;
        _formType = EnumFormType.Detail;
    }

    private async Task Delete(int id)
    {
        bool isConfirm = await _injectService.ConfirmMessageBox("Are you sure you want to delete");
        if (!isConfirm) return;
        _reqModel.CurrentUserId = _userSession.UserId;
        _reqModel.ExamId = id;
        var data = await _examService.Delete(_reqModel);
        if (!data.Response.IsSuccess)
        {
            await _injectService.ErrorMessage(data.Response.Message);
            return;
        }
        await _injectService.SuccessMessage(data.Response.Message);
        await Notification();
        ps = new();
        await List(ps);
        StateHasChanged();
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

    private void OnSubjectCodeChange(object args)
    {
        _reqModel.SubjectCode = args?.ToString();
    }

    private void OnStatusChange(object args)
    {
        _reqModel.SubjectCode = args?.ToString();
    }
}
