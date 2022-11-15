FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["appointmentLookupApi/appointmentLookupApi.csproj", "appointmentLookupApi/"]
RUN dotnet restore "appointmentLookupApi/appointmentLookupApi.csproj"
COPY . .
WORKDIR "/src/appointmentLookupApi"
RUN dotnet build "appointmentLookupApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "appointmentLookupApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 7253
ENTRYPOINT ["dotnet", "appointmentLookupApi.dll"]