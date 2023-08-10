FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env

WORKDIR /src
COPY *.csproj .
RUN dotnet restore --use-current-runtime
COPY . .
RUN dotnet publish --use-current-runtime --self-contained false --no-restore -o /publish

FROM mcr.microsoft.com/dotnet/runtime:7.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .