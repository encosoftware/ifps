FROM mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04 AS base
COPY --chown=mssql:root ["tools/dbcommand.sh","tools/genDbInitQuery.sh","tools/entrypoint.sh", "/home/sql/"]

FROM mcr.microsoft.com/dotnet/core/sdk:2.2.402-stretch AS script-gen
WORKDIR /src
COPY . .
RUN dotnet ef migrations add "Init" --startup-project  factory/IFPS.Factory.API/IFPS.Factory.API.csproj --project factory/IFPS.Factory.EF/IFPS.Factory.EF.csproj -c IFPSFactoryContext --configuration Release -v
RUN dotnet ef migrations script --startup-project factory/IFPS.Factory.API/IFPS.Factory.API.csproj --project factory/IFPS.Factory.EF/IFPS.Factory.EF.csproj -c IFPSFactoryContext -o  updatedb.sql --configuration Release -v

FROM base AS final
COPY --from=script-gen /src/updatedb.sql /home/sql/updatedb.sql
ENTRYPOINT [ "/bin/bash", "/home/sql/entrypoint.sh" ]
CMD [ "/opt/mssql/bin/sqlservr" ]