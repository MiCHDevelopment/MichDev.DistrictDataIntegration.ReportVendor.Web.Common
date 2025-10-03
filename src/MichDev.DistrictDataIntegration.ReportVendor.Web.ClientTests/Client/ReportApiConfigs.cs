using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.ClientTests.Client
{
  public class ReportApiConfigs
  {
    public string ReportApiUrlBase { get; set; }
    public AuthConfigs Auth { get; set; }

    public ReportApiConfigs(
      string reportApiUrlBase,
      AuthConfigs auth)
    {
      ReportApiUrlBase = reportApiUrlBase;
      Auth = auth;
    }

    public ReportApiConfigs()
    {
      ReportApiUrlBase = string.Empty;
      Auth = new AuthConfigs();
    }

    public static async Task<ReportApiConfigs> Load()
    {
      /*
      The config file must be a JSON file with the following structure:
      {
        "reportApiUrlBase": "...",
        "auth": {
          "tokenUrl": "...",
          "clientId": "...",
          "clientSecret": "...",
          "scopes": [ "..." ]
        }
      }
      */
      string configFilePath = "path_to_your_credentials.json";
      string configJson = await File.ReadAllTextAsync(configFilePath);
      ReportApiConfigs? configs = JsonConvert.DeserializeObject<ReportApiConfigs>(configJson);
      return configs!;
    }
  }
}