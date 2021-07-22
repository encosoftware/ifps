FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
# install System.Drawing native dependencies
RUN apt-get update && apt-get install -y --allow-unauthenticated libc6-dev libgdiplus libx11-dev && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["factory/IFPS.Factory.API/IFPS.Factory.API.csproj", "factory/IFPS.Factory.API/"]
COPY ["lib/ENCO.DDD.EntityFrameworkCore.Relational/ENCO.DDD.EntityFrameworkCore.Relational.csproj", "lib/ENCO.DDD.EntityFrameworkCore.Relational/"]
COPY ["lib/ENCO.DDD/ENCO.DDD.csproj", "lib/ENCO.DDD/"]
COPY ["factory/IFPS.Factory.Application/IFPS.Factory.Application.csproj", "factory/IFPS.Factory.Application/"]
COPY ["factory/IFPS.Factory.Domain/IFPS.Factory.Domain.csproj", "factory/IFPS.Factory.Domain/"]
COPY ["factory/IFPS.Factory.EF/IFPS.Factory.EF.csproj", "factory/IFPS.Factory.EF/"]
RUN dotnet restore "factory/IFPS.Factory.API/IFPS.Factory.API.csproj"
COPY . .
WORKDIR /src/factory/IFPS.Factory.API/
RUN dotnet build "IFPS.Factory.API.csproj" -c Release  -o /app 

FROM build AS publish
WORKDIR /src/factory/IFPS.Factory.API/
RUN dotnet publish "IFPS.Factory.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY factory/IFPS.Factory.API/AppData ./AppData_default/
RUN echo "/app/AppData_default/Optimizer/exe/lib" > /etc/ld.so.conf.d/or-tools-lib.conf && ldconfig
RUN chmod 774 /app/AppData_default/Optimizer/exe/*.o
RUN mkdir -p /cnc/01 && mkdir /cnc/02 && mkdir /cnc/03 && mkdir /cnc/04

COPY factory/IFPS.Factory.API/startup.sh .
RUN chmod 774 ./startup.sh

ENTRYPOINT ["bash", "./startup.sh"]
