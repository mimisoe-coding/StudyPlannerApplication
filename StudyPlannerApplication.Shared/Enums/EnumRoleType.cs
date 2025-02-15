using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlannerApplication.Shared.Enums
{
    public enum EnumRoleType
    {
        Default,
        [Description("S001")]Student,
        [Description("A001")]Admin,
    }
}
