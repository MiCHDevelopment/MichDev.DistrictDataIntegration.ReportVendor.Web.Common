# MichDev.DistrictDataIntegration.ReportVendor.Web.Common

**Latest Release:** 1.6.0

ASP.NET Core framework for easily developing Report Vendor APIs or Clients.

This library contains Endpoint constants and API contracts in order to provide a consistent standard upon which the Report Vendor API and consumers may agree.

## Installation

```bash
$ dotnet add package MichDev.DistrictDataIntegration.ReportVendor.Web.Common --version {version}
```

## Library Architecture

There are 2 main namespaces: Contracts and Endpoints.

The Contracts namespace contains all data structures which define the schema of URLs. Each endpoint in the Report Vendor API has its own corresponding namespace in the Contracts namespace which contains all contracts for that endpoint.

The Endpoints namespace contains constants related to routing as well as any query parameter constants. Each endpoint in the Report Vendor API has its own corresponding static class with all URL and query param constants defined in it.
