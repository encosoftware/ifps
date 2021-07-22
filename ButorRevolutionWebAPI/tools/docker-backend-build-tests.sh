#!/bin/bash
set -e

docker-compose -f ../docker/docker-compose-backend.tests.yml build #--force-rm
