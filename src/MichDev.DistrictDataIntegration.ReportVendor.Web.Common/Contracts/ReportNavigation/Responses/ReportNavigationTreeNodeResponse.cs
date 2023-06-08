namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportNavigation.Responses
{
  public abstract class ReportNavigationTreeNodeResponse
  {
    public string InternalId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
  }
}