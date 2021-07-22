#!/bin/bash
set -e

docker-compose -f ../docker/docker-compose-backend.tests.yml run dotnet ef --startup-project  /app/src/backend/sales/IFPS.Sales.API/IFPS.Sales.API.csproj --project /app/src/backend/sales/IFPS.Sales.EF/IFPS.Sales.EF.csproj migrations script -o  updatedb.sql
