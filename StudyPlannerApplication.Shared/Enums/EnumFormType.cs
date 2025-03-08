using System.ComponentModel;

namespace StudyPlannerApplication.Shared.Enums;

public enum EnumFormType
{
    Default,
    Create,
    [Description("Update")] Edit,
    [Description("Save")] Register,
    Detail,
    List
}

public enum EnumSignInFormType
{
    Default,
    SignIn,
    Register,
    ForgotPassword
}