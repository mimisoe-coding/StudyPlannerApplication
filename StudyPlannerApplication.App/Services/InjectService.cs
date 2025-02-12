
using Microsoft.JSInterop;

namespace StudyPlannerApplication.App.Services;

public class InjectService : IInjectService
{
    private readonly IJSRuntime _jSRuntime;

    public InjectService(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }

    public async Task ErrorMessage(string message)
    {
        await _jSRuntime.InvokeVoidAsync("errorMessage", message);
    }

    public async Task SuccessMessage(string message)
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("successMessage", message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());   
        }
    }

    public async Task<bool> ConfirmMessageBox(string message)
    {
        return await _jSRuntime.InvokeAsync<bool>("ConfirmMessageBox", message);
    }
}
