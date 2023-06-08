using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.Auth
{
  /// <summary>
  /// A request contract used to request a JWT token which will grant authorization for
  /// subsequent requests to the External Report Vendor API.
  /// </summary>
  public class TokenRequest
  {
    /// <summary>
    /// The client id designating who is making the request.
    /// This value must be communicated between both the consumer party and the API party.
    /// </summary>
    [JsonProperty("client_id")]
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// The secret string corresponding to the ClientId, almost like a password.
    /// This value must be communicated between both the consumer party and the API party.
    /// </summary>
    [JsonProperty("client_secret")]
    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// The interface and flow which will be used to retrieve tokens. This should
    /// always be "client_credentials".
    /// </summary>
    [JsonProperty("grant_type")]
    [JsonPropertyName("grant_type")]
    public string GrantType { get; set; } = string.Empty;

    /// <summary>
    /// The list of scopes requested in the token. Scopes are essentially policy
    /// identifiers that grants access to a certain set of endpoints or features.
    /// For this API, the scope should be the 5 digit district code of the district
    /// to which the request is authenticating (remember, authorization tokens
    /// should only grant access to a single district). There may be other scopes
    /// but we will need to know what those are.
    /// 
    /// This field is a space separated string of scopes like "scope1 scope2 scope2".
    /// </summary>
    [JsonProperty("scope")]
    [JsonPropertyName("scope")]
    public string Scope { get; set; } = string.Empty;
  }
}