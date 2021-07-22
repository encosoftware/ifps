#!/bin/bash
set -e

printf "Sales SQL Server"
docker-compose -f ../docker/docker-compose.yml -f ../docker/docker-compose.override.yml -f ../docker/docker-compose.cypress.yml logs --no-color ifps.sales.sqlserver
printf "Sales API:"
docker-compose -f ../docker/docker-compose.yml -f ../docker/docker-compose.override.yml -f ../docker/docker-compose.cypress.yml logs --no-color ifps.sales.api 
printf "Sales UI:"
docker-compose -f ../docker/docker-compose.yml -f ../docker/docker-compose.override.yml -f ../docker/docker-compose.cypress.yml logs --no-color ifps.sales.ui 
printf "Sales Cypress:"
docker-compose -f ../docker/docker-compose.yml -f ../docker/docker-compose.override.yml -f ../docker/docker-compose.cypress.yml logs --no-color ifps.sales.cypress


printf "Factory SQL Server"
docker-compose -f ../docker/docker-compose.yml -f ../docker/docker-compose.override.yml -f ../docker/docker-compose.cypress.yml logs --no-color ifps.factory.sqlserver
printf "Factory API:"
docker-compose -f ../docker/docker-compose.yml -f ../docker/docker-compose.override.yml -f ../docker/docker-compose.cypress.yml logs --no-color ifps.factory.api 
printf "Factory UI:"
docker-compose -f ../docker/docker-compose.yml -f ../docker/docker-compose.override.yml -f ../docker/docker-compose.cypress.yml logs --no-color ifps.factory.ui 


printf "Integration API:"
docker-compose -f ../docker/docker-compose.yml -f ../docker/docker-compose.override.yml -f ../docker/docker-compose.cypress.yml logs --no-color ifps.integration.api 