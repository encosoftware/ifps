#!/bin/bash
set -e
docker cp ifps.api.test:/testResults $1
ls $1
docker-compose -f ../docker/docker-compose-backend.tests.yml down
