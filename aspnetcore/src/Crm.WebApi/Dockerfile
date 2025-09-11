FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Crm.WebApi/Crm.WebApi.csproj", "src/Crm.WebApi/"]
COPY ["shared/src/Astra.AspNetCore/Astra.AspNetCore.csproj", "shared/src/Astra.AspNetCore/"]
COPY ["shared/src/Astra.Host.Shared/Astra.Host.Shared.csproj", "shared/src/Astra.Host.Shared/"]
COPY ["shared/src/Astra.Common/Astra.Common.csproj", "shared/src/Astra.Common/"]
COPY ["src/Crm.Admin.Application/Crm.Admin.Application.csproj", "src/Crm.Admin.Application/"]
COPY ["shared/src/Astra.Application/Astra.Application.csproj", "shared/src/Astra.Application/"]
COPY ["shared/src/Astra.Application.Contracts/Astra.Application.Contracts.csproj", "shared/src/Astra.Application.Contracts/"]
COPY ["shared/src/Astra.Domain/Astra.Domain.csproj", "shared/src/Astra.Domain/"]
COPY ["shared/src/Astra.Domain.Shared/Astra.Domain.Shared.csproj", "shared/src/Astra.Domain.Shared/"]
COPY ["src/Crm.Admin.Application.Contracts/Crm.Admin.Application.Contracts.csproj", "src/Crm.Admin.Application.Contracts/"]
COPY ["src/Crm.Domain.Shared/Crm.Domain.Shared.csproj", "src/Crm.Domain.Shared/"]
COPY ["src/Crm.Domain/Crm.Domain.csproj", "src/Crm.Domain/"]
COPY ["src/Crm.Admin.HttpApi/Crm.Admin.HttpApi.csproj", "src/Crm.Admin.HttpApi/"]
COPY ["src/Crm.Application/Crm.Application.csproj", "src/Crm.Application/"]
COPY ["src/Crm.Application.Contracts/Crm.Application.Contracts.csproj", "src/Crm.Application.Contracts/"]
COPY ["src/Crm.EntityFrameworkCore/Crm.EntityFrameworkCore.csproj", "src/Crm.EntityFrameworkCore/"]
COPY ["shared/src/Astra.EntityFrameworkCore/Astra.EntityFrameworkCore.csproj", "shared/src/Astra.EntityFrameworkCore/"]
COPY ["src/Crm.HttpApi/Crm.HttpApi.csproj", "src/Crm.HttpApi/"]
RUN dotnet restore "src/Crm.WebApi/Crm.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Crm.WebApi"
RUN dotnet build "./Crm.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Crm.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crm.WebApi.dll"]
