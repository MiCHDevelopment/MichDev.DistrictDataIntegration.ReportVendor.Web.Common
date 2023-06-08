using System;
using System.Collections.Generic;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Responses.Settings
{
  /// <summary>
  /// <para>
  /// Represents the selection(s) and available options or ranges for a
  /// specific setting.
  /// </para>
  /// <para>
  /// Not all properties in this class are used by all types of settings. For each
  /// setting, certain properties will be null while other won't be. For a list of
  /// which of these properties are used by each type of setting, see the documentation
  /// of each type designator in <see cref="ReportSettingTypes" />.
  /// </para>
  /// </summary>
  public class ReportSettingStateResponse
  {
    /// <summary>
    /// The type of interface to be used for this setting. Values of this field
    /// must be one of the values in <see cref="ReportSettingTypes" />.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// <para>
    /// The value for this setting that was selected by the user. If this setting
    /// changed because of cascading values while processing the report state
    /// request, this will be reset to the default value for this setting (such as
    /// the first available option).
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
    /// The list of values for this setting that was selected by the user. If this setting
    /// changed because of cascading values while processing the report state
    /// request, this will be reset to a set of the default values for this setting (such as
    /// the first available option).
    /// </para>
    /// <para>
    /// This property only applies to settings of with type
    /// <see cref="ReportSettingTypes.TypeMultiSelect" />.
    /// If this setting has a different type, this property will be null.
    /// </para>
    /// </summary>
    public IEnumerable<string>? SelectedValues { get; set; }

    /// <summary>
    /// <para>
    /// The list of options that are available for the user to select for this setting.
    /// </para>
    /// <para>
    /// This property only applies to settings of the following types:
    /// <see cref="ReportSettingTypes.TypeSingleSelect" />,
    /// <see cref="ReportSettingTypes.TypeMultiSelect" />.
    /// If this setting has a different type, this property will be null.
    /// </para>
    /// </summary>
    public IEnumerable<ReportSettingOptionResponse>? Options { get; set; }
    
    /// <summary>
    /// <para>
    /// An object that represents the range of numbers that can be chosen for this setting.
    /// </para>
    /// <para>
    /// This property only applies to settings of with type
    /// <see cref="ReportSettingTypes.TypeNumberRange" />.
    /// If this setting has a different type, this property will be null.
    /// </para>
    /// </summary>
    public ReportSettingNumberRangeResponse? NumberRange { get; set; }
    
    /// <summary>
    /// <para>
    /// An object that represents the range of dates that can be chosen for this setting.
    /// </para>
    /// <para>
    /// This property only applies to settings of with type
    /// <see cref="ReportSettingTypes.TypeDateRange" />.
    /// If this setting has a different type, this property will be null.
    /// </para>
    /// </summary>
    public ReportSettingDateRangeResponse? DateRange { get; set; }
  }
}