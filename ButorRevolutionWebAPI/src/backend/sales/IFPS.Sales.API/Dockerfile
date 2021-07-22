FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["sales/IFPS.Sales.API/IFPS.Sales.API.csproj", "sales/IFPS.Sales.API/"]
COPY ["lib/ENCO.DDD.EntityFrameworkCore.Relational/ENCO.DDD.EntityFrameworkCore.Relational.csproj", "lib/ENCO.DDD.EntityFrameworkCore.Relational/"]
COPY ["lib/ENCO.DDD/ENCO.DDD.csproj", "lib/ENCO.DDD/"]
COPY ["sales/IFPS.Sales.Application/IFPS.Sales.Application.csproj", "sales/IFPS.Sales.Application/"]
COPY ["sales/IFPS.Sales.Domain/IFPS.Sales.Domain.csproj", "sales/IFPS.Sales.Domain/"]
COPY ["sales/IFPS.Sales.EF/IFPS.Sales.EF.csproj", "sales/IFPS.Sales.EF/"]
RUN dotnet restore "sales/IFPS.Sales.API/IFPS.Sales.API.csproj"
COPY . .
WORKDIR "/src/sales/IFPS.Sales.API"
RUN dotnet build "IFPS.Sales.API.csproj" -c Release -o /app

FROM build AS publish
WORKDIR "/src/sales/IFPS.Sales.API"
RUN dotnet publish "IFPS.Sales.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY sales/IFPS.Sales.API/AppData ./AppData_default/
COPY sales/IFPS.Sales.API/startup.sh .
RUN chmod 774 ./startup.sh
ENTRYPOINT ["bash", "./startup.sh"]
