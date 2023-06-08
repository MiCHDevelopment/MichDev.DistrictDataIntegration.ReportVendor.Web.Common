using System.Collections.Generic;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportNavigation.Responses
{
  public class CategoryResponse : ReportNavigationTreeNodeResponse
  {
    public IEnumerable<CategoryResponse>? SubCategories { get; set; }
    public IEnumerable<ReportResponse>? Reports { get; set; }
  }
}