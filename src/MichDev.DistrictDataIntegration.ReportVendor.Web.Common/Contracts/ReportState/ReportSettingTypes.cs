using System;
using System.Collections.Generic;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState
{
  /// <summary>
  /// Static class containing the different values that can be used in the
  /// "Type" property.
  /// </summary>
  public static class ReportSettingTypes
  {
    /// <summary>
    /// <para>
    /// Designator to denote that a setting is a basic, single select field. This will
    /// be presented as just a simple drop down in the UI.
    /// </para>
    /// <para>
    /// Settings of this type use the following properties:
    /// "Type",
    /// "SelectedValue",
    /// "Options",
    /// </para>
    /// </summary>
    public const string TypeSingleSelect = "single-select";
    
    /// <summary>
    /// <para>
    /// Designator to denote that a setting is a basic, select field but with the
    /// ability to select multiple values instead of just one. This will
    /// be presented as just a simple drop down in the UI and allow the user
    /// to select multiple values.
    /// </para>
    /// <para>
    /// Settings of this type use the following properties:
    /// "Type",
    /// "SelectedValues",
    /// "Options",
    /// </para>
    /// </summary>
    public const string TypeMultiSelect = "multi-select";
    
    /// <summary>
    /// <para>
    /// Designator to denote that a setting is a number field that may or may
    /// not have a minimum or maximum value. Only one number can be selected for
    /// these settings.
    /// </para>
    /// <para>
    /// Settings of this type use the following properties:
    /// "Type",
    /// "SelectedValue",
    /// "NumberRange" />,
    /// </para>
    /// </summary>
    public const string TypeNumberRange = "number-range";

    /// <summary>
    /// <para>
    /// Designator to denote that a setting is a date field that may or may
    /// not have a minimum or maximum value. Only one date can be selected for
    /// these settings.
    /// </para>
    /// <para>
    /// Settings of this type use the following properties:
    /// "Type",
    /// "SelectedValue",
    /// "DateRange" />,
    /// </para>
    /// </summary>
    public const string TypeDateRange = "date-range";
  }
}