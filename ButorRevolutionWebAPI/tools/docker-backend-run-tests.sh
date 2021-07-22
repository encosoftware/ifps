#!/bin/bash
set -e

docker-compose -f ../docker/docker-compose-backend.tests.yml up -d
docker exec -w /src/test/IFPS.Sales.UnitTests/ ifps.api.test dotnet test IFPS.Sales.UnitTests.csproj -r /testResults  --logger:trx -v n -c $1
docker exec -w /src/test/IFPS.Sales.FunctionalTests/ ifps.api.test dotnet test IFPS.Sales.FunctionalTests.csproj -r /testResults  --logger:trx -v n -c $1
docker exec -w /src/test/IFPS.Factory.FunctionalTests/ ifps.api.test dotnet test IFPS.Factory.FunctionalTests.csproj -r /testResults  --logger:trx -v n -c $1
