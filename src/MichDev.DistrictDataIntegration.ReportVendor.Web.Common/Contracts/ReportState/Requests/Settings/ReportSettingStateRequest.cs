using System;
using System.Collections.Generic;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Requests.Settings
{
  /// <summary>
  /// <para>
  /// Represents the selection(s) for a specific setting.
  /// </para>
  /// <para>
  /// Not all properties in this class are used by all types of settings. For each
  /// setting, certain properties will be null while other won't be. For a list of
  /// which of these properties are used by each type of setting, see the documentation
  /// of each type designator in <see cref="ReportSettingTypes" />.
  /// </para>
  /// </summary>
  public class ReportSettingStateRequest
  {
    /// <summary>
    /// A programmatic ID used to identify this setting in the Report Vendor API.
    /// Consumers of the Report Vendor API will not depend on this value being in
    /// any specific format or anything. It's value is effectively arbitrary as long
    /// as it is consistent as an identifier.
    /// </summary>
    public string ParameterId { get; set; } = string.Empty;

    /// <summary>
    /// The type of interface to be used for this setting. Values of this field
    /// must be one of the values in <see cref="ReportSettingTypes" />.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// <para>
    /// The value for this setting that was selected by the user.
    /// </para>
    /// <para>
    /// This property only applies to settings of the following types:
    /// <see cref="ReportSettingTypes.TypeSingleSelect" />,
    /// <see cref="ReportSettingTypes.TypeNumberRange" />,
    /// <see cref="ReportSettingTypes.TypeDateRange" />.
    /// If this setting has a different type, this property will be null.
    /// </para>
    /// </summary>
    public string? SelectedValue { get; set; }
    
    /// <summary>
    /// <para>
    /// The list of values for this setting that was selected by the user.
    /// </para>
    /// <para>
    /// This property only applies to settings of with type
    /// <see cref="ReportSettingTypes.TypeMultiSelect" />.
    /// If this setting has a different type, this property will be null.
    /// </para>
    /// </summary>
    public IEnumerable<string>? SelectedValues { get; set; }
  }
}