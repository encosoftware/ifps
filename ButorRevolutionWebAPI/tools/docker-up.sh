#!/bin/bash
set -e

docker-compose -f ../docker/docker-compose.yml -f ../docker/docker-compose.override.yml up -d
