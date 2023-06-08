namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common
{
  /// <summary>
  /// The report navigation endpoint is used to fetch reports in their respective categories.
  /// The response of this endpoint is in a tree like structure where each node is ether a
  /// category or specific report.
  /// </summary>
  public static class ReportNavigationEndpoint
  {
    /// <summary>
    /// The report navigation endpoint after the API base URL segment.
    /// </summary>
    public const string Endpoint = "/reports/report-navigation";

    /// <summary>
    /// <para>
    /// Represents the query parameter that contains a filter on report category.
    /// </para>
    /// <para>
    /// This query param will be in the format of a comma separated, ordered list of category
    /// internalIds that point to a sub-category in the tree. The first item in the list
    /// will be the root category, the last item in the list will be the category that is the
    /// parent of the report or category designated in selectedNodeId.
    /// </para>
    /// </summary>
    public const string QueryParamPathToCategory = "pathToCategory";

    /// <summary>
    /// <para>
    /// Represents the query parameter that contains a filter on report or report category
    /// starting from the pathToCategory.
    /// </para>
    /// <para>
    /// This query param will be in the format of a report or category internalId which is
    /// to be the root category or report in the response.
    /// </para>
    /// </summary>
    public const string QueryParamSelectedNodeId = "selectedNodeId";
  }
}