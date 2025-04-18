#!/bin/bash
set -e

ARG_VERSION=$1
ARG_DESCRIPTION=$2

usage() {
  EXECUTABLE="publish_web_sdk.sh"
  echo -e "Usage:\n\t./$EXECUTABLE VERSION DESCRIPTION\n"
  echo -e "Arguments:"
  echo -e "\tVERSION\t\tnew version of the nuget package to deploy | format: #.#.#    | example: 4.1.1"
  echo -e "\tDESCRIPTION\tdescription of what the package changes    | format: <string> | example: \"Add some changes to some class\""
  echo -e "\nExample usage:\n\t./$EXECUTABLE 4.1.1 \"Add some changes to some class\""
}

checkArgs() {
  if [ -z "$ARG_VERSION" ];
  then
    echo -e "Missing value for argument VERSION.\n"
    usage
    return 1
  elif [ -z "$ARG_DESCRIPTION" ];
  then
    echo -e "Missing value for argument DESCRIPTION.\n"
    usage
    return 1
  fi
}

if ! checkArgs;
then exit
fi

dotnet pack src/MichDev.DistrictDataIntegration.ReportVendor.Web.Common/MichDev.DistrictDataIntegration.ReportVendor.Web.Common.csproj --configuration Release
dotnet nuget push --source michdev src/MichDev.DistrictDataIntegration.ReportVendor.Web.Common/bin/Release/MichDev.DistrictDataIntegration.ReportVendor.Web.Common.${ARG_VERSION}.nupkg

DEPENDENCIES="$(dotnet list src/MichDev.DistrictDataIntegration.ReportVendor.Web.Common package)"

git tag -a DDIS-ReportVender-WebSDK-v$ARG_VERSION -m "$ARG_DESCRIPTION" -m "Dependencies:" -m "$DEPENDENCIES"
git push origin DDIS-ReportVender-WebSDK-v$ARG_VERSION