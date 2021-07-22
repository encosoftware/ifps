#!/bin/bash
set -e

docker-compose -f ../docker/docker-compose.yml -f ../docker/docker-compose.override.yml -f ../docker/docker-compose.cypress.yml build # --no-cache --force-rm 
