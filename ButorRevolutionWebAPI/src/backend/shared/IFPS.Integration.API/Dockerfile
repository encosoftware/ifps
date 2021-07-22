FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["shared/IFPS.Integration.API/IFPS.Integration.API.csproj", "shared/IFPS.Integration.API/"]
COPY ["shared/IFPS.Integration.Models/IFPS.Integration.Application.csproj", "shared/IFPS.Integration.Models/"]
COPY ["lib/ENCO.DDD.EntityFrameworkCore.Relational/ENCO.DDD.EntityFrameworkCore.Relational.csproj", "lib/ENCO.DDD.EntityFrameworkCore.Relational/"]
COPY ["lib/ENCO.DDD/ENCO.DDD.csproj", "lib/ENCO.DDD/"]
COPY ["sales/IFPS.Sales.Application/IFPS.Sales.Application.csproj", "sales/IFPS.Sales.Application/"]
COPY ["sales/IFPS.Sales.Domain/IFPS.Sales.Domain.csproj", "sales/IFPS.Sales.Domain/"]
COPY ["sales/IFPS.Sales.EF/IFPS.Sales.EF.csproj", "sales/IFPS.Sales.EF/"]
COPY ["factory/IFPS.Factory.Application/IFPS.Factory.Application.csproj", "factory/IFPS.Factory.Application/"]
COPY ["factory/IFPS.Factory.Domain/IFPS.Factory.Domain.csproj", "factory/IFPS.Factory.Domain/"]
COPY ["factory/IFPS.Factory.EF/IFPS.Factory.EF.csproj", "factory/IFPS.Factory.EF/"]

RUN dotnet restore "shared/IFPS.Integration.API/IFPS.Integration.API.csproj"
COPY . .
WORKDIR /src/shared/IFPS.Integration.API/
RUN dotnet build "IFPS.Integration.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IFPS.Integration.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IFPS.Integration.API.dll"]
