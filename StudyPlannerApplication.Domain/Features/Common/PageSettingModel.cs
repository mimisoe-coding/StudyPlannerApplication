using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlannerApplication.Domain.Features.Common
{
    public class PageSettingModel
    {
        public PageSettingModel() { }
        public PageSettingModel(int pageNo, int pageSize)
        {
            PageNo = pageNo;
            PageSize = pageSize;
        }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int SkipRowCount => (PageNo - 1) * PageSize;
    }
}
