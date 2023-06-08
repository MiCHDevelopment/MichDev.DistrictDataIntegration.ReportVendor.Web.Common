using System.Collections.Generic;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Requests
{
  /// <summary>
  /// Represents a request for the users selections of report parameters.
  /// </summary>
  public class ReportStateRequest
  {
    /// <summary>
    /// <para>
    /// Points the Report Vendor API to the report for which this request is made.
    /// </para>
    /// <para>
    /// This will be an ordered list of category internalIds that point to a sub-category
    /// in the tree, followed by a report internalId. The first item in the list will be
    /// the root category, the last item in the list will be the report for which this
    /// request is made. All items in the middle are the sub-categories that act as a
    /// navigation path to the selected report.
    /// </para>
    /// </summary>
    /// <example>
    /// [ rootCategoryId, mySubCategoryA, mySubSubCategoryB, mySubSubSubCategoryC, myReportId ]
    /// </example>
    public IEnumerable<string> PathToReport { get; set; } = new List<string>();
    
    /// <summary>
    /// The parameter states and options for the report after a report state request
    /// has been processed. Any cascading effects on parameters will be reflected in
    /// these parameters.
    /// </summary>
    public ReportStateParameterSetRequest Parameters { get; set; } = new ReportStateParameterSetRequest();
  }
}