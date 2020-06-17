FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./ResidentVulnerabilitiesDataPipeline.sln ./
COPY ./ResidentVulnerabilitiesDataPipeline/ResidentVulnerabilitiesDataPipeline.csproj ./ResidentVulnerabilitiesDataPipeline/
COPY ./ResidentVulnerabilitiesDataPipeline.Tests/ResidentVulnerabilitiesDataPipeline.Tests.csproj ./ResidentVulnerabilitiesDataPipeline.Tests/

RUN dotnet restore ./ResidentVulnerabilitiesDataPipeline/ResidentVulnerabilitiesDataPipeline.csproj
RUN dotnet restore ./ResidentVulnerabilitiesDataPipeline.Tests/ResidentVulnerabilitiesDataPipeline.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out ResidentVulnerabilitiesDataPipeline.Tests/ResidentVulnerabilitiesDataPipeline.Tests.csproj

CMD dotnet test
